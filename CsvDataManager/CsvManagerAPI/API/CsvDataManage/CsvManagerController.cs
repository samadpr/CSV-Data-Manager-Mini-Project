using AutoMapper;
using CsvManagerAPI.API.CsvDataManage.RequestObject;
using CsvManagerAPI.Controllers;
using Domain.Services.CsvManager.DTOs;
using Domain.Services.CsvManager.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CsvManagerAPI.API.DataManage
{
    [ApiController]
    [Authorize]
    public class CsvManagerController : BaseApiController<CsvManagerController>
    {
        private readonly ICsvManagerService _csvManagerService;
        private readonly IMapper _mapper;

        public CsvManagerController(ICsvManagerService csvManagerService, IMapper mapper)
        {
            _csvManagerService = csvManagerService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("csv-data-manager/save-csv-data")]
        public async Task<IActionResult> SaveCsvData([FromForm] CsvDataRequestObject request)
        {
            if (request == null && !ModelState.IsValid == false)
                return BadRequest("Invalid data.");

            try
            {
                var csvData = _mapper.Map<CsvDataDto>(request);
                await _csvManagerService.SaveCsvDataAsync(csvData);
                return Ok("Data saved successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

