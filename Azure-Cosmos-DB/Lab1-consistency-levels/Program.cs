using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using DotNetEnv;

class Program{

   static async Task Main(string[] args){
      Env.Load();

      string EndpointUri = Environment.GetEnvironmentVariable("COSMOS_ENDPOINT");
      string PrimaryKey = Environment.GetEnvironmentVariable("COSMOS_KEY");

      // Step 1: Create a CosmosClient
      var client = new CosmosClient(EndpointUri, PrimaryKey);

      // Step 2: Create a database and container
      var database = await client.CreateDatabaseIfNotExistsAsync("ConsistencyLab");
      var container = await database.Database.CreateContainerIfNotExistsAsync(
         id: "Products",
         partitionKeyPath: "/categoryId",
         throughput: 400
      );

      Console.WriteLine("Database and container created successfully!");

      // Step 3: Insert an item into the container
      var item = new { id = "1", name = "Product A", categoryId = "Electronics", price = 100 };
      await container.Container.UpsertItemAsync(item);
      Console.WriteLine("Item inserted successfully!");

      // Step 4: Read the item using default consistency (Session)
      // var responseDefault = await container.Container.ReadItemAsync<dynamic>(
      //       id: "1",
      //       partitionKey: new PartitionKey("Electronics")
      // );
      // Console.WriteLine($"Default consistency read: {responseDefault.Resource.name}");

      // Step 5: Read the item using Strong consistency
      var requestOptionsStrong = new ItemRequestOptions { ConsistencyLevel = ConsistencyLevel.Strong };
      var responseStrong = await container.Container.ReadItemAsync<dynamic>(
            id: "1",
            partitionKey: new PartitionKey("Electronics"),
            requestOptions: requestOptionsStrong
      );
      Console.WriteLine($"Strong consistency read: {responseStrong.Resource.name}");

      // Step 6: Read the item using Eventual consistency
      // var clientEventual = new CosmosClient(EndpointUri, PrimaryKey, new CosmosClientOptions
      // {
      //       ConsistencyLevel = ConsistencyLevel.Eventual
      // });
      // var eventualContainer = clientEventual.GetContainer("ConsistencyLab", "Products");
      // var responseEventual = await eventualContainer.ReadItemAsync<dynamic>(
      //       id: "1",
      //       partitionKey: new PartitionKey("Electronics")
      // );
      // Console.WriteLine($"Eventual consistency read: {responseEventual.Resource.name}");
   }
}