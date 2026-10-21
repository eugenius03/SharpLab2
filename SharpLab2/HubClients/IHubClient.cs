using SharpLab2.Models;
using SharpLab2.Services;

namespace SharpLab2.HubClients;

public interface IHubClient<T> : IBaseService<T> where T : class, IEntity
{
    event Action? Changed;

    // Повертають false, якщо запис уже не існує.
    Task<bool> TryUpdateAsync(T entity);
    Task<bool> TryDeleteAsync(int id);
}
