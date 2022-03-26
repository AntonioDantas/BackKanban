using BACK.Data;
using BACK.Models;
using BACK.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BACK.Repository
{
    /// <summary>
    /// Funcionalidades do Card.
    /// </summary>
    public class CardRepository : ICards
    {
        private readonly ApplicationDbContext _db;

        /// <summary>
        /// Construtor.
        /// </summary>
        public CardRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Retorna todos os cards.
        /// </summary>
        /// <returns>lista.</returns>
        public List<Card> Get()
        {
            var list = _db.Cards.ToList();
            return list;
        }

        /// <summary>
        /// Cria um card.
        /// </summary>
        /// <param name="card">Dados.</param>
        /// <returns>card.</returns>
        public Card Post(Card card)
        {
            card.id = Guid.NewGuid().ToString();
            _db.Cards.Add(card);
            _db.SaveChanges();
            return card;
        }

        /// <summary>
        /// Altera um card.
        /// </summary>
        /// <param name="card">Novos dados.</param>
        public Card Put(Card card)
        {
            var oldCard = _db.Cards.Where(x => x.id == card.id);
            if (oldCard.Any())
            {
                _db.Entry(oldCard.FirstOrDefault()).CurrentValues.SetValues(card);
                _db.SaveChanges();
                return card;
            }
            else
                return null;
        }

        /// <summary>
        /// Excluí o card.
        /// </summary>
        /// <param name="id">Id presente na lista.</param>
        public (Card, List<Card>) Delete(string id)
        {
            var cards = _db.Cards.Where(x => x.id == id);
            if (cards.Any())
            {
                var card = cards.FirstOrDefault();
                _db.Remove(cards.FirstOrDefault());
                _db.SaveChanges();
                return (card, Get());
            }
            else
                return (null, null);
        }
    }
}
