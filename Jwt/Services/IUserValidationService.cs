using JwtToken.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtToken.Services
{
    public interface IUserValidationService
    {
        UserEntity IsValidate(string username, string password);
    }
}
