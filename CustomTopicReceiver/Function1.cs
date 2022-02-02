using Azure.Messaging.EventGrid;
using Azure.Messaging.EventGrid.SystemEvents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Net;
using System.Threading.Tasks;

namespace CustomTopicReceiver
{
    public class Function1
    {
       // private bool EventTypeSubcriptionValidation
       // => HttpContext.Request.Headers["aeg-event-type"].FirstOrDefault() ==
       //"SubscriptionValidation";

        [FunctionName("Function1")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string response = string.Empty;
           // string requestContent = "[{  \"id\": \"2d1781af-3a4c-4d7c-bd0c-e34b19da4e66\",  \"topic\": \"/subscriptions/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx\",  \"subject\": \"mySubject\",  \"data\": {    \"validationCode\": \"512d38b6-c7b8-40c8-89fe-f46f9e9622b6\",    \"validationUrl\": \"https://rp-eastus2.eventgrid.azure.net:553/eventsubscriptions/estest/validate?id=B2E34264-7D71-453A-B5FB-B62D0FDC85EE&t=2018-04-26T20:30:54.4538837Z&apiVersion=2018-05-01-preview&token=1BNqCxBBSSE9OnNSfZM4%2b5H9zDegKMY6uJ%2fO2DFRkwQ%3d\"  },  \"eventType\": \"Microsoft.EventGrid.SubscriptionValidationEvent\",  \"eventTime\": \"2018-01-25T22:12:19.4556811Z\",  \"metadataVersion\": \"1\",  \"dataVersion\": \"1\"}]";

            BinaryData events = await BinaryData.FromStreamAsync(req.Body);
           // BinaryData events = new BinaryData(requestContent);
            log.LogInformation($"Received events: {events}");
            try
            {

                EventGridEvent[] eventGridEvents = EventGridEvent.ParseMany(events);

                foreach (EventGridEvent eventGridEvent in eventGridEvents)
                {
                    log.LogInformation($"inside foreach : {eventGridEvent.Id}");
                    // Handle system events
                    if (eventGridEvent.TryGetSystemEventData(out object eventData))
                    {
                        // Handle the subscription validation event 
                        if (eventData is SubscriptionValidationEventData subscriptionValidationEventData)
                        {
                            log.LogInformation($"Got SubscriptionValidation event data, validation code: {subscriptionValidationEventData.ValidationCode}, topic: {eventGridEvent.Topic}");
                            // Do any additional validation (as required) and then return back the below response

                            var responseData = new SubscriptionValidationResponse()
                            {
                                ValidationResponse = subscriptionValidationEventData.ValidationCode
                            };
                            return new OkObjectResult(responseData);
                        }
                    }
                    // Handle the custom contoso event
                    else if (eventGridEvent.EventType == "Anish.CustomEvent")
                    {
                        var contosoEventData = eventGridEvent.Data.ToObjectFromJson<string>();
                        log.LogInformation($"Event Received {contosoEventData}");
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogInformation($"Exception: {ex.Message}");
            }
            return new OkObjectResult(response);
        }
    }
}

