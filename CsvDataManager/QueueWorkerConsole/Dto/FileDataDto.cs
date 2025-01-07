using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueWorkerConsole.Dto
{
    public class FileDataDto
    {
        public Guid FileId { get; set; }

        public string Data { get; set; } = null!;
    }
}
