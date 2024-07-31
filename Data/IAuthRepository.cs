using TestApi.Dtos.UserDtos;
using TestApi.Models.Response;

namespace TestApi.Data
{
    public interface IAuthRepository
    {        
        Task<ServiceResponse<GetLoginDto>> Login(UserLoginDto request);
        // Task<ServiceResponse<GetTokenDto>> Token(TokenLoginDto request);
        Task<ServiceResponse<GetUserDto>> Register(string UserCode, string Password, string Name, string Email, string Remark);
    }
}