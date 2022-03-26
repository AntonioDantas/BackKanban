using BACK.Models;
using Microsoft.AspNetCore.Mvc;

namespace BACK.Repository.Interfaces
{
    public interface ILogin
    {
        /// <summary>
        /// O sistema deve ter um mecanismo de login usando JWT, com um entrypoint que recebe { "login":"letscode", "senha":"lets@123"} e gera um token.
        /// </summary>
        public string Get(string login, string senha);
    }
}
