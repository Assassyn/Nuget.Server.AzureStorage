using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;

namespace NugetServer.AzureStorage.Tests
{
    public static class AzureStorageHelpers
    {
        public static void DeleteAllBlobContainers(this CloudStorageAccount storageAccount, string prefix = null)
        {
            var blobClient = storageAccount.CreateCloudBlobClient();
            
            var containers = blobClient.ListContainers(prefix ?? "");
            foreach (var container in containers)
            {
                container.DeleteIfExists();
            }
        }
    }
}
