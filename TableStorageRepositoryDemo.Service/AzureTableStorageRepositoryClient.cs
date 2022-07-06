using TableStorageRepositoryDemo.Models;
using TableStorageRepositoryDemo.Repository;

namespace TableStorageRepositoryDemo.Service;

public abstract class AzureTableStorageRepositoryClient<T> : IAzureStorageRepositoryClient<T> where T : IBaseAzureStorageEntityModel
{
    private readonly IAzureStorageRepository<T> _tableStorageRepository;

    public AzureTableStorageRepositoryClient(IAzureStorageRepository<T> tableStorageRepository)
    {
        _tableStorageRepository = tableStorageRepository;
    }

    public virtual Task DeletAsync(string id)
    {
        try {
            return _tableStorageRepository.DeletAsync(id);
        }
        catch {
            // Do nothing
        }
        
        return Task.CompletedTask;
    }

    public virtual async Task<List<T>?> GetAllAsync()
    {
        try {
            return await _tableStorageRepository.GetAllAsync();
        }
        catch {
            // Do nothing, return null
        }

        return null;
    }

    public virtual async Task<T?> GetOneAsync(string id)
    {
        try {
            return await _tableStorageRepository.GetOneAsync(id);
        }
        catch {
            // Do nothing, return null
        }

        return default(T);
    }

    public virtual async Task<T?> UpsertAsync(T entityDetails)
    {
        try {
            var results = await _tableStorageRepository.UpsertAsync(entityDetails);

            return results;
        }
        catch {
            // Do nothing, return null
        }

        return default(T);
    }
}