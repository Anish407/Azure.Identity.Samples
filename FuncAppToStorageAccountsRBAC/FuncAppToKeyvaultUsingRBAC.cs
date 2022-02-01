using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace FuncAppToStorageAccountsRBAC
{
    public  class FuncAppToKeyvaultUsingRBAC
    {
        public FuncAppToKeyvaultUsingRBAC(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        [FunctionName("FuncAppToKeyvaultUsingRBAC")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public  async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            //Assign keyvault reader role and grant appropriate RBAC roles (keyvault reader)
            var client = new SecretClient(new Uri("https://funcappkvdemo.vault.azure.net/"), new DefaultAzureCredential(new DefaultAzureCredentialOptions()
            {
                ManagedIdentityClientId = "MSI's Id"
            }));

            var sample = Environment.GetEnvironmentVariable("Sample");
            var sample2 = Configuration["Sample"];

            var Name = client.GetSecret("name").Value.Value;
            var Age = client.GetSecret("Age").Value.Value;

            return new OkObjectResult(sample);
        }
    }
}

