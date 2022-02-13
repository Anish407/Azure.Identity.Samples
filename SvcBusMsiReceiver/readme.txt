
1. Install 
   a. Azure.Messaging.ServiceBus 
   b. Microsoft.Azure.WebJobs.Extensions.ServiceBus Version="5.2.0" or higher 

2. Assign the "Azure Service Bus Data Receiver" role using IAM

3. if  Connection = "demo" then add a key to local.settings.json
      "demo__fullyQualifiedNamespace": "{namespace}.servicebus.windows.net",

      ServiceBusTrigger("queueName", Connection = "key name in localsettings.json")

4. Add Configuration with to app settings in configuration section of the function app

 "demo__fullyQualifiedNamespace": "{namespace}.servicebus.windows.net",


