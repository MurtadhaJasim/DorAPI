using AutoMapper;
using Dor.Dtos.Complex;
using Dor.Interfaces;
using Dor.Models;
using Dor.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dor.Controllers;

[ApiController]
[Route("api/complexs")]
[Authorize]
public class ComplexsController : ControllerBase
{
    private readonly IRepository<Complex> _complexRepository;
    private readonly FileService _fileService;
    private readonly IMapper _mapper;

    public ComplexsController(IRepository<Complex> complexRepository, FileService fileService, IMapper mapper)
    {
        _complexRepository = complexRepository;
        _fileService = fileService;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("")]
    public async Task<ActionResult<IEnumerable<ComplexDto>>> GetComplex()
    {
        var records = await _complexRepository.GetAllAsync();
        if (records == null)
            return NotFound();

        var result = _mapper.Map<IEnumerable<ComplexDto>>(records);
        return Ok(result);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<ComplexDto>> GetComplexById([FromQuery] int id)
    {
        var record = await _complexRepository.GetByIdAsync(id);
        if (record == null)
            return NotFound();

        var result = _mapper.Map<ComplexDto>(record);
        return Ok(result);
    }

    [HttpPost]
    [Route("")]
    public async Task<ActionResult<int>> AddComplexWithFile([FromForm] CreateComplexDto createComplexDto,IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Invalid file.");

        try
        {
            var complex = _mapper.Map<Complex>(createComplexDto);
            var filePath = await _fileService.SaveFileAsync(file, "complexes");
            complex.LogoPath = filePath;

            await _complexRepository.AddAsync(complex);
            return Ok(complex.Id);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPut]
    [Route("")]
    public async Task<IActionResult> UpdateComplex([FromBody] UpdateComplexDto updateComplexDto)
    {
        var existingComplex = await _complexRepository.GetByIdAsync(updateComplexDto.Id);
        if (existingComplex == null)
            return NotFound();

        _mapper.Map(updateComplexDto, existingComplex);
        await _complexRepository.UpdateAsync(existingComplex);
        return Ok();
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteComplex([FromQuery] int id)
    {
        var existingComplex = await _complexRepository.GetByIdAsync(id);
        if (existingComplex == null)
            return NotFound();

        if (!string.IsNullOrEmpty(existingComplex.LogoPath))
        {
            _fileService.DeleteFile(existingComplex.LogoPath);
        }

        await _complexRepository.DeleteAsync(id);
        return Ok();
    }
}