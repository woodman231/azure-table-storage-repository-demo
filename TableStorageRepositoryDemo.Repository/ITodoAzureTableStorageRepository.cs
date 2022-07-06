using TableStorageRepositoryDemo.Models;

namespace TableStorageRepositoryDemo.Repository;

public interface ITodoAzureTableStorageRepository : IAzureStorageRepository<Todo>
{
}