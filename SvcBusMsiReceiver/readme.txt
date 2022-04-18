
1. Install 
   a. Azure.Messaging.ServiceBus 
   b. Microsoft.Azure.WebJobs.Extensions.ServiceBus Version="5.2.0" or higher 

2. Assign the "Azure Service Bus Data Receiver" role using IAM

3. if  Connection = "demo" then add a key to local.settings.json 
      
      "demo__fullyQualifiedNamespace": "{namespace}.servicebus.windows.net",

      ServiceBusTrigger("queueName", Connection = "demo") 
      <CONNECTION_NAME_PREFIX> = demo (whatever is specified as the connection property in the trigger attribute)
 
 local.settings.json    
 {
    "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "demo__fullyQualifiedNamespace": "{namespace}.servicebus.windows.net",
    "AZURE_TENANT_ID": ""   //if there are multiple tenants 
  }
}

4. Add Configuration with to app settings inside the configuration section of the function app

 "demo__fullyQualifiedNamespace": "{namespace}.servicebus.windows.net",

5. For user assigned msi
   demo__credential="managedidentity"
   demo__clientId="user assigned msi's client id"
