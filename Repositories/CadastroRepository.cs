using CasaDoCodigo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
    public class CadastroRepository : BaseRepository<Cadastro>, ICadastroRepository
    {
        public CadastroRepository(ApplicationContext context) : base(context)
        {
        }

        public Cadastro Update(int cadastroId, Cadastro novoCdastro)
        {
            var cadastroDb = Db.Where(c => c.Id == cadastroId).SingleOrDefault();

            if (cadastroDb == null)
            {
                throw new ArgumentException("cadastro");
            }

            cadastroDb.Update(novoCdastro);

            Context.SaveChanges();

            return cadastroDb;
        }
    }
}
