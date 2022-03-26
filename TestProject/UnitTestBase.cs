using BACK.Data;
using BACK.Models;
using BACK.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace TestProject
{
    public class UnitTestBase
    {
        private readonly int TAMANHO = 10;
        private readonly string[] listas = { "ToDo", "Doing", "Done" };

        [Fact(DisplayName = "Primeiro Teste")]
        public void TestBase()
        {

            var opt = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "LetsCodeAntonioDantasDb").Options;
            var _db = new ApplicationDbContext(opt);

            for (int i = 0; i < TAMANHO; i++)
            {
                new CardRepository(_db).Post(new Card
                {
                    conteudo = "Novo Conteúdo " + Guid.NewGuid(),
                    titulo = "Teste Automatizado",
                    lista = listas[0]
                });
            }

            var cards = new CardRepository(_db).Get();

            for (int i = 0; i < TAMANHO / 2; i++)
            {
                new CardRepository(_db).Put(cards[i]);
            }

            for (int i = TAMANHO / 2 + 1; i < TAMANHO; i++)
            {
                if (new Random().Next(10) % 2 == 0)
                    new CardRepository(_db).Delete(cards[i].id);
            }


        }
    }
}