using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace com.microsoft
{
    public class WriteQueueTimer
    {
        [FunctionName("WriteQueueTimer")]
        [return: Queue("game-results", Connection="AzureWebJobsStorage")]
        public GameResult Run([TimerTrigger("*/90 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var gameResult = GameResult.GenerateResult();

            // maybe do some other interesting stuff?

            return gameResult;
        }
    }
}
