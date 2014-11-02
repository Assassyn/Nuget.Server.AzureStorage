using System.Collections.ObjectModel;

namespace Nuget.Server.AzureStorage.Domain.Services
{
    using AutoMapper;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Nuget.Server.AzureStorage.Doman.Entities;
    using NuGet;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class AzurePackageSerializer : IAzurePackageSerializer
    {
        public AzurePackage ReadFromMetadata(CloudBlockBlob blob)
        {
            blob.FetchAttributes();
            var package = new AzurePackage();

            package.Id = blob.Metadata[PkgConsts.Id];
            package.Version = new SemanticVersion(blob.Metadata[PkgConsts.Version]);
            if (blob.Metadata.ContainsKey(PkgConsts.Title))
            {
                package.Title = blob.Metadata[PkgConsts.Title];
            }
            if (blob.Metadata.ContainsKey(PkgConsts.Authors))
            {
                package.Authors = blob.Metadata[PkgConsts.Authors].Split(',');
            }
            if (blob.Metadata.ContainsKey(PkgConsts.Owners))
            {
                package.Owners = blob.Metadata[PkgConsts.Owners].Split(',');
            }
            if (blob.Metadata.ContainsKey(PkgConsts.IconUrl))
            {
                package.IconUrl = new Uri(blob.Metadata[PkgConsts.IconUrl]);
            }
            if (blob.Metadata.ContainsKey(PkgConsts.LicenseUrl))
            {
                package.LicenseUrl = new Uri(blob.Metadata[PkgConsts.LicenseUrl]);
            }
            if (blob.Metadata.ContainsKey(PkgConsts.ProjectUrl))
            {
                package.ProjectUrl = new Uri(blob.Metadata[PkgConsts.ProjectUrl]);
            }
            if (blob.Metadata.ContainsKey(PkgConsts.Language))
            {
                package.Language = blob.Metadata[PkgConsts.Language];
            }
            if (blob.Metadata.ContainsKey(PkgConsts.Copyright))
            {
                package.Copyright = blob.Metadata[PkgConsts.Copyright];
            }
            if (blob.Metadata.ContainsKey(PkgConsts.ReportAbuseUrl))
            {
                package.ReportAbuseUrl = new Uri(blob.Metadata[PkgConsts.ReportAbuseUrl]);
            }
            package.DownloadCount = Convert.ToInt32(blob.Metadata[PkgConsts.DownloadCount]);
            package.RequireLicenseAcceptance = blob.Metadata[PkgConsts.RequireLicenseAcceptance].ToBool();
            package.DevelopmentDependency = blob.Metadata[PkgConsts.DevelopmentDependency].ToBool();
            if (blob.Metadata.ContainsKey(PkgConsts.Description))
            {
                package.Description = blob.Metadata[PkgConsts.Description];
            }
            if (blob.Metadata.ContainsKey(PkgConsts.Summary))
            {
                package.Summary = blob.Metadata[PkgConsts.Summary];
            }
            if (blob.Metadata.ContainsKey(PkgConsts.ReleaseNotes))
            {
                package.ReleaseNotes = blob.Metadata[PkgConsts.ReleaseNotes];
            }
            if (blob.Metadata.ContainsKey(PkgConsts.Tags))
            {
                package.Tags = blob.Metadata[PkgConsts.Tags];
            }
            var dependencySetContent = blob.Metadata[PkgConsts.Dependencies];
            dependencySetContent = Base64Decode(dependencySetContent);
            package.DependencySets = dependencySetContent
                .FromJson<IEnumerable<AzurePackageDependencySet>>()
                .Select(x => new PackageDependencySet(x.TargetFramework, x.Dependencies));

            package.FrameworkAssemblies = blob.Metadata[PkgConsts.FrameworkAssemblies].FromJson<IEnumerable<FrameworkAssemblyReference>>();
            package.PackageAssemblyReferences = blob.Metadata[PkgConsts.PackageAssemblyReferences].FromJson<Collection<PackageReferenceSet>>();

            package.IsAbsoluteLatestVersion = blob.Metadata[PkgConsts.IsAbsoluteLatestVersion].ToBool();
            package.IsLatestVersion = blob.Metadata[PkgConsts.IsLatestVersion].ToBool();
            if (blob.Metadata.ContainsKey(PkgConsts.MinClientVersion))
            {
                package.MinClientVersion = new Version(blob.Metadata[PkgConsts.MinClientVersion]);
            }
            if (blob.Metadata.ContainsKey(PkgConsts.Published))
            {
                package.Published = DateTimeOffset.Parse(blob.Metadata[PkgConsts.Published]);
            }
            package.Listed = blob.Metadata[PkgConsts.Listed].ToBool();

            return package;
        }

        public void SaveToMetadata(AzurePackage package, CloudBlockBlob blob)
        {
            blob.Metadata[PkgConsts.Id] = package.Id;
            blob.Metadata[PkgConsts.Version] = package.Version.ToString();
            if (!string.IsNullOrEmpty(package.Title))
            {
                blob.Metadata[PkgConsts.Title] = package.Title;
            }
            blob.Metadata[PkgConsts.Authors] = string.Join(",", package.Authors);
            blob.Metadata[PkgConsts.Owners] = string.Join(",", package.Owners);
            if (package.IconUrl != null)
            {
                blob.Metadata[PkgConsts.IconUrl] = package.IconUrl.AbsoluteUri;
            }
            if (package.LicenseUrl != null)
            {
                blob.Metadata[PkgConsts.LicenseUrl] = package.LicenseUrl.AbsoluteUri;
            }
            if (package.ProjectUrl != null)
            {
                blob.Metadata[PkgConsts.ProjectUrl] = package.ProjectUrl.AbsoluteUri;
            }
            blob.Metadata[PkgConsts.RequireLicenseAcceptance] = package.RequireLicenseAcceptance.ToString();
            blob.Metadata[PkgConsts.DevelopmentDependency] = package.DevelopmentDependency.ToString();
            blob.Metadata[PkgConsts.Description] = package.Description;
            if (!string.IsNullOrEmpty(package.Summary))
            {
                blob.Metadata[PkgConsts.Summary] = package.Summary;
            }
            if (!string.IsNullOrEmpty(package.ReleaseNotes))
            {
                blob.Metadata[PkgConsts.ReleaseNotes] = package.ReleaseNotes;
            }
            if (!string.IsNullOrEmpty(package.Tags))
            {
                blob.Metadata[PkgConsts.Tags] = package.Tags;
            }
            if (!string.IsNullOrEmpty(package.Language))
            {
                blob.Metadata[PkgConsts.Language] = package.Language;
            }
            if (!string.IsNullOrEmpty(package.Copyright))
            {
                blob.Metadata[PkgConsts.Copyright] = package.Copyright;
            }
            if (package.ReportAbuseUrl != null)
            {
                blob.Metadata[PkgConsts.ReportAbuseUrl] = package.ReportAbuseUrl.ToString();
            }
            blob.Metadata[PkgConsts.DownloadCount] = package.DownloadCount.ToString();

            blob.Metadata[PkgConsts.FrameworkAssemblies] = package.FrameworkAssemblies.ToJson();
            blob.Metadata[PkgConsts.PackageAssemblyReferences] = package.PackageAssemblyReferences.ToJson();
            blob.Metadata[PkgConsts.Dependencies] = this.Base64Encode(package.DependencySets.Select(Mapper.Map<AzurePackageDependencySet>).ToJson());

            blob.Metadata[PkgConsts.IsAbsoluteLatestVersion] = package.IsAbsoluteLatestVersion.ToString();
            blob.Metadata[PkgConsts.IsLatestVersion] = package.IsLatestVersion.ToString();
            if (package.MinClientVersion != null)
            {
                blob.Metadata[PkgConsts.MinClientVersion] =  package.MinClientVersion.ToString();
            }
            if (package.Published != null)
            {
                blob.Metadata[PkgConsts.Published] = package.Published.Value.ToString("yyyy-MM-ddTHH:mm:ssZ");
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