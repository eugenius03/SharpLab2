using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.SignalR;
using SharpLab2.Models;
using SharpLab2.Services;

namespace SharpLab2.Hubs;

public abstract class CrudHub<T>(IBaseService<T> service) : Hub where T : class, IEntity
{
    public async Task<List<T>> GetAll()
    {
        return await service.GetAllAsync();
    }

    public async Task<T?> GetById(int id)
    {
        return await service.GetByIdAsync(id);
    }

    public async Task<bool> Exists(int id)
    {
        return await service.ExistsAsync(id);
    }

    public async Task<T> Create(T entity)
    {
        Validate(entity);

        await service.CreateAsync(entity);
        await Clients.All.SendAsync("Changed");

        return entity;
    }

    public async Task<bool> Update(T entity)
    {
        Validate(entity);

        if (!await service.ExistsAsync(entity.Id))
        {
            return false;
        }

        await service.UpdateAsync(entity);
        await Clients.All.SendAsync("Changed");

        return true;
    }

    public async Task<bool> Delete(int id)
    {
        if (!await service.ExistsAsync(id))
        {
            return false;
        }

        await service.DeleteAsync(id);
        await Clients.All.SendAsync("Changed");

        return true;
    }

    private static void Validate(T entity)
    {
        var results = new List<ValidationResult>();
        if (!Validator.TryValidateObject(entity, new ValidationContext(entity), results, validateAllProperties: true))
        {
            throw new HubException(string.Join(" ", results.Select(r => r.ErrorMessage)));
        }
    }
}
