using BACK.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace BACK.Repository.Interfaces
{

    /// <summary>
    /// Controle para autenticação.
    /// </summary>
    [Route("login")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly ILogin _Login;

        public LoginController(ILogin Login)
        {
            _Login = Login;
        }

        /// <summary>
        /// Informa dados de autenticação para geração do token JWT.
        /// </summary>
        /// <param name="acesso">login e senha do usuário com acesso a API.</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Post(Acesso acesso)
        {
            try
            {
                if(acesso == null)
                    return BadRequest();
                var token = _Login.Get(acesso.login, acesso.senha);

                if (token == String.Empty)
                    return Unauthorized();

                return Ok(token);
            }
            catch (Exception ex)
            {
                Log.Error($"O Processo falhou na etapa: {MethodBase.GetCurrentMethod().DeclaringType.FullName} retornando o erro: {ex.Message}");
                return BadRequest();
            }
        }
    }
}
