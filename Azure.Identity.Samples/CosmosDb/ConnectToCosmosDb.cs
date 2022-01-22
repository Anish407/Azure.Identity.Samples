// See https://aka.ms/new-console-template for more information

//Install - Package Azure.Identity - Version 1.5.0
//Install - Package Microsoft.Azure.Cosmos - Version 3.23.0

using Azure.Identity;
using Azure.Identity.Samples.Models;
using Microsoft.Azure.Cosmos;



try
{
    string dbName = "Demo";
    string containerName = "mycont1";
    string accountEndpoint = "https://rbacsampleanish.documents.azure.com:443/";

    // system assigned Managed Identity
    CosmosClient cosmosClient = new CosmosClient(accountEndpoint, new DefaultAzureCredential()); 

    //Use the below option when using a User Assigned Managed Identity
    //new DefaultAzureCredential(new DefaultAzureCredentialOptions { ManagedIdentityClientId="User Assigned MI's AppId" }));
    Container container = cosmosClient.GetContainer(dbName, containerName);

    await QueryItemsAsync(container);

    //Without Assigning Write Roles
    /// Response status code does not indicate success: 
    /// Forbidden(403); Substatus: 5302; ActivityId: c7d18a67 - 1644 - 4623 - b60c - ff6869ff9b62; 
    /// Reason: (Message: {
    //  "Errors":["Request is blocked because principal [myobjectId]
    //  does not have the required RBAC permissions to perform action
    await CreateItem(new RbacDataModel
    {
        id = Guid.NewGuid().ToString(),
        Name = "AnishDemo"
    }, container);



    Console.ReadKey();
}
catch (CosmosException ex)
{
    throw;
}

async Task CreateItem(RbacDataModel rbacDataModel, Container container)
{
    var response = await container.CreateItemAsync(rbacDataModel, new PartitionKey(rbacDataModel.id));
}

Console.ReadLine();


async Task QueryItemsAsync(Container container)
{
    var queryResultSetIterator = await container.ReadItemAsync<RbacDataModel>("1", new PartitionKey("1"));
    var data = queryResultSetIterator.Resource;
    Console.WriteLine($"Id: {data.id}, name:{data.Name}");
}


Console.WriteLine("Hello, World!");
