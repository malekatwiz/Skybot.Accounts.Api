using System;
using System.Threading.Tasks;

namespace Skybot.Accounts.Api.Data
{
    public interface IRepository<T>
    {
        Task<T> AddAsync(T document);
        Task<T> GetAsync(string id, string partitionKey = null);
        T GetBy(Func<T, bool> func);
        Task UpdateAsync(T item);
        Task ReplaceAsync(string id, T item);
    }
}
