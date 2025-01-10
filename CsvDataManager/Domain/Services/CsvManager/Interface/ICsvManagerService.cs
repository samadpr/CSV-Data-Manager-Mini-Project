
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Services.CsvManager.DTOs;

namespace Domain.Services.CsvManager.Interface
{
    public interface ICsvManagerService
    {
        Task SaveCsvUploaderAsync(CsvUploaderDto csvUploaderDto);
        Task SaveFileDataAsync(FileDataDto fileDataDto);
        Task SaveBatchFileDataAsync(FileDataDto fileDataDtos);
        Task<List<FileDataDto>> GetFileDataByUserIdAsync(Guid userId);

    }
}
