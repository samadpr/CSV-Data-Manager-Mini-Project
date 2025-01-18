using CsvDataManager.Dtos;

namespace CsvDataManager.Service.Interface
{
    public interface ICsvDataRetrieveApiService
    {
        Task<List<Dictionary<string, string>>> GetFileDataByUserIdAsync(Guid userId);
        Task<List<CsvFileModelDto>> GetUploadedCsvFilesByUserIdAsync(Guid userId);
        Task<List<Dictionary<string, string>>> GetFileDataByFileIdAsync(Guid fileId);

    }
}
