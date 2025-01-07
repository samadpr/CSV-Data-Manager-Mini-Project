using AutoMapper;
using CsvManagerAPI.API.User.RequestObject;
using CsvManagerAPI.Controllers;
using Domain.Services.Login.DTOs;
using Domain.Services.Login.Interface;
using Domain.Services.SignUp.DTOs;
using Domain.Services.SignUp.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CsvManagerAPI.API.User
{
    [ApiController]
   
    public class UserController : BaseApiController<UserController>
    {
        private readonly IMapper _mapper;
        private readonly ISignUpRequestService _signUpRequestService;
        private readonly ILoginRequestService _loginRequestService;

        public UserController(IMapper mapper, ISignUpRequestService signUpRequestService, ILoginRequestService loginRequestService)
        {
            _mapper = mapper;
            _signUpRequestService = signUpRequestService;
            _loginRequestService = loginRequestService;
        }

        [HttpPost]
        [Route("user/register")]
        public async Task<IActionResult> Register([FromBody] SignUpRequestObject signUpRequest)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(signUpRequest.Email) || string.IsNullOrWhiteSpace(signUpRequest.Password))
                    return BadRequest("Email and Password are required");

                var mappedSignUpRequest = _mapper.Map<SignUpRequestDto>(signUpRequest);
                var result = await _signUpRequestService.RegisterUserAsync(mappedSignUpRequest);

                if (result == "Email already exists")
                    return Conflict(result);

                if (result == "User registered successfully")
                    return Ok(result);

                return StatusCode(500, result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Register API: {ex.Message}");
                return StatusCode(500, "An unexpected error occurred while processing your request");
            }
        }

        [HttpPost]
        [Route("user/login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestObject loginRequest)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(loginRequest.Email) || string.IsNullOrWhiteSpace(loginRequest.Password))
                    return BadRequest("Email and Password are required");

                var mappedLoginRequest = _mapper.Map<LoginRequestDto>(loginRequest);
                var response = await _loginRequestService.AuthenticateUserAsync(mappedLoginRequest);

                if (response == null)
                    return Unauthorized("Invalid email or password");

                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Login API: {ex.Message}");
                return StatusCode(500, "An unexpected error occurred while processing your request");
            }
        }
    }
}
