using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalR_Authentication.Models;

namespace SignalR_Authentication.Interfaces
{
    public interface ITokenService
    {
        public string GenerateToken(LoginModel model);

        public bool ValidateToken(string token);
    }
}
