Nuget.Server.AzureStorage
=========================

The taks of this project is to allow the storage of the nuget packages on the Azure Blob.

To make it working just include the package to the Nuget Server proejct.

<b>Configuration</b>

To configure just add the AppSetting entry like:
<pre>
&lt;add key="StorageConnectionString" value="DefaultEndpointsProtocol=https;AccountName=<account name>;AccountKey=<account key>" /&gt;
</pre>
