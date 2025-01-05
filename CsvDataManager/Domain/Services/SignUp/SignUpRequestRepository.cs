using Domain.Models;
using Domain.Services.SignUp.DTOs;
using Domain.Services.SignUp.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.SignUp
{
    public class SignUpRequestRepository : ISignUpRequestRepository
    {
        private readonly CsvDataManagerDbContext _context;
        public SignUpRequestRepository(CsvDataManagerDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddUserAsync(User user)
        {
            if (await IsEmailExistAsync(user.Email))
                return false;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task<bool> IsEmailExistAsync(string email)
        {
            var isExist = _context.Users.Any(user => user.Email == email);
            return Task.FromResult(isExist);
        }
    }
}
