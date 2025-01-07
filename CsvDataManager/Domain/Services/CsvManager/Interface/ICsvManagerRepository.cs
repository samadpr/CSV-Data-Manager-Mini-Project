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
        Task AddCsvUploaderAsync(CsvUploader csvUploader);
        Task AddCsvUploaderAsync(CsvDataDto request);
        Task AddFileDataAsync(FileData fileData);
        Task SaveChangesAsync();
    }
}
