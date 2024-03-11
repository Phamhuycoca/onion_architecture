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
        bool Create(T entity);
        bool Update(T entity);
        bool Delete(long id);
        T GetById(long id);
    }
}
