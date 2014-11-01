//-----------------------------------------------------------------------
// <copyright file="Package.cs" company="A-IT">
//     Copyright (c) A-IT. All rights reserved.
// </copyright>
// <author>Szymon M Sasin</author>
//-----------------------------------------------------------------------

namespace Nuget.Server.AzureStorage.Doman.Entities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Versioning;
    using Newtonsoft.Json;
    using NuGet;

    public sealed class AzurePackage : IPackage
    {
        public AzurePackage() {}

        public IEnumerable<IPackageAssemblyReference> AssemblyReferences { get; set; }
        public bool IsAbsoluteLatestVersion { get; set; }
        public bool IsLatestVersion { get; set; }
        public bool Listed { get; set; }
        public DateTimeOffset? Published { get; set; }
        public IEnumerable<string> Authors { get; set; }
        public string Copyright { get; set; }
        public IEnumerable<PackageDependencySet> DependencySets { get; set; }
        public string Description { get; set; }
        public bool DevelopmentDependency { get; set; }
        public IEnumerable<FrameworkAssemblyReference> FrameworkAssemblies { get; set; }
        public Uri IconUrl { get; set; }
        public string Language { get; set; }
        public Uri LicenseUrl { get; set; }
        public Version MinClientVersion { get; set; }
        public IEnumerable<string> Owners { get; set; }
        public ICollection<PackageReferenceSet> PackageAssemblyReferences { get; set; }
        public Uri ProjectUrl { get; set; }
        public string ReleaseNotes { get; set; }
        public bool RequireLicenseAcceptance { get; set; }
        public string Summary { get; set; }
        public string Tags { get; set; }
        public string Title { get; set; }
        public string Id { get; set; }
        public SemanticVersion Version { get; set; }
        public int DownloadCount { get; set; }
        public Uri ReportAbuseUrl { get; set; }

        public IEnumerable<IPackageFile> GetFiles()
        {
            throw new NotImplementedException();
        }

        public Stream GetStream()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FrameworkName> GetSupportedFrameworks()
        {
            throw new NotImplementedException();
        }
    }
}