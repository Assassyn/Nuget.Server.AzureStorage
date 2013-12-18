//-----------------------------------------------------------------------
// <copyright file="AzureFileSystem.cs" company="Aranea It Ltd">
//     Copyright (c) Aranea It Ltd. All rights reserved.
// </copyright>
// <author>Szymon M Sasin</author>
//-----------------------------------------------------------------------

namespace Nuget.Server.AzureStorage
{
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using NuGet;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <remarks>
    /// 
    /// </remarks>
    internal sealed class AzureFileSystem : IFileSystem
    {
        private CloudStorageAccount storageAccount;
        private CloudBlobClient blobClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureFileSystem"/> class.
        /// </summary>
        public AzureFileSystem()
        {
            var azureConnectionString = CloudConfigurationManager.GetSetting("StorageConnectionString");
            this.storageAccount = CloudStorageAccount.Parse(azureConnectionString);
            this.blobClient = this.storageAccount.CreateCloudBlobClient();
        }

        /// <summary>
        /// Adds the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="writeToStream">The write to stream.</param>
        public void AddFile(string path, Action<System.IO.Stream> writeToStream)
        {
            Contract.Requires(writeToStream != null);

            this.AddFileCore(path, writeToStream);
        }

        /// <summary>
        /// Adds the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="stream">The stream.</param>
        public void AddFile(string path, System.IO.Stream stream)
        {
            Contract.Requires(stream != null, "Stream could not be null");

            this.AddFileCore(path, x => stream.CopyTo(x));
        }

        private void AddFileCore(string path, Action<Stream> writeToStream)
        {
            var packageName =  this.GetPackageName(path);
            var container = this.blobClient.GetContainerReference(packageName);
            container.CreateIfNotExists();

            var packageVersion =  this.GetPackageVerion(path);
            var blob = container.GetBlockBlobReference(packageVersion);
            using (var stream = new MemoryStream())
            {
                writeToStream(stream);
                stream.Position = 0;
                blob.UploadFromStream(stream);
            }
        }

        private string GetPackageVerion(string path)
        {
            return path.Split('|')[1];
        }

        private string GetPackageName(string path)
        {
            return path.Split('|')[0];
        }

        public System.IO.Stream CreateFile(string path)
        {
            throw new NotImplementedException();
        }

        public void DeleteDirectory(string path, bool recursive)
        {
            throw new NotImplementedException();
        }

        public void DeleteFile(string path)
        {
            throw new NotImplementedException();
        }

        public bool DirectoryExists(string path)
        {
            throw new NotImplementedException();
        }

        public bool FileExists(string path)
        {
            throw new NotImplementedException();
        }

        public DateTimeOffset GetCreated(string path)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetDirectories(string path)
        {
            return blobClient.ListContainers().Select(x => x.Name);
        }

        public IEnumerable<string> GetFiles(string path, string filter, bool recursive)
        {
            var container = this.blobClient.GetContainerReference(path);
            return container.ListBlobs().Select(x=>x.Container.Name);
        }

        public string GetFullPath(string path)
        {
            throw new NotImplementedException();
        }

        public DateTimeOffset GetLastAccessed(string path)
        {
            throw new NotImplementedException();
        }

        public DateTimeOffset GetLastModified(string path)
        {
            var container = this.blobClient.GetContainerReference(path);
            return new DateTimeOffset();
        }

        public ILogger Logger
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void MakeFileWritable(string path)
        {
            throw new NotImplementedException();
        }

        public System.IO.Stream OpenFile(string path)
        {
            throw new NotImplementedException();
        }

        public string Root
        {
            get
            {
                return string.Empty;
            }
        }
    }
}
