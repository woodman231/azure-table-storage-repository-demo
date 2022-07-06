using Azure.Data.Tables;
using TableStorageRepositoryDemo.Models;

namespace TableStorageRepositoryDemo.Repository;

public class TodoAzureTableStorageRepository : AzureTableStorageRepository<Todo>, ITodoAzureTableStorageRepository
{
    public TodoAzureTableStorageRepository(TableServiceClient tableServiceClient) : base(tableServiceClient, "TodosTable", "TodoItems")
    {
    }
}