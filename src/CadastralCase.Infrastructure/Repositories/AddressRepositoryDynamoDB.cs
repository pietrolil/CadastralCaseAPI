using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using CadastralCase.Domain.Entities;
using CadastralCase.Domain.Interfaces;
using System.Text.Json;

namespace CadastralCase.Infrastructure.Repositories;

/// <summary>
/// Repository for Addresses using DynamoDB (NoSQL)
/// Implements Repository Pattern for NoSQL data layer abstraction
/// </summary>
public class AddressRepositoryDynamoDB : IAddressRepository
{
    private readonly IAmazonDynamoDB _dynamoDbClient;
    private readonly string _tableName = "Addresses";

    public AddressRepositoryDynamoDB(IAmazonDynamoDB dynamoDbClient)
    {
        _dynamoDbClient = dynamoDbClient;
    }

    public async Task<Address?> GetByIdAsync(Guid id)
    {
        var request = new GetItemRequest
        {
            TableName = _tableName,
            Key = new Dictionary<string, AttributeValue>
            {
                { "Id", new AttributeValue { S = id.ToString() } }
            }
        };

        var response = await _dynamoDbClient.GetItemAsync(request);
        
        if (response.Item == null || response.Item.Count == 0)
            return null;

        return MapToEntity(response.Item);
    }

    public async Task<IEnumerable<Address>> GetAllAsync()
    {
        var request = new ScanRequest
        {
            TableName = _tableName
        };

        var response = await _dynamoDbClient.ScanAsync(request);
        return response.Items.Select(MapToEntity).ToList();
    }

    public async Task<IEnumerable<Address>> GetActiveAsync()
    {
        var request = new ScanRequest
        {
            TableName = _tableName,
            FilterExpression = "IsActive = :isActive",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":isActive", new AttributeValue { BOOL = true } }
            }
        };

        var response = await _dynamoDbClient.ScanAsync(request);
        return response.Items.Select(MapToEntity).ToList();
    }

    public async Task<Address?> GetByPostalCodeAsync(string postalCode)
    {
        var cleanPostalCode = postalCode.Replace("-", "").Replace(".", "");
        
        var request = new ScanRequest
        {
            TableName = _tableName,
            FilterExpression = "PostalCode = :postalCode",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":postalCode", new AttributeValue { S = cleanPostalCode } }
            }
        };

        var response = await _dynamoDbClient.ScanAsync(request);
        var item = response.Items.FirstOrDefault();
        
        return item != null ? MapToEntity(item) : null;
    }

    public async Task<IEnumerable<Address>> GetByCityAsync(string city, string state)
    {
        var request = new ScanRequest
        {
            TableName = _tableName,
            FilterExpression = "City = :city AND State = :state",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":city", new AttributeValue { S = city } },
                { ":state", new AttributeValue { S = state.ToUpper() } }
            }
        };

        var response = await _dynamoDbClient.ScanAsync(request);
        return response.Items.Select(MapToEntity).ToList();
    }

    public async Task<Address> AddAsync(Address entity)
    {
        var item = MapToAttributeValues(entity);

        var request = new PutItemRequest
        {
            TableName = _tableName,
            Item = item
        };

        await _dynamoDbClient.PutItemAsync(request);
        return entity;
    }

    public async Task UpdateAsync(Address entity)
    {
        var item = MapToAttributeValues(entity);

        var request = new PutItemRequest
        {
            TableName = _tableName,
            Item = item
        };

        await _dynamoDbClient.PutItemAsync(request);
    }

    public async Task DeleteAsync(Guid id)
    {
        var request = new DeleteItemRequest
        {
            TableName = _tableName,
            Key = new Dictionary<string, AttributeValue>
            {
                { "Id", new AttributeValue { S = id.ToString() } }
            }
        };

        await _dynamoDbClient.DeleteItemAsync(request);
    }

    public async Task ActivateAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            entity.Activate();
            await UpdateAsync(entity);
        }
    }

    public async Task DeactivateAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            entity.Deactivate();
            await UpdateAsync(entity);
        }
    }

    private static Address MapToEntity(Dictionary<string, AttributeValue> item)
    {
        // Use reflection to create the object (since the constructor is private)
        var address = (Address)System.Runtime.Serialization.FormatterServices
            .GetUninitializedObject(typeof(Address));

        // Set properties using reflection
        SetProperty(address, "Id", Guid.Parse(item["Id"].S));
        SetProperty(address, "PostalCode", item["PostalCode"].S);
        SetProperty(address, "Street", item["Street"].S);
        SetProperty(address, "City", item["City"].S);
        SetProperty(address, "State", item["State"].S);
        SetProperty(address, "StateName", item["StateName"].S);
        SetProperty(address, "CreatedAt", DateTime.Parse(item["CreatedAt"].S));
        SetProperty(address, "IsActive", item["IsActive"].BOOL);

        if (item.ContainsKey("Complement") && !string.IsNullOrEmpty(item["Complement"].S))
            SetProperty(address, "Complement", item["Complement"].S);

        if (item.ContainsKey("Number") && !string.IsNullOrEmpty(item["Number"].S))
            SetProperty(address, "Number", item["Number"].S);

        if (item.ContainsKey("District") && !string.IsNullOrEmpty(item["District"].S))
            SetProperty(address, "District", item["District"].S);

        if (item.ContainsKey("IbgeCode") && !string.IsNullOrEmpty(item["IbgeCode"].S))
            SetProperty(address, "IbgeCode", item["IbgeCode"].S);

        if (item.ContainsKey("AreaCode") && !string.IsNullOrEmpty(item["AreaCode"].S))
            SetProperty(address, "AreaCode", item["AreaCode"].S);

        if (item.ContainsKey("UpdatedAt") && !string.IsNullOrEmpty(item["UpdatedAt"].S))
            SetProperty(address, "UpdatedAt", DateTime.Parse(item["UpdatedAt"].S));

        return address;
    }

    private static void SetProperty(object obj, string propertyName, object value)
    {
        var property = obj.GetType().GetProperty(propertyName);
        if (property != null)
        {
            property.SetValue(obj, value);
        }
    }

    private static Dictionary<string, AttributeValue> MapToAttributeValues(Address address)
    {
        var item = new Dictionary<string, AttributeValue>
        {
            { "Id", new AttributeValue { S = address.Id.ToString() } },
            { "PostalCode", new AttributeValue { S = address.PostalCode } },
            { "Street", new AttributeValue { S = address.Street } },
            { "City", new AttributeValue { S = address.City } },
            { "State", new AttributeValue { S = address.State } },
            { "StateName", new AttributeValue { S = address.StateName } },
            { "CreatedAt", new AttributeValue { S = address.CreatedAt.ToString("O") } },
            { "IsActive", new AttributeValue { BOOL = address.IsActive } }
        };

        if (!string.IsNullOrEmpty(address.Complement))
            item.Add("Complement", new AttributeValue { S = address.Complement });

        if (!string.IsNullOrEmpty(address.Number))
            item.Add("Number", new AttributeValue { S = address.Number });

        if (!string.IsNullOrEmpty(address.District))
            item.Add("District", new AttributeValue { S = address.District });

        if (!string.IsNullOrEmpty(address.IbgeCode))
            item.Add("IbgeCode", new AttributeValue { S = address.IbgeCode });

        if (!string.IsNullOrEmpty(address.AreaCode))
            item.Add("AreaCode", new AttributeValue { S = address.AreaCode });

        if (address.UpdatedAt.HasValue)
            item.Add("UpdatedAt", new AttributeValue { S = address.UpdatedAt.Value.ToString("O") });

        return item;
    }
}
