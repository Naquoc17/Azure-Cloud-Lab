using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Azure.Cosmos;

public class Program{
    public static readonly string EndpointUri = "https://cosmos-db-aq-test-1.documents.azure.com:443/";
    public static readonly string PrimaryKey = "km7uFawid7NoLWxh9mccp99t4X2Z5gsdtoaoHe3MxSGXoqvC3ofJURttE5EdJcDMGxkFrni0vJINACDbXE5GgQ==";

    private CosmosClient cosmosClient;
    private Database database;
    private Container container;

    private string databaseId = "az204Database";
    private string containerId = "az204Container";

    public static async Task Main(string[] args){
        try{
            Console.WriteLine("Beginning operations ... \n");
            Program p = new Program();
            await p.CosmosAsync();
        }
        catch (CosmosException de){
            Exception baseException = de.GetBaseException();
            Console.WriteLine("{0} error occurred: {1}", de.StatusCode, de);
        }
        catch (Exception e){
            Console.WriteLine("Error: {0}", e);
        }
        finally {
            Console.WriteLine("End of program, press any key to exit.");
            Console.ReadKey();
        }
    }

    public async Task CosmosAsync(){
        this.cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
        await this.CreateDatabaseAsync();
        await this.CreateContainerAsync();
    }

    private async Task CreateDatabaseAsync(){
        this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
        Console.WriteLine("Created Database: {0}\n", this.database.Id);
    }
    private async Task CreateContainerAsync()
    {
        // Create a new container
        this.container = await this.database.CreateContainerIfNotExistsAsync(containerId, "/LastName");
        Console.WriteLine("Created Container: {0}\n", this.container.Id);
    }
}