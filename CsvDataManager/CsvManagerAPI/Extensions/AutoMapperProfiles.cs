using AutoMapper;
using CsvManagerAPI.API.User.RequestObject;
using Domain.Models;
using Domain.Services.Login.DTOs;
using Domain.Services.SignUp.DTOs;

namespace CsvManagerAPI.Extensions
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<SignUpRequestObject, SignUpRequestDto>().ReverseMap();
            CreateMap<SignUpRequestDto, User>().ReverseMap();

            CreateMap<LoginRequestObject, LoginRequestDto>().ReverseMap();
        }
    }
}
