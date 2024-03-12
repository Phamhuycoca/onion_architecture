using onion_architecture.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onion_architecture.Domain.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        List<T> GetAll();
        T Create(T entity);
        T Update(T entity);
        T Delete(long id);
        T GetById(long id);
    }
}
