using collabBackend.Models;
using collabDB;
using collabDB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

namespace collabBackend.Services
{
    public class LoginService : ILoginService
    {
        CollabContext context;
        private readonly IConfiguration _config;

        public LoginService(CollabContext dbcontext, IConfiguration config)
        {
            context = dbcontext;
            _config = config;
        }

        public IEnumerable<User> Get()
        {
            return context.Users;
        }

        public LoginResultModel ValidateCredentials(string email, string password)
        {
            string ePass = GetSHA256(password);
            if (EsValido(email, ePass))
            {
                var usuarioModel = ObtenerUsuario(email);
                var tokenJwt = Generate(usuarioModel);
                return new LoginResultModel
                {
                    Success = true,
                    Token = tokenJwt,
                    User = usuarioModel
                };
            }
            else
            {
                return new LoginResultModel
                {
                    Success = false,
                    Token = null,
                    Error = "Credenciales inválidas",
                    User = null
                };
            }


        }

        private bool EsValido(string email, string password)
        {
            var user = context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                return false;
            }

            if (password != user.Password)
            {
                return false;
            }

            return true;
        }

        private UserModel ObtenerUsuario(string email)
        {
            var user = context.Users.FirstOrDefault(u => u.Email == email);
            return new UserModel
            {
                Name = user.Name,
                Email = user.Email,
                UserType = user.UserTypeId
            };



        }

        //generate JWT
        private string Generate(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Crear los claims
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.Name),
                new Claim(ClaimTypes.Role, user.UserType.ToString()),
            };


            // Crear el token

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //encript password 8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92
        private static string GetSHA256(string str)
        {
            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }

    }



    public interface ILoginService
    {
        IEnumerable<User> Get();
        LoginResultModel ValidateCredentials(string email, string password);
    }
}
