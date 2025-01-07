using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Login.DTOs
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = null!;
        public Guid userId { get; set; }
    }
}
