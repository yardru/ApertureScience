using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience
{
    public class AuthentificationOptions
    {
        static public string ISSUER = "MyAuthServer"; // издатель токена
        static public string AUDIENCE = "MyAuthClient"; // потребитель токена
        static public int LIFETIME = 1; // время жизни токена - 1 минута
        static public string KEY
        {
            set
            {
                _KEY_VALUE = value;
            }
        }
        static public SymmetricSecurityKey SECURITY_KEY
        {
            get
            {
                return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_KEY_VALUE));
            }
        }
        static private string _KEY_VALUE = "mysupersecret_secretkey!123";
    }
}
