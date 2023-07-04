using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR_Authentication.Models
{
    public class LoginModel
    {
        public string Name { get; set; }

        public string Role { get; set; }

        public string Email { get; set; }
    }
}
