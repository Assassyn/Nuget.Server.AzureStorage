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

    /// <remarks>
    ///
    /// </remarks>
    internal sealed class AzureFileSystem : IFileSystem
    {
        private CloudStorageAccount storageAccount;
        private CloudBlobClient blobClient;
        private const string Created = "Creted";
        private const string LatestModificationDate = "LastModified";
        private const string LastUploadedVersion = "LastVersion";
        private const string LastAccessed = "LastAccessed";

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
            var packageName = this.GetPackageName(path);
            var container = this.blobClient.GetContainerReference(packageName);
            var existed = !container.Exists();
            container.CreateIfNotExists();

            var packageVersion = this.GetPackageVersion(path);

            container.Metadata[AzureFileSystem.LatestModificationDate] = DateTimeOffset.Now.ToString();
            if (existed)
            {
                container.Metadata[AzureFileSystem.Created] = DateTimeOffset.Now.ToString();
            }
            container.Metadata[AzureFileSystem.LastUploadedVersion] = packageVersion;
            container.SetMetadata();

            var blob = container.GetBlockBlobReference(packageVersion);
            using (var stream = new MemoryStream())
            {
                writeToStream(stream);
                stream.Position = 0;
                blob.UploadFromStream(stream);
            }

            blob.Metadata[AzureFileSystem.LatestModificationDate] = DateTimeOffset.Now.ToString();
            blob.SetMetadata();
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
            var project = this.GetPackageName(path);
            var version = this.GetPackageVersion(path);
            var container = this.blobClient.GetContainerReference(project);

            if (container.Exists())
            {
                var blob = container.GetBlockBlobReference(version);
                blob.Delete();
            }

            if (container.ListBlobs().Count() == 0)
            {
                container.Delete();
            }
        }

        public bool DirectoryExists(string path)
        {
            var container = this.blobClient.GetContainerReference(path);
            return container.Exists();
        }

        public bool FileExists(string path)
        {
            var exists = false;
            var container = this.blobClient.GetContainerReference(path);
            if (container.Exists())
            {
                container.FetchAttributes();
                var latestVersion = container.Metadata[AzureFileSystem.LastUploadedVersion];

                var blob = container.GetBlockBlobReference(latestVersion);

                exists = blob.Exists();
            }
            return exists;
        }

        public DateTimeOffset GetCreated(string path)
        {
            var container = this.blobClient.GetContainerReference(path);

            if (container.Exists())
            {
                return container.Properties.LastModified ?? DateTimeOffset.MinValue;
            }

            return DateTimeOffset.MinValue;
        }

        public IEnumerable<string> GetDirectories(string path)
        {
            return new string[0];
        }

        public IEnumerable<string> GetFiles(string path, string filter, bool recursive)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return blobClient.ListContainers().Select(x => x.Name);
            }
            var container = this.blobClient.GetContainerReference(path);
            return container.ListBlobs().Select(x => x.Container.Name);
        }

        public string GetFullPath(string path)
        {
            var container = this.blobClient.GetContainerReference(path);
            container.FetchAttributes();
            var latestVersion = container.Metadata[AzureFileSystem.LastUploadedVersion];

            return path + "|" + latestVersion;
        }

        public DateTimeOffset GetLastAccessed(string path)
        {
            var container = this.blobClient.GetContainerReference(path);

            if (container.Exists())
            {
                container.FetchAttributes();
                var timestamp = container.Metadata[AzureFileSystem.LastAccessed];
                return DateTimeOffset.Parse(timestamp);
            }

            return DateTimeOffset.MinValue;
        }

        public DateTimeOffset GetLastModified(string path)
        {
            var container = this.blobClient.GetContainerReference(path);

            if (container.Exists())
            {
                return container.Properties.LastModified ?? DateTimeOffset.MinValue;
            }

            return DateTimeOffset.MinValue;
        }

        public ILogger Logger { get; set; }

        public void MakeFileWritable(string path)
        {
        }

        public System.IO.Stream OpenFile(string path)
        {
            var container = this.blobClient.GetContainerReference(path);
            container.FetchAttributes();
            var latestVersion = container.Metadata[AzureFileSystem.LastUploadedVersion];

            var blob = container.GetBlockBlobReference(latestVersion);
            var stream = new MemoryStream();
            blob.DownloadToStream(stream);
            return stream;
        }

        public string Root
        {
            get
            {
                return string.Empty;
            }
        }

        private string GetPackageVersion(string path)
        {
            return path.Split('|')[1];
        }

        private string GetPackageName(string path)
        {
            return path.Split('|')[0];
        }

        private bool IsFilePath(string path)
        {
            return path.Contains('|');
        }

        private void UpdateAccessTimeStamp(CloudBlobContainer container)
        {
            container.Metadata[AzureFileSystem.LastAccessed] = DateTimeOffset.Now.ToString();
            container.SetMetadata();
        }
    }
}