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

namespace ServerLessDatabase
{
    public static class AddTodo
    {
        [FunctionName("AddTodo")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "todo")] HttpRequest req,
             [CosmosDB(
                databaseName: "TodoDb",
                collectionName: "TodoItems",
                ConnectionStringSetting = "CosmosDbConnectionString")]
                out Todo document,
                ILogger log)
        {
            try
            {
                var todo = new StreamReader(req.Body).ReadToEnd();
                document = JsonConvert.DeserializeObject<Todo>(todo);

                log.LogInformation($"C# HTTP trigger function inserted one row");

                return new OkObjectResult(document);
            } catch (Exception e)
            {
                document = null;
                return new BadRequestObjectResult(e.Message);
            }
        }
    }
}
