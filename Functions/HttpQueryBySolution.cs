using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System.Linq;

namespace com.microsoft
{
    public class HttpQueryBySolution
    {
        private readonly ILogger<HttpQueryBySolution> _logger;

        public HttpQueryBySolution(ILogger<HttpQueryBySolution> log)
        {
            _logger = log;
        }

        [FunctionName("HttpQueryBySolution")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "find solution" })]
        [OpenApiParameter(name: "word", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The solution word to search for")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<GameResult>), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [CosmosDB(databaseName:"scores", collectionName: "allScores", ConnectionStringSetting ="CosmosConnectionString")] DocumentClient documentClient
        )
        {
            string searchedTerm = req.Query["word"];
            // _logger.LogInformation($"the query is: {name}");

            var collectionUri = UriFactory.CreateDocumentCollectionUri("scores","allScores");

            var query = documentClient.CreateDocumentQuery<GameResult>(collectionUri)
                .Where(result => result.Solution == searchedTerm).AsDocumentQuery();

            List<GameResult> allResults = new();

            while (query.HasMoreResults)
            {
                allResults.AddRange((await query.ExecuteNextAsync<GameResult>()));
            }

            return new OkObjectResult(allResults);
        }
    }
}

