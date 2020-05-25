const { default: cosmosServer } = require("@zeit/cosmosdb-server");
const { CosmosClient } = require("@azure/cosmos");
const https = require("https");

cosmosServer().listen(3000, () => {
  console.log(`Cosmos DB server running at https://localhost:3000`);

  runClient().catch(console.error);
});

async function runClient() {
  const client = new CosmosClient({
    endpoint: `https://localhost:3000`,
    key: "dummy key",
    // disable SSL verification
    // since the server uses self-signed certificate
    agent: https.Agent({ rejectUnauthorized: false })
  });

  const { database } = await client.databases.createIfNotExists({ id: 'zerofriction' });
  const { container: customersContainer } = await database.containers.createIfNotExists({ id: 'customers', partitionKey: { kind: "Hash", paths: ["/tenantId"] } });
  const { container: invoicesContainer } = await database.containers.createIfNotExists({ id: 'invoices', partitionKey: { kind: "Hash", paths: ["/tenantId"] } });
  const { resource: createdItem } = await customersContainer.items.create({
    id: "1",
    tenantId: "localhost"
  });
}