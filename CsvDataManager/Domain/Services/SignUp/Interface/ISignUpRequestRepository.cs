using Domain.Models;
using Domain.Services.SignUp.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.SignUp.Interface
{
    public interface ISignUpRequestRepository
    {
        Task<bool> AddUserAsync(User user);
        Task<bool> IsEmailExistAsync(string email);
    }
}
