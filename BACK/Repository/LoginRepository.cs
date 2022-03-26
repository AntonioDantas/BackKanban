using BACK.Data;
using BACK.Models;
using BACK.Repository.Interfaces;
using IdentityModel;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace BACK.Repository
{
    public class LoginRepository : ILogin
    {
        private readonly ApplicationDbContext _db;
        private readonly AppSettings _appSettings;
        private readonly AppAccess _appAccess;

        public LoginRepository(ApplicationDbContext db,
            IOptions<AppSettings> appSettings,
            IOptions<AppAccess> appAccess)
        {
            _db = db;
            _appSettings = appSettings.Value;
            _appAccess = appAccess.Value;
        }

        public string Get(string login, string senha)
        {
            try
            {
                if (login.Equals(_appAccess.login) && senha.Equals(_appAccess.senha))
                {
                    var identityClaims = new ClaimsIdentity();
                    identityClaims.AddClaim(new Claim(JwtClaimTypes.Name, login));

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = identityClaims,
                        Issuer = _appSettings.Issuer,
                        Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiresTimeInHour),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };

                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    return tokenHandler.WriteToken(token);
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"O Processo falhou na etapa: {MethodBase.GetCurrentMethod().DeclaringType.FullName} retornando o erro: {ex.Message}");
                return string.Empty;
            }
        }
    }
}
