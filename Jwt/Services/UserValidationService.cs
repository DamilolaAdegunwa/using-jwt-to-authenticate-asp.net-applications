using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using JwtToken.HelperService;
using JwtToken.Model;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JwtToken.Services
{
    public class UserValidationService : IUserValidationService
    {
        List<UserEntity> users;
        KeySettings key;

        public UserValidationService(IOptions<KeySettings> appSettings)
        {
            this.key = appSettings.Value;
            this.users = new List<UserEntity>
            {
                new UserEntity { Id = 1, FirstName = "Sachin", LastName = "Tendulkar", Email = "sachin@gmail.com",Username = "sachin", Password = "123" }
            };
        }

        public UserEntity IsValidate(string username, string password)
        {
            var user = users.SingleOrDefault(x => x.Username == username && x.Password == password);
            if (user == null)
            {
                return null;
            }
            var key = Encoding.ASCII.GetBytes(this.key.SecretKey);
            var jwtToken = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(jwtToken);
            user.Token = tokenHandler.WriteToken(token);
            return user;
        }
    }
}
