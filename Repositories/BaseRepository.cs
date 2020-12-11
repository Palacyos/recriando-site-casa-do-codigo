using CasaDoCodigo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
    public class BaseRepository<T> where T: BaseModel
    {
        protected ApplicationContext Context { get; }

        protected DbSet<T> Db { get; }

        public BaseRepository(ApplicationContext context)
        {
            Context = context;
            Db = context.Set<T>();
        }
    }
}
