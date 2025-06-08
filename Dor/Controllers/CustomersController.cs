using AutoMapper;
using Dor.Dtos.Customer;
using Dor.Interfaces;
using Dor.Models;
using Dor.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dor.Controllers;

[ApiController]
[Route("api/customers")]
[Authorize]
public class CustomersController : ControllerBase
{
    private readonly IRepository<Customers> _customerRepository;
    private readonly FileService _fileService;
    private readonly IMapper _mapper;

    public CustomersController(IRepository<Customers> customerRepository, FileService fileService, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _fileService = fileService;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("")]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAll()
    {
        var customers = await _customerRepository.GetAllAsync();
        if (customers == null)
            return NotFound();

        var result = _mapper.Map<IEnumerable<CustomerDto>>(customers);
        return Ok(result);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<CustomerDto>> GetById([FromQuery] int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null)
            return NotFound();

        var result = _mapper.Map<CustomerDto>(customer);
        return Ok(result);
    }
    [HttpPost]
    [Route("")]
    public async Task<ActionResult<int>> Create([FromForm] CreateCustomerDto createCustomerDto,IFormFile photo)
    {
        if (photo == null || photo.Length == 0)
            return BadRequest("Invalid photo file.");

        try
        {
            var customer = _mapper.Map<Customers>(createCustomerDto);
            var photoPath = await _fileService.SaveFileAsync(photo, "customers");
            customer.PhotoPath = photoPath;

            await _customerRepository.AddAsync(customer);
            return Ok(customer.Id);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPut]
    [Route("")]
    public async Task<IActionResult> Update([FromBody] UpdateCustomerDto updateCustomerDto)
    {
        var existingCustomer = await _customerRepository.GetByIdAsync(updateCustomerDto.Id);
        if (existingCustomer == null)
            return NotFound();

        _mapper.Map(updateCustomerDto, existingCustomer);
        await _customerRepository.UpdateAsync(existingCustomer);
        return Ok();
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromQuery] int id)
    {
        var existingCustomer = await _customerRepository.GetByIdAsync(id);
        if (existingCustomer == null)
            return NotFound();

        if (!string.IsNullOrEmpty(existingCustomer.PhotoPath))
        {
            _fileService.DeleteFile(existingCustomer.PhotoPath);
        }

        await _customerRepository.DeleteAsync(id);
        return Ok();
    }
}