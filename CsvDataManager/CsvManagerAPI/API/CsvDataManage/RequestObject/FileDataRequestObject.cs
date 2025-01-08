namespace CsvManagerAPI.API.CsvDataManage.RequestObject
{
    public class FileDataRequestObject
    {
        public Guid FileId { get; set; }
        public string Data { get; set; } = null!;
    }
}
