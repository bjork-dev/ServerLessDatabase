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
using System.Collections.Generic;

namespace assignment
{
    public static class GetTodo
    {
        private static DocumentClient client;

        [FunctionName("GetTodo")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "todo")] HttpRequest req,
            [CosmosDB(
                databaseName: "TodoDb",
                collectionName: "TodoItems",
                ConnectionStringSetting = "CosmosDbConnectionString")]
                IEnumerable<Todo> todoItems,
                ILogger log)
        {
            if(todoItems == null)
                return new NotFoundObjectResult("No entries found.");
            return new OkObjectResult(todoItems);
        }
    }
}
