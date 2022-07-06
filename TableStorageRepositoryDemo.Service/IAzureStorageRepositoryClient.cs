using TableStorageRepositoryDemo.Models;

namespace TableStorageRepositoryDemo.Service;

public interface IAzureStorageRepositoryClient<T> where T : IBaseAzureStorageEntityModel
{
    Task<T?> UpsertAsync(T entityDetails);
    Task<T?> GetOneAsync(string id);
    Task<List<T>?> GetAllAsync();
    Task DeletAsync(string id);
}
