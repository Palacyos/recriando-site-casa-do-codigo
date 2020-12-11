using CasaDoCodigo.Models;
using CasaDoCodigo.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using static CasaDoCodigo.Repositories.ProdutoRepository;

namespace CasaDoCodigo
{
    public class DataService : IDataService
    {
        private ApplicationContext Context { get; }

        private IProdutoRepository ProdutoRepository { get; }

        public DataService(ApplicationContext context, IProdutoRepository produtoRepository)
        {
            Context = context;
            ProdutoRepository = produtoRepository;
        }

        public void InicializaDB()
        {
            Context.Database.EnsureCreated();

            List<Livro> livros = GetLivros();

            ProdutoRepository.SalvaProdutos(livros);
        }

        
        private static List<Livro> GetLivros()
        {
            var json = File.ReadAllText("livros.json");
            var livros = JsonConvert.DeserializeObject<List<Livro>>(json);
            return livros;
        }

        
    }
}
