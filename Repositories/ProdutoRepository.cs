using CasaDoCodigo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
    public class ProdutoRepository : BaseRepository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(ApplicationContext context) : base(context)
        {
        }

        public void SalvaProdutos(List<Livro> livros)
        {

            foreach (var livro in livros)
            {
                if (!Db.Where(p => p.Codigo == livro.Codigo).Any())
                {
                    Db.Add(new Produto(livro.Codigo, livro.Nome, livro.Preco));
                }
            }
            Context.SaveChanges();
        }

        public IList<Produto> GetProdutos()
        {
            return Db.ToList();
        }

        public class Livro
        {
            public string Codigo { get; set; }
            public string Nome { get; set; }
            public decimal Preco { get; set; }
        }
    }
}
