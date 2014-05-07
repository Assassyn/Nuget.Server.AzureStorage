//-----------------------------------------------------------------------
// <copyright file="AzurePackageSerializer.cs" company="A-IT">
//     Copyright (c) A-IT. All rights reserved.
// </copyright>
// <author>Szymon M Sasin</author>
//-----------------------------------------------------------------------

namespace Nuget.Server.AzureStorage.Domain.Services
{
    using AutoMapper;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Nuget.Server.AzureStorage.Doman.Entities;
    using NuGet;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal sealed class AzurePackageSerializer : IAzurePackageSerializer
    {
        public AzurePackage ReadFromMetadata(CloudBlockBlob blob)
        {
            blob.FetchAttributes();
            var package = new AzurePackage();

            package.Id = blob.Metadata["Id"];
            package.Version = new SemanticVersion(blob.Metadata["Version"]);
            if (blob.Metadata.ContainsKey("Title"))
            {
                package.Title = blob.Metadata["Title"];
            }
            if (blob.Metadata.ContainsKey("Authors"))
            {
                package.Authors = blob.Metadata["Authors"].Split(',');
            }
            if (blob.Metadata.ContainsKey("Owners"))
            {
                package.Owners = blob.Metadata["Owners"].Split(',');
            }
            if (blob.Metadata.ContainsKey("IconUrl"))
            {
                package.IconUrl = new Uri(blob.Metadata["IconUrl"]);
            }
            if (blob.Metadata.ContainsKey("LicenseUrl"))
            {
                package.LicenseUrl = new Uri(blob.Metadata["LicenseUrl"]);
            }
            if (blob.Metadata.ContainsKey("ProjectUrl"))
            {
                package.ProjectUrl = new Uri(blob.Metadata["ProjectUrl"]);
            }
            package.RequireLicenseAcceptance = blob.Metadata["RequireLicenseAcceptance"].ToBool();
            package.DevelopmentDependency = blob.Metadata["DevelopmentDependency"].ToBool();
            if (blob.Metadata.ContainsKey("Description"))
            {
                package.Description = blob.Metadata["Description"];
            }
            if (blob.Metadata.ContainsKey("Summary"))
            {
                package.Summary = blob.Metadata["Summary"];
            }
            if (blob.Metadata.ContainsKey("ReleaseNotes"))
            {
                package.ReleaseNotes = blob.Metadata["ReleaseNotes"];
            }
            if (blob.Metadata.ContainsKey("Tags"))
            {
                package.Tags = blob.Metadata["Tags"];
            }
            package.DependencySets = blob.Metadata["Dependencies"]
                .FromJson<IEnumerable<AzurePackageDependencySet>>()
                .Select(x => new PackageDependencySet(x.TargetFramework, x.Dependencies));
            package.IsAbsoluteLatestVersion = blob.Metadata["IsAbsoluteLatestVersion"].ToBool();
            package.IsLatestVersion = blob.Metadata["IsLatestVersion"].ToBool();
            if (blob.Metadata.ContainsKey("MinClientVersion"))
            {
                package.MinClientVersion = new Version(blob.Metadata["MinClientVersion"]);
            }
            package.Listed = blob.Metadata["Listed"].ToBool();

            return package;
        }

        public void SaveToMetadata(AzurePackage package, CloudBlockBlob blob)
        {
            blob.Metadata["Id"] = package.Id;
            blob.Metadata["Version"] = package.Version.ToString();
            if (!string.IsNullOrEmpty(package.Title))
            {
                blob.Metadata["Title"] = package.Title;
            }
            blob.Metadata["Authors"] = string.Join(",", package.Authors);
            blob.Metadata["Owners"] = string.Join(",", package.Owners);
            if (package.IconUrl != null)
            {
                blob.Metadata["IconUrl"] = package.IconUrl.AbsoluteUri;
            }
            if (package.LicenseUrl != null)
            {
                blob.Metadata["LicenseUrl"] = package.LicenseUrl.AbsoluteUri;
            }
            if (package.ProjectUrl != null)
            {
                blob.Metadata["ProjectUrl"] = package.ProjectUrl.AbsoluteUri;
            }
            blob.Metadata["RequireLicenseAcceptance"] = package.RequireLicenseAcceptance.ToString();
            blob.Metadata["DevelopmentDependency"] = package.DevelopmentDependency.ToString();
            blob.Metadata["Description"] = package.Description;
            if (!string.IsNullOrEmpty(package.Summary))
            {
                blob.Metadata["Summary"] = package.Summary;
            }
            blob.Metadata["ReleaseNotes"] = package.ReleaseNotes;
            if (!string.IsNullOrEmpty(package.Tags))
            {
                blob.Metadata["Tags"] = package.Tags;
            }
            blob.Metadata["Dependencies"] = package.DependencySets.Select(Mapper.Map<AzurePackageDependencySet>).ToJson();
            blob.Metadata["IsAbsoluteLatestVersion"] = package.IsAbsoluteLatestVersion.ToString();
            blob.Metadata["IsLatestVersion"] = package.IsLatestVersion.ToString();
            blob.Metadata["MinClientVersion"] = package.MinClientVersion.ToString();
            blob.Metadata["Listed"] = package.Listed.ToString();

            blob.SetMetadata();
        }
    }
}