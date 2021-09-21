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

namespace ServerLessDatabase
{
    public static class DeleteTodo
    {
        public static readonly List<Todo> todos = new List<Todo>();
        [FunctionName("DeleteTodo")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "todo/{id}")] HttpRequest req,
            [CosmosDB(
                databaseName: "TodoDb",
                collectionName: "TodoItems",
                PartitionKey = "{id}",
                Id = "{id}",
                ConnectionStringSetting = "CosmosDbConnectionString")] Todo todoItem,
            ILogger log)
        {
            return new OkObjectResult("Deleted");
        }
    }
}
