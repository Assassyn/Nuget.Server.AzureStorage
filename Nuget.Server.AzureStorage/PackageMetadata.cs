using System;
using System.Collections.Generic;
using NuGet;

namespace Nuget.Server.AzureStorage
{
    public class PackageMetadata : IPackageMetadata
    {
        public IEnumerable<string> Authors { get; set; }
        public string Copyright { get; set; }
        public IEnumerable<PackageDependencySet> DependencySets { get; set; }
        public string Description { get; set; }
        public IEnumerable<FrameworkAssemblyReference> FrameworkAssemblies { get; set; }
        public Uri IconUrl { get; set; }
        public string Id { get; set; }
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
        public SemanticVersion Version { get; set; }
        public bool DevelopmentDependency { get; set; }
    }
}