using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.Dtos.UserDtos
{
    public class CreateUserDto
    {
        public string UserCode { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Email { get; set; }

        public string? Remark { get; set; }
        public DateTime? CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public byte[]? PasswordHash { get; set; }

        public byte[]? PasswordSalt { get; set; }
    }
}