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


namespace com.microsoft
{
    public class HttpQueryCosmosBinding
    {
        private readonly ILogger<HttpQueryCosmosBinding> _logger;

        public HttpQueryCosmosBinding(ILogger<HttpQueryCosmosBinding> log)
        {
            _logger = log;
        }

        [FunctionName("HttpQueryCosmosBinding")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiParameter(name: "searchTerm", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [CosmosDB(databaseName:"scores", collectionName:"allScores", ConnectionStringSetting = "CosmosConnectionString", SqlQuery = "select * from c where c.solution = '{Query.searchTerm}'")] IEnumerable<GameResult> allResults
        )
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");


            return new OkObjectResult(allResults);
        }
    }
}

