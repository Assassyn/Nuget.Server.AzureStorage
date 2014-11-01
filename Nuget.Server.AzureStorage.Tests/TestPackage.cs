using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using NuGet;
using NUnit.Framework;

namespace NugetServer.AzureStorage.Tests
{
    public class TestPackage : IPackage
    {
        public string Id { get; set; }
        public SemanticVersion Version { get; set; }
        public string Title { get; set; }
        public IEnumerable<string> Authors { get; set; }
        public IEnumerable<string> Owners { get; set; }
        public Uri IconUrl { get; set; }
        public Uri LicenseUrl { get; set; }
        public Uri ProjectUrl { get; set; }
        public bool RequireLicenseAcceptance { get; set; }
        public bool DevelopmentDependency { get; set; }
        public string Description { get; set; }
        public string Summary { get;  set; }
        public string ReleaseNotes { get; set; }
        public string Language { get; set; }
        public string Tags { get; set; }
        public string Copyright { get; set; }
        public IEnumerable<FrameworkAssemblyReference> FrameworkAssemblies { get; set; }
        public ICollection<PackageReferenceSet> PackageAssemblyReferences { get; set; }
        public IEnumerable<PackageDependencySet> DependencySets { get; set; }
        public Version MinClientVersion { get; set; }
        public Uri ReportAbuseUrl { get; set; }
        public int DownloadCount { get; set; }
        public IEnumerable<IPackageFile> GetFiles()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FrameworkName> GetSupportedFrameworks()
        {
            throw new NotImplementedException();
        }

        public Stream GetStream()
        {
            return TestStream;
        }
        public Stream TestStream { get; set; }

        public bool IsAbsoluteLatestVersion { get; set; }
        public bool IsLatestVersion { get; set; }
        public bool Listed { get; set; }
        public DateTimeOffset? Published { get; set; }
        public IEnumerable<IPackageAssemblyReference> AssemblyReferences { get; set; }

        public static TestPackage MakePackage(FileStream fileStream)
        {
            return new TestPackage()
            {
                Id = "id_1234",
                Version = new SemanticVersion("1.0.0"),
                Title = "Title",
                Authors = new List<string>(){"auth_1","auth_b"},
                Owners = new List<string>{"ownera","ownerb"},
                IconUrl = new Uri("http://www.google.com"),
                LicenseUrl = new Uri("http://www.licurl.com"),
                ProjectUrl = new Uri("http://www.projurl.com"),
                RequireLicenseAcceptance = true,
                DevelopmentDependency = true,
                Description = "Description",
                Summary = "Summary",
                ReleaseNotes = "ReleaseNotes",
                Language = "Language",
                Tags = "Tags",
                Copyright = "Copyright",
                FrameworkAssemblies = new List<FrameworkAssemblyReference>(),
                PackageAssemblyReferences = new Collection<PackageReferenceSet>(),
                DependencySets = new List<PackageDependencySet>(),
                MinClientVersion = new Version("1.2.3"),
                ReportAbuseUrl = new Uri("http://abuseurl.com"),
                DownloadCount = 12345432,
                IsAbsoluteLatestVersion = true,
                IsLatestVersion = true,
                Listed = true,
                Published = new DateTimeOffset(2001,1,1,1,1,1,1,new TimeSpan(0)),
                AssemblyReferences = new List<IPackageAssemblyReference>(),
                TestStream = fileStream
            };
        }
    }
}
