using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.Dtos.UserDtos
{
    public class GetLoginDto
    {       
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiredAtDate { get; set; }
        public double ExpiredAt { get; set; }
        public DateTime IssuedAtDate { get; set; }
        public double IssuedAt { get; set; }
        public int ExpiresIn { get; set; }
        public string UserId { get; set; } = "";
        public string UserCode { get; set; } = "";
        public string UserName { get; set; } = "";
    }
}