//-----------------------------------------------------------------------
// <copyright file="AzureServerPackageRepository.cs" company="Aranea It Ltd">
//     Copyright (c) Aranea It Ltd. All rights reserved.
// </copyright>
// <author>Szymon M Sasin</author>
//-----------------------------------------------------------------------

namespace Nuget.Server.AzureStorage
{
    using AutoMapper;
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
    /// An class used to provide diffrent FileSystem to <see cref="ServerPackageRepository" />
    /// </summary>
    public class AzureServerPackageRepository : IServerPackageRepository
    {
        private readonly CloudStorageAccount storageAccount;
        private readonly CloudBlobClient blobClient;
        private readonly IPackageLocator packageLocator;
        private readonly IAzurePackageSerializer packageSerializer;

        public PackageSaveModes PackageSaveMode { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServerPackageRepository"/> class.
        /// </summary>
        /// <param name="packageLocator">The package locator.</param>
        /// <param name="packageSerializer">The package serializer.</param>
        public AzureServerPackageRepository(IPackageLocator packageLocator, IAzurePackageSerializer packageSerializer)
        {
            this.packageLocator = packageLocator;
            this.packageSerializer = packageSerializer;
            var azureConnectionString = CloudConfigurationManager.GetSetting("StorageConnectionString");
            this.storageAccount = CloudStorageAccount.Parse(azureConnectionString);
            this.blobClient = this.storageAccount.CreateCloudBlobClient();
        }
        public AzureServerPackageRepository(IPackageLocator packageLocator, 
                                            IAzurePackageSerializer packageSerializer,
                                            CloudStorageAccount storageAccount)
        {
            this.packageLocator = packageLocator;
            this.packageSerializer = packageSerializer;

            this.storageAccount = storageAccount;
            this.blobClient = this.storageAccount.CreateCloudBlobClient();
        }

        /// <summary>
        /// Gets the metadata package.
        /// </summary>
        /// <param name="package">The package.</param>
        /// <returns></returns>
        public Package GetMetadataPackage(IPackage package)
        {
            //var pkg = new Package(package, new DerivedPackageData());
            return new Package(package, new DerivedPackageData());
        }

        /// <summary>
        /// Gets the updates.
        /// </summary>
        /// <param name="packages">The packages.</param>
        /// <param name="includePrerelease">if set to <c>true</c> [include prerelease].</param>
        /// <param name="includeAllVersions">if set to <c>true</c> [include all versions].</param>
        /// <param name="targetFrameworks">The target frameworks.</param>
        /// <param name="versionConstraints">The version constraints.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IEnumerable<IPackage> GetUpdates(
            IEnumerable<IPackageName> packages,
            bool includePrerelease,
            bool includeAllVersions,
            IEnumerable<FrameworkName> targetFrameworks,
            IEnumerable<IVersionSpec> versionConstraints)
        {
            return this.GetUpdatesCore(packages, includePrerelease, includeAllVersions, targetFrameworks, versionConstraints);
        }

        /// <summary>
        /// Searches the specified search term.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="targetFrameworks">The target frameworks.</param>
        /// <param name="allowPrereleaseVersions">if set to <c>true</c> [allow prerelease versions].</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IQueryable<IPackage> Search(string searchTerm, IEnumerable<string> targetFrameworks, bool allowPrereleaseVersions)
        {
            var packages = this.GetPackages().ToList();
            return 
                this.GetPackages()
                .Find(searchTerm)
                .FilterByPrerelease(allowPrereleaseVersions)
                .Where(p => p.Listed)
                .AsQueryable<IPackage>();
        }

        /// <summary>
        /// Adds the package.
        /// </summary>
        /// <param name="package">The package.</param>
        public void AddPackage(IPackage package)
        {
            var name = this.packageLocator.GetContainerName(package);
            var container = this.blobClient.GetContainerReference(name);

            var exists = !container.CreateIfNotExists();

            this.UpdateContainerMetadata(package, container, exists);

            var blobName = package.Version.ToString();

            // this.packageLocator.GetItemName(package);
            var blob = container.GetBlockBlobReference(blobName);
            blob.UploadFromStream(package.GetStream());
            this.UpdateBlobMetadata(package, blob);
        }

        /// <summary>
        /// Gets the packages.
        /// </summary>
        /// <returns></returns>
        public IQueryable<IPackage> GetPackages()
        {
            return this.blobClient
                .ListContainers(NsasConstants.ContainerPrefix)
                .Select(x => this.packageSerializer.ReadFromMetadata(this.GetLatestBlob(x)))
                .AsQueryable<IPackage>();
        }

        /// <summary>
        /// Removes the package.
        /// </summary>
        /// <param name="package">The package.</param>
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

        /// <summary>
        /// Removes the package.
        /// </summary>
        /// <param name="packageId">The package identifier.</param>
        /// <param name="version">The version.</param>
        public void RemovePackage(string packageId, SemanticVersion version)
        {
            this.RemovePackage(new AzurePackage
            {
                Id = packageId,
                Version = version,
            });
        }

        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        public string Source
        {
            get
            {
                return "/";
            }
        }

        /// <summary>
        /// Gets a value indicating whether [supports prerelease packages].
        /// </summary>
        /// <value>
        /// <c>true</c> if [supports prerelease packages]; otherwise, <c>false</c>.
        /// </value>
        public bool SupportsPrereleasePackages
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Updates the container metadata.
        /// </summary>
        /// <param name="package">The package.</param>
        /// <param name="container">The container.</param>
        /// <param name="exists">if set to <c>true</c> [exists].</param>
        private void UpdateContainerMetadata(IPackage package, CloudBlobContainer container, bool exists)
        {
            container.Metadata[Constants.LatestModificationDate] = DateTimeOffset.Now.ToString();
            if (!exists)
            {
                container.Metadata[Constants.Created] = DateTimeOffset.Now.ToString();
            }
            container.Metadata[Constants.LastUploadedVersion] = package.Version.ToString();
            container.Metadata[Constants.PackageId] = package.Id;
            container.SetMetadata();
        }

        /// <summary>
        /// Updates the BLOB metadata.
        /// </summary>
        /// <param name="package">The package.</param>
        /// <param name="blob">The BLOB.</param>
        private void UpdateBlobMetadata(IPackage package, CloudBlockBlob blob)
        {
            blob.Metadata[Constants.LatestModificationDate] = DateTimeOffset.Now.ToString();
            var azurePackage = Mapper.Map<AzurePackage>(package);
            //blob.Metadata[AzurePropertiesConstants.Package] = JsonConvert.SerializeObject(azurePackage);
            this.packageSerializer.SaveToMetadata(azurePackage, blob);
            blob.SetMetadata();
        }

        /// <summary>
        /// Gets the latest BLOB.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns></returns>
        private CloudBlockBlob GetLatestBlob(CloudBlobContainer container)
        {
            container.FetchAttributes();
            var latest = container.Metadata[Constants.LastUploadedVersion];

            return container.GetBlockBlobReference(latest);
        }
    }
}