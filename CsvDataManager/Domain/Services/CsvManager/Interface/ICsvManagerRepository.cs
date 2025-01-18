using Domain.Models;
using Domain.Services.CsvManager.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.CsvManager.Interface
{
    public interface ICsvManagerRepository
    {
        Task SaveCsvUploaderAsync(CsvUploader csvUploader);
        //Task SaveFileDataAsync(FileData fileData);
        Task SaveBatchFileDataAsync(FileData fileDataBatch);
        Task<List<FileData>> GetFileDataByUserIdAsync(Guid userId);
        Task<List<CsvUploader>> GetCsvFileByUserIdAsync(Guid userId);
        Task<List<FileData>> GetFileDataByFileIdAsync(Guid fileId);
    }
}
