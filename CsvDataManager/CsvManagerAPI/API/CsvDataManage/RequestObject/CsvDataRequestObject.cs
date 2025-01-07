using Domain.Models;

namespace CsvManagerAPI.API.CsvDataManage.RequestObject
{
    public class CsvDataRequestObject
    {
        public CsvUploader CsvUploader { get; set; }
        public List<FileData> FileData { get; set; }

    }
}
