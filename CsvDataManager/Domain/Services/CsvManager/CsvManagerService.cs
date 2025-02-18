﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Models;
using Domain.Services.CsvManager.DTOs;
using Domain.Services.CsvManager.Interface;

namespace Domain.Services.CsvManager
{
    public class CsvManagerService : ICsvManagerService
    {
        private readonly ICsvManagerRepository _repository;
        private readonly IMapper _mapper;

        public CsvManagerService(ICsvManagerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task SaveCsvUploaderAsync(CsvUploaderDto csvUploaderDto)
        {
            if (csvUploaderDto == null) throw new ArgumentNullException(nameof(csvUploaderDto));

            var csvUploader = _mapper.Map<CsvUploader>(csvUploaderDto);

            await _repository.SaveCsvUploaderAsync(csvUploader);
        }

        public async Task SaveBatchFileDataAsync(FileDataDto fileDataDtos)
        {
            if (fileDataDtos == null)
                throw new ArgumentException("File data batch cannot be null or empty.", nameof(fileDataDtos));

            var fileDataBatch = _mapper.Map<FileData>(fileDataDtos);
            await _repository.SaveBatchFileDataAsync(fileDataBatch);
        }

        public async Task<List<FileDataDto>> GetFileDataByUserIdAsync(Guid userId)
        {
            var fileDataList = await _repository.GetFileDataByUserIdAsync(userId);
            return _mapper.Map<List<FileDataDto>>(fileDataList);
        }

        public async Task<List<CsvUploaderDto>> GetCsvFileByUserIdAsync(Guid userId)
        {
            var csvFiles = await _repository.GetCsvFileByUserIdAsync(userId);

            return _mapper.Map<List<CsvUploaderDto>>(csvFiles);
        }

        public async Task<List<FileDataDto>> GetFileDataByFileIdAsync(Guid fileId)
        {
            var fileData = await _repository.GetFileDataByFileIdAsync(fileId);
            return fileData.Select(fd => _mapper.Map<FileDataDto>(fd)).ToList();
        }
    }
}
