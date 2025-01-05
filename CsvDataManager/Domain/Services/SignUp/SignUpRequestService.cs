using AutoMapper;
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
    public class SignUpRequestService : ISignUpRequestService
    {
        private readonly ISignUpRequestRepository _signUpRequestRepository;
        private readonly IMapper _mapper;

        public SignUpRequestService(ISignUpRequestRepository signUpRequestRepository, IMapper mapper)
        {
            _signUpRequestRepository = signUpRequestRepository;
            _mapper = mapper;
        }

        public async Task<string> RegisterUserAsync(SignUpRequestDto requestDto)
        {
            try
            {
                if (await _signUpRequestRepository.IsEmailExistAsync(requestDto.Email))
                    return "Email already exists";

                var mappedUser = _mapper.Map<SignUpRequestDto, User>(requestDto);

                var success = await _signUpRequestRepository.AddUserAsync(mappedUser);
                return success ? "User registered successfully" : "Failed to register user";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in RegisterUserAsync: {ex.Message}");
                return "An error occurred during registration";
            }
        }
    }
}
