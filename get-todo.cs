using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Configuration;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using assignment.Models;
using System.Net;

namespace assignment
{
    public static class get_todo
    {
        private static DocumentClient client;
        private static readonly string ENDPOINT = Environment.GetEnvironmentVariable("AccountEndpoint", EnvironmentVariableTarget.Process);
        private static readonly string KEY = Environment.GetEnvironmentVariable("AccountKey", EnvironmentVariableTarget.Process);


        [FunctionName("get-todo")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            //  string id = req.Query["id"];


            using (client = new DocumentClient(new Uri(ENDPOINT), KEY))
            {
                var response = await ReadDocument("TodoDb", "Todos", "1");

                return new OkObjectResult(response);
            }


        }

        private static async Task<IActionResult> ReadDocument(string databaseName, string collectionName, string id)
        {
            try
            {
                var s = await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, id), new RequestOptions { PartitionKey = new PartitionKey(id) });
                return new OkObjectResult(s.ToString());
            }
            catch (DocumentClientException de)
            {
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    return new NotFoundObjectResult("Not found");
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
