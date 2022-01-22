<h1> Create a Role </h1> 
<code>
$resourceGroupName='<myResourceGroup>'
$accountName='<myCosmosAccount>'
az cosmosdb sql role definition create --account-name $accountName --resource-group $resourceGroupName --body ./ReaderRole.json </code>
  
  Checkout : <a href='./ReaderRole.json'>ReaderRole.Json</a>
<p>
The DataActions arrays contains all the rights that we want to expose through this role. The readMetadata role is used by the SDK to read meta about the database 
for ex: the regions where it has been replicated etc. The other roles enable the user to read, execute sql queries and subscribe to the change feed.
</p>
<h1>List the created Role </h1> 
  <code>
  az cosmosdb sql role definition list --account-name $accountName --resource-group $resourceGroupName
  </code>
 from the output copy the id of the new role
  

  
