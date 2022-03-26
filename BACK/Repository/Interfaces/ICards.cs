using BACK.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BACK.Repository.Interfaces
{
    /// <summary>
    /// Interface dos Cards.
    /// </summary>
    public interface ICards
    {
        /// <summary>
        /// Retorno de todos os cards.
        /// </summary>
        /// <returns>Lista.</returns>
        List<Card> Get();

        /// <summary>
        /// Para inserir um card o título, o conteúdo e o nome da lista devem estar preenchidos, o id não deve conter valor. Ao inserir retorne o card completo incluindo o id atribuído com o statusCode apropriado. Caso inválido, retorne status 400.
        /// </summary>
        /// <returns>Card.</returns>
        Card Post(Card card);

        /// <summary>
        /// Para alterar um card, o entrypoint deve receber um id pela URL e um card pelo corpo da requisição. Valem as mesmas regras de validação do item acima exceto que o id do card deve ser o mesmo id passado pela URL. Na alteração todos os campos são alterados. Caso inválido, retorne status 400. Caso o id não exista retorne 404. Se tudo correu bem, retorne o card alterado.
        /// </summary>
        /// <returns>Card.</returns>
        Card Put(Card card);

        /// <summary>
        /// Para remover um card, o entrypoint deve receber um id pela URL. Caso o id não exista retorne 404. Se a remoção for bem sucedida retorne a lista de cards.
        /// </summary>
        /// <returns>Card e Lista.</returns>
        (Card, List<Card>) Delete(string id);
    }

}
