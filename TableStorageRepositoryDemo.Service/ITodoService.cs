using TableStorageRepositoryDemo.Models;

namespace TableStorageRepositoryDemo.Service;

public interface ITodoService : IAzureStorageRepositoryClient<Todo>
{
}