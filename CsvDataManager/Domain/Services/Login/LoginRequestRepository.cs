using Domain.Models;
using Domain.Services.Login.DTOs;
using Domain.Services.Login.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Login
{
    public class LoginRequestRepository : ILoginRequestRepository
    {
        private readonly CsvDataManagerDbContext _context;
        public LoginRequestRepository(CsvDataManagerDbContext context)
        {
            _context = context;
        }
        public async Task<User?> GetUserByEmailAndPasswordAsync(LoginRequestDto loginRequestDto)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == loginRequestDto.Email && u.Password == loginRequestDto.Password);
        }

    }
}
