using Domain.Models;
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
        Task AddFileDataAsync(FileData fileData);
        Task SaveChangesAsync();
    }
}
