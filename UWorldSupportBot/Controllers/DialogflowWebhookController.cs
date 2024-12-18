﻿using Google.Cloud.Dialogflow.V2;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace UWorldSupportBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DialogflowWebhookController : ControllerBase
    {
        [HttpPost]
        public IActionResult HookRequest([FromBody] JObject request)
        {
            // Log the request for debugging purposes (you can remove this in production)

            // Initialize the response object
            var response = new WebhookResponse();

            var outputContexts = request.SelectToken("queryResult.outputContexts");
            if (outputContexts != null) {
                foreach (var outputContext in outputContexts) { 
                    var contextName = outputContext["name"]?.ToString();
                }
            }

            // Get the parameters from the request (name, age, position)
            //var parameters = request.QueryResult.Parameters.Fields;

            //// Extract the 'name', 'age', and 'position' parameters
            //string name = parameters.ContainsKey("name") ? parameters["name"].ToString() : "unknown";
            //string age = parameters.ContainsKey("age") ? parameters["age"].ToString() : "unknown";
            //string position = parameters.ContainsKey("position") ? parameters["position"].ToString() : "unknown";

            //// Create a dynamic response with all the details
            //var fulfillmentText = $"Thanks for the details! Here’s what I have:\n" +
            //                      $"Name: {name}\n" +
            //                      $"Age: {age}\n" +
            //                      $"Position: {position}";

            //// Set the response fulfillment text
            //response.FulfillmentText = fulfillmentText;

            // Optionally, set additional fields like output contexts if needed
            // response.OutputContexts = new List<Context> { new Context { Name = "context_name", LifespanCount = 5 } };

            // Return the response to Dialogflow
            return Ok(200);
        }
    }

    // Dialogflow Webhook Request Model
    public class WebhookRequest
    {
        public dynamic QueryResult { get; set; }
        public string ResponseId { get; set; }
    }

    // Dialogflow Webhook Response Model
    public class WebhookResponse
    {
        public string FulfillmentText { get; set; }
        public List<Context> OutputContexts { get; set; } = new List<Context>();
    }

    // Context class (Optional, you can use it if you're passing output contexts)
    public class Context
    {
        public string Name { get; set; }
        public int LifespanCount { get; set; }
    }
}
