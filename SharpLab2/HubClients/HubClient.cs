using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using SharpLab2.Models;

namespace SharpLab2.HubClients;

public abstract class HubClient<T>(NavigationManager navigation, string hubPath) : IHubClient<T>, IAsyncDisposable
    where T : class, IEntity
{
    private readonly SemaphoreSlim _startLock = new(1, 1);
    private HubConnection? _connection;

    public event Action? Changed;

    public async Task<List<T>> GetAllAsync()
    {
        var connection = await GetConnectionAsync();
        return await connection.InvokeAsync<List<T>>("GetAll");
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        var connection = await GetConnectionAsync();
        return await connection.InvokeAsync<T?>("GetById", id);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        var connection = await GetConnectionAsync();
        return await connection.InvokeAsync<bool>("Exists", id);
    }

    public async Task CreateAsync(T entity)
    {
        var connection = await GetConnectionAsync();
        var created = await connection.InvokeAsync<T>("Create", entity);
        entity.Id = created.Id;
    }

    public async Task UpdateAsync(T entity)
    {
        await TryUpdateAsync(entity);
    }

    public async Task DeleteAsync(int id)
    {
        await TryDeleteAsync(id);
    }

    public async Task<bool> TryUpdateAsync(T entity)
    {
        var connection = await GetConnectionAsync();
        return await connection.InvokeAsync<bool>("Update", entity);
    }

    public async Task<bool> TryDeleteAsync(int id)
    {
        var connection = await GetConnectionAsync();
        return await connection.InvokeAsync<bool>("Delete", id);
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection != null)
        {
            await _connection.DisposeAsync();
        }

        _startLock.Dispose();
    }

    private async Task<HubConnection> GetConnectionAsync()
    {
        await _startLock.WaitAsync();
        try
        {
            _connection ??= CreateConnection();

            if (_connection.State == HubConnectionState.Disconnected)
            {
                await _connection.StartAsync();
            }

            return _connection;
        }
        finally
        {
            _startLock.Release();
        }
    }

    private HubConnection CreateConnection()
    {
        var connection = new HubConnectionBuilder()
            .WithUrl(navigation.ToAbsoluteUri(hubPath))
            .AddJsonProtocol(options =>
            {
                // Сервер повертає граф сутностей з циклічними посиланнями (Ticket <-> Passenger тощо).
                options.PayloadSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;

                // На сервер відправляємо лише прості властивості (без навігаційних),
                // щоб не пересилати весь граф при створенні та редагуванні.
                options.PayloadSerializerOptions.TypeInfoResolver = new DefaultJsonTypeInfoResolver
                {
                    Modifiers = { IgnoreNavigationProperties }
                };
            })
            .WithAutomaticReconnect()
            .Build();

        connection.On("Changed", () => Changed?.Invoke());

        return connection;
    }

    private static void IgnoreNavigationProperties(JsonTypeInfo typeInfo)
    {
        if (typeInfo.Kind != JsonTypeInfoKind.Object)
        {
            return;
        }

        foreach (var property in typeInfo.Properties)
        {
            if (IsNavigation(property.PropertyType))
            {
                property.ShouldSerialize = static (_, _) => false;
            }
        }
    }

    private static bool IsNavigation(Type type)
    {
        if (typeof(IEntity).IsAssignableFrom(type))
        {
            return true;
        }

        return type.IsGenericType
               && type.GetGenericTypeDefinition() == typeof(List<>)
               && typeof(IEntity).IsAssignableFrom(type.GetGenericArguments()[0]);
    }
}
