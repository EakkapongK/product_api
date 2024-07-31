using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TestApi.Dtos.UserDtos;
using TestApi.Models.DB;
using TestApi.Models.Response;

namespace TestApi.Data
{
    public class AuthRepository : IAuthRepository
    {
        
        private readonly DataContext _context;
        private readonly ILogger<AuthRepository> _logger;
        private readonly IMapper _mapper;

        public AuthRepository(DataContext context, ILogger<AuthRepository> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _context = context;

        }
        
        public async Task<ServiceResponse<GetLoginDto>> Login(UserLoginDto request)
        {
            var response = new ServiceResponse<GetLoginDto>();
            try
            {                 
                var dbUser = await _context.Users.FirstOrDefaultAsync(c =>
                    c.UserCode.ToLower().Equals(request.UserCode.ToLower()) &&
                    c.Password.Equals(request.Password)
                );

                if (dbUser == null)
                {
                    response.Message = "Wrong credential.";
                    response.Code = "101";
                    response.Success = false;
                    return response;
                }
                else
                {
                    if(dbUser.PasswordHash != null && dbUser.PasswordSalt != null){
                        if (!VerifyPasswordHash(request.Password, dbUser.PasswordHash, dbUser.PasswordSalt))
                        {
                            response.Message = "Wrong credential.";
                            response.Code = "101";
                            response.Success = false;
                            return response;
                        }
                    }

                    int expiresInDay = 1;                    
                    var currUtcDate = DateTime.UtcNow;
                    var expireDate = currUtcDate;

                    expireDate = expireDate.AddDays(expiresInDay);

                    var result = new GetLoginDto
                    {
                        Token = CreateToken(dbUser, expireDate),
                        ExpiredAt = expireDate
                        .Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                        .TotalMilliseconds,
                                    ExpiredAtDate = Helper.UtcToThaiDate(expireDate),
                                    IssuedAt = currUtcDate.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                        .TotalMilliseconds,
                        IssuedAtDate = Helper.UtcToThaiDate(currUtcDate),
                        ExpiresIn = expiresInDay * 24 * 60 * 60, //seconds
                        UserId = dbUser.Id.ToString(),
                        UserCode = dbUser.UserCode.Trim(),
                        UserName = dbUser.Name.Trim()
                    };

                    response.Success = true;
                    response.Data = result;
                    response.Code = "000";
                    response.Message = "Success";
                }    

            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    response.Message = e.InnerException.ToString().Substring(0, 200);
                }
                else
                {
                    response.Message = e.Message;
                }
                response.Code = "888";
                response.Success = false;
                _logger.LogError("Exception caught. {0}", e);
            }

            return response;
        }

        public async Task<ServiceResponse<GetUserDto>> Register(string UserCode, string Password, string Name, string Email, string Remark)
        {
            var response = new ServiceResponse<GetUserDto>();
            try
            {
                CreatePasswordHash(Password, out byte[] passwordHash, out byte[] passwordSalt);
                Console.WriteLine("passwordHash = {0} ", passwordHash);
                Console.WriteLine("passwordSalt = {0} ", passwordSalt);
                var user = new User
                {
                    UserCode = UserCode,
                    Password = Password,
                    Name = Name,
                    Email = Email,
                    Remark = Remark,
                    // PasswordHash = passwordHash,
                    // PasswordSalt = passwordSalt,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                
                response.Data = _mapper.Map<GetUserDto>(user);;
                response.Code = "000";
                response.Success = true;
                response.Message = "Success";
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    response.Message = e.InnerException.ToString().Substring(0, 200);
                }
                else
                {
                    response.Message = e.Message;
                }
                response.Code = "888";
                response.Success = false;
                _logger.LogError("Exception caught. {0}", e);
            }

            return response;
        }

        
        private string CreateToken(User user, DateTime expireDate)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("UserCode", user.UserCode)
            };

            // var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Env.AppToken()));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            
            var currUtcDate = DateTime.UtcNow;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                // Expires = Helper.DateNow().AddDays(1),
                Expires = expireDate,
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // _logger.LogInformation("{0}",Helper.UtcToThaiDate(token.ValidTo));

            return tokenHandler.WriteToken(token);
        }


        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}