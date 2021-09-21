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
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "todo")] HttpRequest req,
           [CosmosDB(
                databaseName: "TodoDb",
                collectionName: "TodoItems",
                ConnectionStringSetting = "CosmosDbConnectionString",
                SqlQuery = "SELECT * FROM c order by c._ts desc")]
                IEnumerable<Todo> toDoItems,
                ILogger log)
        {
        
            log.LogInformation("C# HTTP trigger function processed a request.");
            foreach (Todo toDoItem in toDoItems)
            {
                log.LogInformation(toDoItem.Description);
            }
            return new OkObjectResult(toDoItems);
        }
    }
}
