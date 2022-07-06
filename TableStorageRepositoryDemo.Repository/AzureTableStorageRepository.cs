using Azure;
using Azure.Data.Tables;
using System.Reflection;
using TableStorageRepositoryDemo.Models;

namespace TableStorageRepositoryDemo.Repository;

public abstract class AzureTableStorageRepository<T> : IAzureStorageRepository<T> where T : IBaseAzureStorageEntityModel
{
    private readonly TableServiceClient _tableServiceClient;
    private readonly string _tableName;
    private readonly string _partitionKey;

    public AzureTableStorageRepository(TableServiceClient tableServiceClient, string tableName, string partitionKey)
    {
        _tableServiceClient = tableServiceClient;
        _tableName = tableName;
        _partitionKey = partitionKey;
    }

    public async Task DeletAsync(string id)
    {
        var tableClient = await GetTableClientAsync();

        await tableClient.DeleteEntityAsync(_partitionKey, id);
    }

    public async Task<List<T>?> GetAllAsync()
    {
        var tableClient = await GetTableClientAsync();

        var results = new List<T>();

        Pageable<TableEntity> queryResults = tableClient.Query<TableEntity>(w => w.PartitionKey == _partitionKey);

        foreach (var qEntity in queryResults)
        {
            if (qEntity is not null)
            {
                var result = TableEntityToEntity(qEntity);
                if (result is not null)
                {
                    results.Add(result);
                }
            }
        }

        return results;
    }

    public async Task<T?> GetOneAsync(string id)
    {
        var tableClient = await GetTableClientAsync();

        var tableEntity = await tableClient.GetEntityAsync<TableEntity>(_partitionKey, id);

        if (tableEntity is not null)
        {
            var result = TableEntityToEntity(tableEntity);

            if (result is not null)
            {
                return result;
            }
        }

        return default(T);
    }

    public async Task<T?> UpsertAsync(T entityDetails)
    {
        var tableClient = await GetTableClientAsync();

        var tableEntity = EntityToTableEntity(entityDetails);

        await tableClient.UpsertEntityAsync(tableEntity);

        return entityDetails;
    }

    private TableEntity EntityToTableEntity(T entity)
    {
        var result = new TableEntity();
        result.PartitionKey = _partitionKey;

        Type t = typeof(T);
        PropertyInfo[] entityProperties = t.GetProperties();

        foreach (var entityProperty in entityProperties)
        {

            var propertyName = entityProperty.Name;

            if (propertyName == "Id")
            {
                result.RowKey = entity.Id;
            }
            else
            {
                var propertyValue = entityProperty.GetValue(entity);

                result[propertyName] = propertyValue;
            }

        }

        return result;
    }

    private T TableEntityToEntity(TableEntity tableEntity)
    {
        var result = (T)Activator.CreateInstance<T>();

        Type t = typeof(T);
        PropertyInfo[] entityProperties = t.GetProperties();

        foreach (var entityProperty in entityProperties)
        {

            var propertyName = entityProperty.Name;

            if (propertyName == "Id")
            {
                result.Id = tableEntity.RowKey;
            }
            else
            {

                var propertyValue = tableEntity[propertyName];

                entityProperty.SetValue(result, propertyValue);
            }
        }

        return result;
    }

    private async Task<TableClient> GetTableClientAsync()
    {
        var tableClient = _tableServiceClient.GetTableClient(_tableName);
        await tableClient.CreateIfNotExistsAsync();

        return tableClient;
    }
}