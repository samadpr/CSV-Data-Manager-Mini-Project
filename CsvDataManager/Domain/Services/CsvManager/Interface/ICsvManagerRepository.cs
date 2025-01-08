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
        Task SaveFileDataAsync(FileData fileData);
        Task SaveBatchFileDataAsync(FileData fileDataBatch);
    }
}
