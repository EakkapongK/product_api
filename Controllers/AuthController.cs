using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestApi.Data;
using TestApi.Dtos.UserDtos;
using TestApi.Models.Response;

namespace TestApi.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {        
        private readonly IAuthRepository _authRepo;

        public AuthController(IAuthRepository authRepo)
        {
            _authRepo = authRepo;
        }
        
        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(string UserCode, string Password, string Name, string Email="", string Remark="")
        {
            var response = await _authRepo.Register(UserCode, Password, Name, Email, Remark);
            if(!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(UserLoginDto request)
        {
            var response = await _authRepo.Login(request);
            if(response.Code == "101")
            {
                return Unauthorized(response);
            }
            if(!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        
    }
}