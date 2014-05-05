//-----------------------------------------------------------------------
// <copyright file="AzureServerPackageRepository.cs" company="Aranea It Ltd">
//     Copyright (c) Aranea It Ltd. All rights reserved.
// </copyright>
// <author>Szymon M Sasin</author>
//-----------------------------------------------------------------------

namespace Nuget.Server.AzureStorage
{
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Nuget.Server.AzureStorage.Domain.Services;
    using Nuget.Server.AzureStorage.Doman.Entities;
    using NuGet;
    using NuGet.Server.DataServices;
    using NuGet.Server.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Versioning;

    /// <summary>
    /// An class used to provide diffrent FileSystem to <see cref="ServerPackageRepository"/>
    /// </summary>
    public class AzureServerPackageRepository : IServerPackageRepository
    {
        private readonly CloudStorageAccount storageAccount;
        private readonly CloudBlobClient blobClient;
        private readonly IPackageLocator packageLocator;

        public PackageSaveModes PackageSaveMode { get; set; }

        public AzureServerPackageRepository(IPackageLocator packageLocator)
        {
            this.packageLocator = packageLocator;
            var azureConnectionString = CloudConfigurationManager.GetSetting("StorageConnectionString");
            this.storageAccount = CloudStorageAccount.Parse(azureConnectionString);
            this.blobClient = this.storageAccount.CreateCloudBlobClient();
        }

        public Package GetMetadataPackage(IPackage package)
        {
            return new Package(package, new DerivedPackageData());
        }

        public IEnumerable<IPackage> GetUpdates(
            IEnumerable<IPackageName> packages,
            bool includePrerelease,
            bool includeAllVersions,
            IEnumerable<FrameworkName> targetFrameworks,
            IEnumerable<IVersionSpec> versionConstraints)
        {
            throw new NotImplementedException();
        }

        public IQueryable<IPackage> Search(string searchTerm, IEnumerable<string> targetFrameworks, bool allowPrereleaseVersions)
        {
            throw new NotImplementedException();
        }

        public void AddPackage(IPackage package)
        {
            var name = this.packageLocator.GetContainerName(package);
            var container = this.blobClient.GetContainerReference(name);

            container.CreateIfNotExists();

            this.UpdateMetadata(package, container);

            var blobName = this.packageLocator.GetItemName(package);
            var blob = container.GetBlockBlobReference(blobName);
            blob.UploadFromStream(package.GetStream());
            blob.Metadata[AzurePropertiesConstants.LatestModificationDate] = DateTimeOffset.Now.ToString();
            blob.SetMetadata();
        }

        public IQueryable<IPackage> GetPackages()
        {
            return this.blobClient
                .ListContainers()
                .Select(x => new AzurePackage(x))
                .AsQueryable<IPackage>();
        }

        public void RemovePackage(IPackage package)
        {
            var name = this.packageLocator.GetContainerName(package);
            var container = this.blobClient.GetContainerReference(name);

            if (container.Exists())
            {
                var blobName = this.packageLocator.GetItemName(package);
                var blob = container.GetBlockBlobReference(blobName);
                blob.DeleteIfExists();
            }
        }

        public void RemovePackage(string packageId, SemanticVersion version)
        {
            this.RemovePackage(new AzurePackage
            {
                Id = packageId,
                Version = version,
            });
        }

        public string Source
        {
            get
            {
                return "/";
            }
        }

        public bool SupportsPrereleasePackages
        {
            get
            {
                return false;
            }
        }

        private void UpdateMetadata(IPackage package, CloudBlobContainer container)
        {
            container.Metadata[AzurePropertiesConstants.LatestModificationDate] = DateTimeOffset.Now.ToString();
            if (!container.Exists())
            {
                container.Metadata[AzurePropertiesConstants.Created] = DateTimeOffset.Now.ToString();
            }
            container.Metadata[AzurePropertiesConstants.LastUploadedVersion] = package.Version.ToString();
            container.SetMetadata();
        }
    }
}