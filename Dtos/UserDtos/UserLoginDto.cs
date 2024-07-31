using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.Dtos.UserDtos
{
    public class UserLoginDto
    {        
        public string UserCode { get; set; } = "";
        public string Password { get; set; } = "";
    }
}