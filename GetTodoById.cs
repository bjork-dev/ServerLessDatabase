using assignment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ServerLessDatabase
{
    public static class GetTodoById
    {
        [FunctionName("GetTodoById")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "todo/{id}")] HttpRequest req,
            [CosmosDB(
                databaseName: "TodoDb",
                collectionName: "TodoItems",
                ConnectionStringSetting = "CosmosDbConnectionString",
                PartitionKey = "{id}", // Used for querying the table
                Id = "{id}")] Todo toDoItem, // Used for binding to the class
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            if (toDoItem == null)
            {
                log.LogInformation($"ToDo item not found");
            }
            else
            {
                log.LogInformation($"Found ToDo item, Description={toDoItem.Description}");
            }
            return new OkObjectResult(toDoItem);
        }
    }
}
