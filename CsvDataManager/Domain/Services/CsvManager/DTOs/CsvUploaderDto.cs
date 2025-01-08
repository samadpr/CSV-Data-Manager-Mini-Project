using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.CsvManager.DTOs
{
    public class CsvUploaderDto
    {
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
