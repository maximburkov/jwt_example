using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWT_Example.Models
{
    public class UserDTO
    {
        public string Login { get; set; }

        public string Role { get; set; }

        public string Token { get; set; }
    }
}
