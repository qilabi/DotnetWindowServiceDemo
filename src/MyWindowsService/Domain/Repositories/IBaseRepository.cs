using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWindowsService.Domain.Repositories
{
    public interface IBaseRepository<TEntity>
        where TEntity : class
    {
        Task Create(TEntity obj);
        void Update(string id,TEntity obj);
        void Delete(string id);
        Task<TEntity> Get(string id);
        Task<IEnumerable<TEntity>> Get();
    }
}
