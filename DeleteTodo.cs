using System.Reflection.Metadata;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using assignment.Models;
using System.Collections.Generic;
using Microsoft.Azure.Documents.Client;
using System.Configuration;

namespace ServerLessDatabase
{
    public static class DeleteTodo
    {
        private static DocumentClient client;
        private static readonly string ENDPOINT = Environment.GetEnvironmentVariable("AccountEndpoint", EnvironmentVariableTarget.Process);
        private static readonly string KEY = Environment.GetEnvironmentVariable("AccountKey", EnvironmentVariableTarget.Process);

        [FunctionName("DeleteTodo")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "todo/{id}")] HttpRequest req,
            [CosmosDB(
                databaseName: "TodoDb",
                collectionName: "TodoItems",
                PartitionKey = "{id}",
                Id = "{id}",
                ConnectionStringSetting = "CosmosDbConnectionString")] Todo todo,
            ILogger log)
        {
            try
            {
                client = new DocumentClient(new Uri(ENDPOINT), KEY);
                await client.DeleteDocumentAsync(UriFactory.CreateDocumentUri("TodoDb", "TodoItems", todo.Id), new RequestOptions { PartitionKey = new Microsoft.Azure.Documents.PartitionKey(todo.Id) });
                return new OkObjectResult("Deleted " + todo.Title);
            }
            catch
            {
                return new NotFoundObjectResult("Item does not exist.");
            }
        }
    }
}
