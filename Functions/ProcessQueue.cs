using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace com.microsoft
{
    public class ProcessQueue
    {
        [FunctionName("ProcessQueue")]
        public void Run(
            [QueueTrigger("game-results", Connection = "AzureWebJobsStorage")]GameResult myQueueItem, 
            [CosmosDB(databaseName:"scores", collectionName:"allScores", ConnectionStringSetting = "CosmosConnectionString")]out dynamic cosmosDocument,
        ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem.Solution}");

            cosmosDocument = myQueueItem;
        }
    }
}
