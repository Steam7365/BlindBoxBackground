using BlindBox.Models;

namespace BlindBox.IServers
{
    public interface IBaseService<T>
    {
        IQueryable<T> ModelQueryable { get; }
        Task<T> CreateAsync(T t);
        Task<T> UpdateAsync(T t);
        Task<bool> DeleteAsync(int? t);

        Task<T> SingAsync(int? t);
    }
}