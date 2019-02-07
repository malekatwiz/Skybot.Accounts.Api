using System;
using System.Threading.Tasks;

namespace Skybot.Accounts.Api.Data
{
    public interface IRepository<T>
    {
        Task<T> Add(T document);
        Task<T> Get(Guid id);
        T GetBy(Func<T, bool> func);
        Task Update(T item);
    }
}
