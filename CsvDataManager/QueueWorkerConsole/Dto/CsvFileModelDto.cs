using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueWorkerConsole.Dto
{
    public class CsvFileModelDto
    {
        public CsvUploaderDto CsvUploader { get; set; } = new CsvUploaderDto();
        public List<FileDataDto> FileData { get; set; } = new List<FileDataDto>();
    }
}
