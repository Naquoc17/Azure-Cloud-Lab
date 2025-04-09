const { MongoClient } = require("mongodb");
require("dotenv").config();

const connectionString = process.env.PRIMARY_CONNECTION_STRING;

async function main() {
   const client = new MongoClient(connectionString);

   try {
      await client.connect();
      console.log("Connected to MongoDB API");

      const database = client.db("StoreDB");
      const collection = database.collection("Orders");

      const newOrder = {
         orderId: "ORD-001",
         customerName: "Thomas",
         items: [
            { product: "Phone", quantity: 1, price: 699.99 },
            { product: "Headphones", quantity: 2, price: 300.0 },
         ],
         totalAmount: 999.99,
         orderDate: new Date(),
      };

      const result = await collection.insertOne(newOrder);
      console.log(`Document inserted with _id: ${result.insertedId}`);

      const query = { totalAmount: { $gt: 500 } };
      const orders = await collection.find(query).toArray();

      console.log("Found the following orders:");
      console.log(orders);
   } finally {
      await client.close();
   }
}

main().catch(console.error);
