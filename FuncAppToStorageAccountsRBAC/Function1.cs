using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace FuncAppToStorageAccountsRBAC
{
    public class Function1
    {
        [FunctionName("Function1")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            try
            {
                await WriteToBlobStorageUsingRBACs();

                WriteToTableStorage();

            }
            catch (Exception ex)
            {

                return new BadRequestObjectResult(ex.Message);
            }

            return new OkObjectResult("");
        }

        private static void WriteToTableStorage()
        {
            string tableUri = "https://rbacdemostorage.table.core.windows.net/rbactable";
            string tableName = "rbactable";
            TableServiceClient tableServiceClient = new TableServiceClient(new Uri(tableUri), new DefaultAzureCredential());
            TableClient tableClient = tableServiceClient.GetTableClient(tableName);
            TableEntity entity = new TableEntity("anish", Guid.NewGuid().ToString())
                {
                    { "name", "Anish" },
                };
            tableClient.AddEntity(entity);
        }

        private static async Task WriteToBlobStorageUsingRBACs()
        {
            string containerName = "rbaccontainer";
            string blobEndpoint = "https://rbacdemostorage.blob.core.windows.net/";
            Uri blobUrl = new Uri(blobEndpoint);

            BlobServiceClient blobServiceClient = new BlobServiceClient(blobUrl, new DefaultAzureCredential());
            BlobContainerClient container = blobServiceClient.GetBlobContainerClient(containerName);

            BlobContentInfo result = await container.UploadBlobAsync($"anishrbacDemo-{Guid.NewGuid().ToString()}",
                new MemoryStream(Encoding.UTF8.GetBytes("Anish Rbac Demo")));
        }
    }

}

