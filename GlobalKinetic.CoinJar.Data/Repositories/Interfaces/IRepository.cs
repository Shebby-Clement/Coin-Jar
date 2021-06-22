using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GlobalKinetic.CoinJar.Data.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {

        void Add(T entity);
        void Update(T entity);
        void Delete(Guid entityId);
        void Delete(Expression<Func<T, bool>> where);
        T GetById(Guid id);
        T Get(Expression<Func<T, bool>> where);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);
    }
}
