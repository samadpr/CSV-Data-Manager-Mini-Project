using CsvManagerAPI.API.CsvDataManage.RequestObject;
using CsvManagerAPI.Controllers;
using Domain.Services.CsvManager.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CsvManagerAPI.API.DataManage
{
    [ApiController]
    public class CsvManagerController : BaseApiController<CsvManagerController>
    {
        private readonly ICsvManagerService _csvManagerService;

        public CsvManagerController(ICsvManagerService csvManagerService)
        {
            _csvManagerService = csvManagerService;
        }

        [HttpPost("csv-data-manager/save-csv-data")]
        public async Task<IActionResult> SaveCsvData([FromBody] CsvDataRequestObject request)
        {
            if (request == null || request.CsvUploader == null || request.FileData == null)
                return BadRequest("Invalid data.");

            try
            {
                await _csvManagerService.SaveCsvDataAsync(request.CsvUploader, request.FileData);
                return Ok("Data saved successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

