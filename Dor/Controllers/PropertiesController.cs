using AutoMapper;
using Dor.Dtos.Property;
using Dor.Interfaces;
using Dor.Models;
using Dor.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dor.Controllers;

[ApiController]
[Route("api/properties")]
[Authorize]
public class PropertiesController : ControllerBase
{
    private readonly IRepository<Property> _propertyRepository;
    private readonly FileService _fileService;
    private readonly IMapper _mapper;

    public PropertiesController(IRepository<Property> propertyRepository, FileService fileService, IMapper mapper)
    {
        _propertyRepository = propertyRepository;
        _fileService = fileService;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("")]
    public async Task<ActionResult<IEnumerable<PropertyDto>>> GetAll()
    {
        var records = await _propertyRepository.GetAllAsync();
        if (records == null)
            return NotFound();

        var result = _mapper.Map<IEnumerable<PropertyDto>>(records);
        return Ok(result);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<PropertyDto>> GetById([FromQuery] int id)
    {
        var record = await _propertyRepository.GetByIdAsync(id);
        if (record == null)
            return NotFound();

        var result = _mapper.Map<PropertyDto>(record);
        return Ok(result);
    }

    [HttpPost]
    [Route("")]
    public async Task<ActionResult<int>> Create([FromForm] CreatePropertyDto createPropertyDto, IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Invalid file.");

        try
        {
            var property = _mapper.Map<Property>(createPropertyDto);
            var filePath = await _fileService.SaveFileAsync(file, "properties");
            property.MapPath = filePath;

            await _propertyRepository.AddAsync(property);
            return Ok(property.Id);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPut]
    [Route("")]
    public async Task<IActionResult> Update([FromBody] UpdatePropertyDto updatePropertyDto)
    {
        var existingProperty = await _propertyRepository.GetByIdAsync(updatePropertyDto.Id);
        if (existingProperty == null)
            return NotFound();

        _mapper.Map(updatePropertyDto, existingProperty);
        await _propertyRepository.UpdateAsync(existingProperty);
        return Ok();
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromQuery] int id)
    {
        var existingProperty = await _propertyRepository.GetByIdAsync(id);
        if (existingProperty == null)
            return NotFound();

        if (!string.IsNullOrEmpty(existingProperty.MapPath))
        {
            _fileService.DeleteFile(existingProperty.MapPath);
        }

        await _propertyRepository.DeleteAsync(id);
        return Ok();
    }
}