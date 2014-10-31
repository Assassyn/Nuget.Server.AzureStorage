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
            var dependencySetContent = blob.Metadata["Dependencies"];
            dependencySetContent = this.Base64Decode(dependencySetContent);
            package.DependencySets = dependencySetContent
                .FromJson<IEnumerable<AzurePackageDependencySet>>()
                .Select(x => new PackageDependencySet(x.TargetFramework, x.Dependencies));
            package.IsAbsoluteLatestVersion = blob.Metadata[PkgConsts.IsAbsoluteLatestVersion].ToBool();
            package.IsLatestVersion = blob.Metadata[PkgConsts.IsLatestVersion].ToBool();
            if (blob.Metadata.ContainsKey("MinClientVersion"))
            {
                package.MinClientVersion = new Version(blob.Metadata["MinClientVersion"]);
            }
            package.Listed = blob.Metadata[PkgConsts.Listed].ToBool();

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
            if (!string.IsNullOrEmpty(package.ReleaseNotes))
            {
                blob.Metadata["ReleaseNotes"] = package.ReleaseNotes;
            }
            if (!string.IsNullOrEmpty(package.Tags))
            {
                blob.Metadata["Tags"] = package.Tags;
            }
            blob.Metadata["Dependencies"] = this.Base64Encode(package.DependencySets.Select(Mapper.Map<AzurePackageDependencySet>).ToJson());
            blob.Metadata[PkgConsts.IsAbsoluteLatestVersion] = package.IsAbsoluteLatestVersion.ToString();
            blob.Metadata[PkgConsts.IsLatestVersion] = package.IsLatestVersion.ToString();
            if (package.MinClientVersion != null)
            {
                blob.Metadata["MinClientVersion"] =  package.MinClientVersion.ToString();
            }

            blob.Metadata[PkgConsts.Listed] = package.Listed.ToString();

            blob.SetMetadata();
        }

        private string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        private string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}