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

    }
}

