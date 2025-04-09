# Exploring Azure Cosmos DB APIs: A Hands-on Laboratory

## Part 1: Create Azure Cosmos DB Accounts

### Exercise 1 : Create a Cosmos DB Account with NoSQL API

- Log in 
- Search Azure Cosmos DB
- Choose API: Azure Cosmos DB for NoSQL
- Configure basic settings:
  - resource group: rg-cosmos-db
  - account name: naquoc17-nosql
- Click Review + Create -> Create

### Exercise 2: Create a Cosmos DB Account with MongoDB API

- Do same steps in the exercise 1
- Choose API: Azure Cosmos DB for MongoDB
- Configure basic settings:
  - resource group: rg-cosmos-db
  - account name: naquoc17-mongodb

## Part 2: Working with the NoSQL API

### Exercise 3: Create a DB and COntainer in NoSQL API

- Navigate to NoSQL API Cosmos DB account
- Select Data Explorer from left menu
- Click New Container
- Create a new DB and container:
  - DB id: ProductsDB
  - COntainer id: Inventory
  - Partition key: /category
  - Thoughput: 1000 RU/s (default)
- Click OK

### Exercise 4: Add items to the NoSQL Container

- In Data Explorer, expand ProductsDB then Inventory and select Items
- Click New Item and add the following JSON document:

```json
{
  "id": "item1",
  "name": "Laptop Computer",
  "category": "electronics",
  "price": 999.99,
  "quantity": 15,
  "description": "High-performance laptop with 16GB RAM",
  "inStock": true
}
```

- Click Save
- Add 2 more items with dif categories

```json
{
  "id": "item2",
  "name": "Fuji xt20",
  "category": "cameras",
  "price": 499.99,
  "quantity": 27,
  "description": "High-performance camera",
  "inStock": true
}
```

### Exercise 5: Query Using SQL Syntax in NoSQL API

- In Data Explorer, select New SQL Query
- Execute:

```sql
SELECT * FROM c where c.category = "electronics"

SELECT c.id, c.name, c.price FROM c where c.price > 500

SELECT COUNT(1) AS TotalProducts FROM c
```

- Observe the results and SQL like syntax used with JSON documents

## Part 3: Working with MongoDB API

### Exercise 6: Create a Database and Collection in MongoDB API

- Navigate to MongoDB API Cosmos DB account
- Select Data Explorer from left menu
- Create a new DB and collection:
  - DB id: StoreDB
  - Collection id: Orders
  - Shared key: _id
  - Throughput: 1000
- Click OK

### Exercise 7: Connect with Node.js App

- Create a new directory on local machine and initialize a Node.js project

```bash
mkdir cosmos-mongo-test
cd cosmos-mongo-test
npm init -y
npm install mongodb
```

- Search "Quick start" and find PRIMARY CONNECTION STRING
- Create a file named app.js with the following code:

```JS
const { MongoClient } = require('mongodb');

const connectionString = "mongodb://<account-name>:<primary-key>@<account-name>.mongo.cosmos.azure.com:10255/?ssl=true&replicaSet=globaldb";

async function main() {
  const client = new MongoClient(connectionString);
  
  try {
    await client.connect();
    console.log("Connected to MongoDB API");
    
    const database = client.db('StoreDB');
    const collection = database.collection('Orders');
    
    // Insert a new document
    const newOrder = {
      orderId: "ORD-001",
      customerName: "John Doe",
      items: [
        { product: "Phone", quantity: 1, price: 699.99 },
        { product: "Headphones", quantity: 1, price: 149.99 }
      ],
      totalAmount: 849.98,
      orderDate: new Date()
    };
    
    const result = await collection.insertOne(newOrder);
    console.log(`Document inserted with _id: ${result.insertedId}`);
    
    // Query documents
    const query = { totalAmount: { $gt: 500 } };
    const orders = await collection.find(query).toArray();
    console.log("Found the following orders:");
    console.log(orders);
  } finally {
    await client.close();
  }
}

main().catch(console.error);
```

- Run the app

```bash
node app.js
```

## Part 4: Comparing APIs

### Exercise 8: Performance Evaluation

- In NoSQL API Cosmos DB account, navigate to Metrics in left menu
- Observe the RU consumption for the operations we performed in the last exercises
- Compare with RU consumption in MongoDB API account

## Part 5: Multi-Region Configuration

### Exercise 9: Configure Multi-Region Writing

- Navigate to NoSQL API Cosmos DB account
- Select Replicate data globally from the left menu
- Add one additional region to database
- Enable Multi-region Writes option
- Click Save
