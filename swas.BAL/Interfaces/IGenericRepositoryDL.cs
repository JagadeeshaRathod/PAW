using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.BAL.Interfaces
{ 
    public interface IGenericRepositoryDL<T> where T : class
    {
        Task<T> Get(int id);
        Task<T> GetByByte(int id);
        Task<IEnumerable<T>> GetAll();
        Task Add(T entity);
        Task<T> AddWithReturn(T entity);
        Task Delete(T entity);
        Task<T> Delete(int id);
        Task Update(T entity);
        Task<T> UpdateWithReturn(T entity);
    }
}
