using System;
using Microsoft.Azure.Cosmos;

namespace BlazorCosmosDemo.Data
{
    public class EngineerService : IEngineerService
    {
        private readonly string CosmosDbConnectionString = "";
        private readonly string CosmosDBName = "Contractors";
        private readonly string CosmosDBContainerName = "Enginners";

        private Container GetContainerClient()
        {
            var cosmosDbClinet = new CosmosClient(CosmosDbConnectionString);
            var conatiner = cosmosDbClinet.GetContainer(CosmosDBName, CosmosDBContainerName);
            return conatiner;
        }

        public async Task UpsertEngineer(Engineer engineer)
        {
            try
            {
                if (engineer.id == null)
                {
                    engineer.id = Guid.NewGuid();
                }
                var container = GetContainerClient();
                var updateRes = await container.UpsertItemAsync(engineer, new PartitionKey(engineer.id.ToString()));
                Console.Write(updateRes.StatusCode);
            }
            catch (Exception ex)
            {
                throw new Exception("Exception", ex);
            }
        }

        public async Task DeleteEngineer(string? id, string? partitionKey)
        {
            try
            {
                var container = GetContainerClient();
                var response = await container.DeleteItemAsync<Engineer>(id, new PartitionKey(partitionKey));
                Console.Write(response.StatusCode);
            }
            catch (Exception ex)
            {
                throw new Exception("Exception :", ex);
            }
        }

        public async Task<List<Engineer>> GetEngineerDetails()
        {
            List<Engineer> lstEngineers = new();
            try
            {
                var container = GetContainerClient();
                var sqlQuery = "SELECT * FROM c";
                QueryDefinition queryDefinition = new QueryDefinition(sqlQuery);
                FeedIterator<Engineer> queryResultSetIterator = container.GetItemQueryIterator<Engineer>(queryDefinition);

                while (queryResultSetIterator.HasMoreResults)
                {
                    FeedResponse<Engineer> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                    foreach (Engineer engineer in currentResultSet)
                    {
                        lstEngineers.Add(engineer);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return lstEngineers;
        }

        public async Task<Engineer> GetEngineerById(string? id, string? partitionKey)
        {
            try
            {
                var container = GetContainerClient();
                ItemResponse<Engineer> response = await container.ReadItemAsync<Engineer>(id, new PartitionKey(partitionKey));
                return response.Resource;

            }
            catch (Exception ex)
            {
                throw new Exception("Exception :", ex);
            }
        }
    }
}

