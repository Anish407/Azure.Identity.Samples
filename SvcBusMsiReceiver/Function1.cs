using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace SvcBusMsiReceiver
{
    public  class Function1
    {
        [FunctionName("Function1")]
        public  void Run([ServiceBusTrigger("msidemo", Connection = "demo")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        }
    }
}
