using AutoMapper;
using Dor.Dtos.Building;
using Dor.Interfaces;
using Dor.Models;
using Dor.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dor.Controllers;

[ApiController]
[Route("api/buildings")]
[Authorize]
public class BuildingsController : ControllerBase
{
    private readonly IRepository<Building> _buildingRepository;
    private readonly ILogger<BuildingsController> _logger;
    private readonly FileService _fileService;
    private readonly IMapper _mapper;

    public BuildingsController(IRepository<Building> buildingRepository, ILogger<BuildingsController> logger, FileService fileService, IMapper mapper)
    {
        _buildingRepository = buildingRepository;
        _logger = logger;
        _fileService = fileService;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("")]
    public async Task<ActionResult<IEnumerable<BuildingDto>>> GetAll()
    {
        var records = await _buildingRepository.GetAllAsync();
        if (records == null)
            return NotFound();

        var result = _mapper.Map<IEnumerable<BuildingDto>>(records);
        return Ok(result);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<BuildingDto>> GetById([FromQuery] int id)
    {
        _logger.LogDebug("Getting building #{id}", id);
        var record = await _buildingRepository.GetByIdAsync(id);
        if (record == null)
        {
            _logger.LogWarning("Building #{id} was not found", id);
            return NotFound();
        }

        var result = _mapper.Map<BuildingDto>(record);
        return Ok(result);
    }

    [HttpPost]
    [Route("")]
    public async Task<ActionResult<int>> Create([FromForm] CreateBuildingDto createBuildingDto, IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Invalid file.");

        try
        {
            var building = _mapper.Map<Building>(createBuildingDto);
            var filePath = await _fileService.SaveFileAsync(file, "buildings");
            building.MapPath = filePath;

            await _buildingRepository.AddAsync(building);
            return Ok(building.Id);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPut]
    [Route("")]
    public async Task<IActionResult> Update([FromBody] UpdateBuildingDto updateBuildingDto)
    {
        var existingBuilding = await _buildingRepository.GetByIdAsync(updateBuildingDto.Id);
        if (existingBuilding == null)
            return NotFound();

        _mapper.Map(updateBuildingDto, existingBuilding);
        await _buildingRepository.UpdateAsync(existingBuilding);
        return Ok();
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromQuery] int id)
    {
        var existingBuilding = await _buildingRepository.GetByIdAsync(id);
        if (existingBuilding == null)
            return NotFound();

        if (!string.IsNullOrEmpty(existingBuilding.MapPath))
        {
            _fileService.DeleteFile(existingBuilding.MapPath);
        }

        await _buildingRepository.DeleteAsync(id);
        return Ok();
    }
}