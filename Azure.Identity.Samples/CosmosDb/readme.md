<h1> Create a Role </h1> 
<code>
$resourceGroupName='<myResourceGroup>'
  $accountName='<myCosmosAccount>' </br>
az cosmosdb sql role definition create --account-name $accountName --resource-group $resourceGroupName --body ./ReaderRole.json </code>
  
  Checkout : <a href='./ReaderRole.json'>ReaderRole.Json</a>
<p>
The DataActions arrays contains all the rights that we want to expose through this role. The readMetadata role is used by the SDK to read meta about the database 
for ex: the regions where it has been replicated , What is the consistency level, Info about all the physical partitions etc. The other roles enable the user to read, execute sql queries and subscribe to the change feed.
</p>
<h1>List the created Role </h1> 
  <code>
  az cosmosdb sql role definition list --account-name $accountName --resource-group $resourceGroupName
  </code>  </br>
 from the output copy the id of the new role to a vairable $reader


<h1>Assign the Reader Role To a users/application/ad group (in my case I have assigned it to my ObjectId) </h1>
<code>
az cosmosdb sql role assignment create --account-name 
$accountName --resource-group $resourceGroupName 
--scope "/dbs/Demo/colls/mycont1" 
--principal-id $principalId 
--role-definition-id $reader
</code>
<p> The role is scoped to the container Check "AssignableScopes": [ "/dbs/Demo/colls/mycont1" ] section in  <a href='./ReaderRole.json'>ReaderRole.Json</a>

/ -> (account-level), </br>
/dbs/{database-name}  -> (database-level), </br>
/dbs/{database-name}/colls/{container-name}  ->(container-level) </br>
</p>

<h1>Run the Application</h1>
<ul>
  <li> Install-Package Azure.Identity -Version 1.5.0 </li>
  <li> Install-Package Microsoft.Azure.Cosmos -Version 3.23.0 </li>
</ul>

<p> 
I have created a DataBase named "Demo" and a Container named "mycont1".
I have assigned the reader role to my id, But in the solution i try to read and then create an item (<a href="./ConnectToCosmosDb.cs">ConnectToCosmosDb.cs</a>) which will throw an exception. So to make it work just create a write role and assign it to an application/user/group where the solution will run.  
</p>
 
  

  
