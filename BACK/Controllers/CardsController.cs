using BACK.Models;
using BACK.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;

namespace BACK.Controllers
{
    /// <summary>
    /// Controle dos cards do Kanban.
    /// </summary>
    [Route("cards")]
    public class CardsController : Controller
    {
        private readonly ICards _Cards;
        /// <summary>
        /// Construtor.
        /// </summary>
        public CardsController(ICards Cards)
        {
            _Cards = Cards;
        }

        /// <summary>
        /// Retorna todos os cards.
        /// </summary>
        /// <returns>Lista de Cards.</returns>
        [HttpGet]
        [Route("")]
        [Authorize]
        public IActionResult Get()
        {
            try
            {
                return Ok(_Cards.Get());
            }
            catch (Exception ex)
            {
                Log.Error($"O Processo falhou na etapa: {MethodBase.GetCurrentMethod().DeclaringType.FullName} retornando o erro: {ex.Message}");
                return BadRequest();
            }
        }

        /// <summary>
        /// Registra um novo card.
        /// </summary>
        /// <param name="card">Objeto para registro com id em branco.</param>
        /// <returns>Objeto do card com id registrado.</returns>
        [HttpPost]
        [Authorize]
        [Route("")]
        public IActionResult Post([FromBody]Card card)
        {
            try
            {
                if (!ValidRequest(card, true))
                    return BadRequest();
                return Ok(_Cards.Post(card));
            }
            catch (Exception ex)
            {
                Log.Error($"O Processo falhou na etapa: {MethodBase.GetCurrentMethod().DeclaringType.FullName} retornando o erro: {ex.Message}");
                return BadRequest();
            }
        }

        /// <summary>
        /// Altera os registros de um card.
        /// </summary>
        /// <param name="id">Mesmo id do objeto do card.</param>
        /// <param name="card">Objetos card a ser atualizado.</param>
        /// <returns>Objeto atualizado.</returns>
        [HttpPut]
        [Authorize]
        [Route("{id}")]
        public IActionResult Put(string id, [FromBody] Card card)
        {
            try
            {
                if (!ValidRequest(card, false) || !card.id.Equals(id))
                    return BadRequest();

                var cards = _Cards.Put(card);
                if (cards != null)
                {
                    Log.Warning($"{DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")} - Card {card.id} - {card.titulo} - Alterar");
                    return Ok(cards);
                }
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                Log.Error($"O Processo falhou na etapa: {MethodBase.GetCurrentMethod().DeclaringType.FullName} retornando o erro: {ex.Message}");
                return BadRequest();
            }
        }

        /// <summary>
        /// Excluí o card.
        /// </summary>
        /// <param name="id">Id do card a ser removido.</param>
        /// <returns>Lista dos Cards.</returns>
        [HttpDelete]
        [Authorize]
        [Route("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return BadRequest();

                (var card, var cards) = _Cards.Delete(id);
                if (cards != null)
                {
                    Log.Warning($"{DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")} - Card {card.id} - {card.titulo} - Remover");
                    return Ok(cards);
                }
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                Log.Error($"O Processo falhou na etapa: {MethodBase.GetCurrentMethod().DeclaringType.FullName} retornando o erro: {ex.Message}");
                return BadRequest();
            }
        }

        /// <summary>
        /// Método interno de apoio para verificação da informações.
        /// </summary>
        /// <param name="card">Objeto card com informações a serem verificadas.</param>
        /// <param name="Id">Verificação do Id (true == "") (false != ""). </param>
        /// <returns>Verdadeiro caso as informações estejam válidas.</returns>
        internal bool ValidRequest(Card card, bool Id)
        {
            if (card == null || card.conteudo.Equals("") || card.lista.Equals("") || card.titulo.Equals(""))
                return false;

            if (card.id == null) card.id = "";
            if ((!Id && card.id.Equals("")) || (Id && !card.id.Equals("")))
                return false;
            return true;
        }

    }
}
