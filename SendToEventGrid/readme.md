<h1>Publish to Event Grid using RBAC<h1>
<ul>
  <li>Assign the role Event Grid Data Sender to a user/group/application.</li>  
  <li> 
    Initialize the Event grid client using Azure.Identity </br>
    <code>
        EventGridPublisherClient client = new EventGridPublisherClient(
            new Uri(topicEndpoint),
            new DefaultAzureCredential());
    </code>
</ul>

