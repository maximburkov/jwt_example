using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace JWT_Example
{
    public class AuthOptions
    {
        public const string ISSUER = "AuthServer"; 
        public const string AUDIENCE = "AuthClient"; 
        const string KEY = "my_secretkey!123";  
        public const int LIFETIME = 1; // token lifetime 1 minute
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
