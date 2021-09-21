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
using Microsoft.Azure.Documents.Client;

namespace ServerLessDatabase
{
    public static class EditTodo
    {
        private static DocumentClient client;
        private static readonly string _endpoint = Environment.GetEnvironmentVariable("AccountEndpoint", EnvironmentVariableTarget.Process);
        private static readonly string _key = Environment.GetEnvironmentVariable("AccountKey", EnvironmentVariableTarget.Process);

        [FunctionName("EditTodo")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "todo/{id}")] HttpRequest req,
             [CosmosDB(
                databaseName: "TodoDb",
                collectionName: "TodoItems",
                PartitionKey = "{id}",
                Id = "{id}",
                ConnectionStringSetting = "CosmosDbConnectionString")]
                Todo todo,
                ILogger log)
        {
            try
            {
                string id = todo.Id;
                var obj = new StreamReader(req.Body).ReadToEnd();
                todo = JsonConvert.DeserializeObject<Todo>(obj);
                todo.Id = id; // By assining here, we don't need to include ID in the request body.
                client = new DocumentClient(new Uri(_endpoint), _key);
                await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri("TodoDb", "TodoItems", todo.Id), todo, new RequestOptions { PartitionKey = new Microsoft.Azure.Documents.PartitionKey(todo.Id) });

                log.LogInformation($"C# HTTP trigger function inserted one row");

                return new OkObjectResult(todo);
            } catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }
    }
}
