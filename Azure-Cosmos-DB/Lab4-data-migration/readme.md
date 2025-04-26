# Simulating Azure Cosmos DB Migration within a single Subscription

## Part 1: Set up source env

### Step 1: Create a source Cosmos DB account

1. Log in to Azure and set subscription

```bash
az login
az account set --subscription "<Subscription-ID>"
```

2. Create resource group and source Cosmos DB account

```bash
az group create --name rg-cosmosdb-source --location westus3
az cosmosdb create --name source-cosmosdb-lab --resource-group rg-cosmosdb-source --kind GlobalDocumentDB --default-consistency-level Session
```
3. Create db and container in the source account

