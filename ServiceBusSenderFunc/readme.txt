---------------------------------------------------  Sender ---------------------------------------------------------------------------------------

1. Install Azure.Messaging.ServiceBus

2. Install Azure.Identity

2. Created a user assigned managed and assigned it "Azure Service Bus Data Sender"  role

 ServiceBusClient svcClient = new ServiceBusClient("{namespace}.servicebus.windows.net", 
                new DefaultAzureCredential(
                       new DefaultAzureCredentialOptions 
                       { 
                            ManagedIdentityClientId= "Client id of the User assigned MSI"
                       }));
-----------------------------------------------------------------------------------------------------------------------------------------------------