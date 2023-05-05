using BlindBox.Models;

namespace BlindBox.IServers.IDtoServers
{
    public interface IBaseDtoService<T>
    {
        IQueryable<T> ModelQueryable { get; }
        Task<T> CreateAsync(T t);
        Task<T> UpdateAsync(T t);
        Task<bool> DeleteAsync(int? t);

        Task<T> SingAsync(int? t);
        Task<List<T>> FuzzyAsync(string name);
    }
}