Nuget.Server.AzureStorage
=========================
# Getting started

It is recommended that you just use the example project as the starting base of the new NuGet server.

# Configuration

all settings for the nuget server are in the web config file.  The most important setting is the StorageConnectionString setting.

To configure just add the AppSetting entry like:
<pre>
&lt;add key="StorageConnectionString" value="DefaultEndpointsProtocol=https;AccountName=<account name>;AccountKey=<account key>" /&gt;
</pre>

# Dev SetUp
1) go download chocolatey
2) use chocolatey to install the Nuget Command Line interface. this will allow you to use powershell to manually upload and download nuget files for testing

