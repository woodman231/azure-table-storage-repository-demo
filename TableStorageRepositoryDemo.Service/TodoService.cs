using TableStorageRepositoryDemo.Models;
using TableStorageRepositoryDemo.Repository;

namespace TableStorageRepositoryDemo.Service;

public class TodoService : AzureTableStorageRepositoryClient<Todo>, ITodoService
{
    public TodoService(ITodoAzureTableStorageRepository repository) : base (repository)
    {        
    }
}