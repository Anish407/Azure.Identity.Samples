using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MsiBindings.Receiver
{
    public  class MsiBindings
    {
        private BlobServiceClient blobServiceClient;
        private BlobContainerClient container;

        public MsiBindings()
        {
            string containerName = "democont";
            string blobEndpoint = "https://anishstrgdemo.blob.core.windows.net/";
            Uri blobUrl = new Uri(blobEndpoint);

            blobServiceClient = new BlobServiceClient(blobUrl, new DefaultAzureCredential());
            container = blobServiceClient.GetBlobContainerClient(containerName);
        }

        [FunctionName("MsiBindings")]
        public  async Task Run(
            [ServiceBusTrigger("myqueue", Connection = "ServiceBus")] QueueMsg queueMsg,
            ILogger log)
        {
            await container.UploadBlobAsync($"{queueMsg.filename}.txt",
                new MemoryStream(Encoding.UTF8.GetBytes(queueMsg.data)));

            log.LogInformation($"C# ServiceBus queue trigger function processed message: {queueMsg.data}");
        }

        //send data in this format to svc bus..
//        {
//    "filename":"anish",
//    "data":"helloo anish"
//}

    public class QueueMsg
        {
            public string filename { get; set; }
            public string data { get; set; }
        }
    }
}
