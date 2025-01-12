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
        [Route("csv-data-manager/save-csv-uploader")]
        public async Task<IActionResult> SaveCsvUploader([FromBody] CsvUploaderRequestObject request)
        {
            if (request == null || !ModelState.IsValid)
                return BadRequest("Invalid data.");

            try
            {
                var csvUploader = _mapper.Map<CsvUploaderDto>(request);
                await _csvManagerService.SaveCsvUploaderAsync(csvUploader);
                return Ok("CsvUploader data saved successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("csv-data-manager/save-file-data")]
        public async Task<IActionResult> SaveFileDataBatch([FromBody] FileDataRequestObject requestBatch)
        {
            if (requestBatch == null)
                return BadRequest("Invalid or empty batch data.");

            try
            {
                var fileDataDtos = _mapper.Map<FileDataDto>(requestBatch);

                await _csvManagerService.SaveBatchFileDataAsync(fileDataDtos);

                return Ok("File data batch saved successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("csv-data-manager/get-file-data/{userId}")]
        public async Task<IActionResult> GetFileDataByUserId(Guid userId)
        {
            try
            {
                var fileDataList = await _csvManagerService.GetFileDataByUserIdAsync(userId);

                if (fileDataList == null || fileDataList.Count == 0)
                    return NotFound("No data found for the given UserId.");

                return Ok(fileDataList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("csv-data-manager/get-csv-file/{userId}")]
        public async Task<IActionResult> GetCsvFileByUserId(Guid userId)
        {
            var csvFiles = await _csvManagerService.GetCsvFileByUserIdAsync(userId);
            if (csvFiles == null || csvFiles.Count == 0)
                return NotFound("No CSV files found for the user.");

            return Ok(csvFiles);
        }

    }
}

