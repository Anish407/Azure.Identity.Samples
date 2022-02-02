// See https://aka.ms/new-console-template for more information
using Azure;
using Azure.Identity;
using Azure.Messaging.EventGrid;

Console.WriteLine("Sending");
string topicEndpoint = "https://azurerbac.northeurope-1.eventgrid.azure.net/api/events";

try
{
    EventGridPublisherClient client = new EventGridPublisherClient(
            new Uri(topicEndpoint),
            new DefaultAzureCredential());

    List<EventGridEvent> eventsList = new List<EventGridEvent>
            {
                new EventGridEvent(
                    "AADRbac",
                    "Anish.CustomEvent",
                    "1.0",
                    "This is the event data")
            };

    //Without Permissions:
    //Exception:
    //The principal associated with access token presented with the incoming request does not have permission to send data to 
    //Assign EventGrid Data Sender role to user/group/MI to make it work
    await client.SendEventsAsync(eventsList);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

Console.WriteLine("Done");

Console.ReadKey();
