using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.CsvManager.DTOs
{
    public class FileDataDto
    {
        public Guid Id { get; set; }
        public Guid FileId { get; set; }
        public string Data { get; set; } = null!;
    }
}
