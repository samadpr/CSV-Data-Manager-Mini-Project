using Domain.Models;

namespace CsvManagerAPI.API.CsvDataManage.RequestObject
{
    public class CsvDataRequestObject
    {
        public Guid FileId { get; set; }

        public string Data { get; set; } = null!;

        public Guid Id { get; set; }

        public string FileName { get; set; } = null!;

        public string Extension { get; set; } = null!;

        public string FilePath { get; set; } = null!;

        public long FileSize { get; set; }

        public int NoOfRow { get; set; }

        public string Status { get; set; } = null!;

        public Guid UserId { get; set; }

    }
}
