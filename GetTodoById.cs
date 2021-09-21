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
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "todo/{id}")] HttpRequest req,
            [CosmosDB(
                databaseName: "TodoDb",
                collectionName: "TodoItems",
                ConnectionStringSetting = "CosmosDbConnectionString",
                PartitionKey = "{id}", // Used for querying the table
                Id = "{id}")] Todo todoItem, // Used for binding to the class
            ILogger log)
        {
            if (todoItem == null)
            {
                log.LogError($"Todo item not found");
                return new NotFoundObjectResult("Todo item not found");
            }
            return new OkObjectResult(todoItem);
        }
    }
}
