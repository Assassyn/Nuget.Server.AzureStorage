using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.WindowsAzure.Storage;
using NuGet;
using Nuget.Server.AzureStorage;
using Nuget.Server.AzureStorage.Domain.Services;
using Nuget.Server.AzureStorage.Doman.Entities;
using NUnit.Framework;

namespace NugetServer.AzureStorage.Tests
{
    [TestFixture]
    public class AzureServerPackageRepositoryTests
    {
        
        private AzureServerPackageRepository _azureServerPackageRepository;
        private CloudStorageAccount _storageAccount;
        [SetUp]
        public void SetUp()
        {
            Bootstraper.SetUpMapper();

            Mapper.CreateMap<TestPackage, AzurePackage>();
            _storageAccount = CloudStorageAccount.Parse("UseDevelopmentStorage=true;");
            _storageAccount.DeleteAllBlobContainers(NsasConstants.ContainerPrefix);

            _azureServerPackageRepository = new AzureServerPackageRepository(
                new AzurePackageLocator(),
                new AzurePackageSerializer(),
                _storageAccount);
        }

        [Test]
        public void SanityCheck()
        {
            Assert.True(_azureServerPackageRepository != null);
        }

        [Test]
        public void MapperTest()
        {
            
            Mapper.AssertConfigurationIsValid();
        }

        [Test]
        public void AddPackage()
        {
            using (var fileStream = new FileStream("../../TestFiles/angularjs.1.2.25.nupkg", FileMode.Open))
            {
                var package = TestPackage.MakePackage("angular","1.2.26",fileStream);
                _azureServerPackageRepository.AddPackage(package);
                //Assert.AreEqual(0, packages.Count());
            }
        }
        [Test]
        public void GetPackages_empty()
        {
            var packages = _azureServerPackageRepository.GetPackages();
            Assert.AreEqual(0,packages.Count());
        }

        [Test]
        public void GetPackages_single()
        {
            using (var fileStream = new FileStream("../../TestFiles/angularjs.1.2.25.nupkg", FileMode.Open))
            {
                var package = TestPackage.MakePackage("angular","1.2.26",fileStream);
                _azureServerPackageRepository.AddPackage(package);

                var packages = _azureServerPackageRepository.GetPackages();
                Assert.AreEqual(1, packages.Count());

                var reloadedPackage = packages.Single();
                Assert.AreEqual(package.Id,reloadedPackage.Id);
                Assert.AreEqual(package.Version.ToString(), reloadedPackage.Version.ToString());
                Assert.AreEqual(package.Title,reloadedPackage.Title);
                Assert.AreEqual(package.Authors.Count(), reloadedPackage.Authors.Count());
                Assert.True(package.Authors.All(x=>reloadedPackage.Authors.Contains(x)));
                Assert.True(package.Owners.All(x => reloadedPackage.Owners.Contains(x)));
                Assert.AreEqual(package.RequireLicenseAcceptance, reloadedPackage.RequireLicenseAcceptance);
                Assert.AreEqual(package.DevelopmentDependency, reloadedPackage.DevelopmentDependency);
                Assert.AreEqual(package.Description, reloadedPackage.Description);
                Assert.AreEqual(package.Summary, reloadedPackage.Summary);
                Assert.AreEqual(package.Tags, reloadedPackage.Tags);
                Assert.AreEqual(package.ReleaseNotes, reloadedPackage.ReleaseNotes);
                Assert.AreEqual(package.Language, reloadedPackage.Language);
                Assert.AreEqual(package.Copyright, reloadedPackage.Copyright);
                Assert.AreEqual(package.FrameworkAssemblies.Count(), reloadedPackage.FrameworkAssemblies.Count());
                //Assert.True(package.FrameworkAssemblies.All(x => reloadedPackage.FrameworkAssemblies.Contains(x)));//TODO
                Assert.AreEqual(package.PackageAssemblyReferences.Count,reloadedPackage.PackageAssemblyReferences.Count);
                //Assert.True(package.PackageAssemblyReferences.All(x => reloadedPackage.PackageAssemblyReferences.Contains(x)));//TODO
                Assert.AreEqual(package.DependencySets.Count(),reloadedPackage.DependencySets.Count());
                //Assert.True(package.DependencySets.All(x => reloadedPackage.DependencySets.Contains(x)));
                Assert.AreEqual(package.Version.ToString(), reloadedPackage.Version.ToString());
                Assert.AreEqual(package.ReportAbuseUrl.ToString(), reloadedPackage.ReportAbuseUrl.ToString());
                Assert.AreEqual(package.Published.ToString(), reloadedPackage.Published.ToString());
                Assert.AreEqual(package.DownloadCount, reloadedPackage.DownloadCount);
                Assert.AreEqual(package.IsLatestVersion, reloadedPackage.IsLatestVersion);
                Assert.AreEqual(package.IsAbsoluteLatestVersion, reloadedPackage.IsAbsoluteLatestVersion);
                Assert.AreEqual(package.Listed, reloadedPackage.Listed);
                Assert.AreEqual(package.AssemblyReferences.Count(), reloadedPackage.AssemblyReferences.Count());
                //Assert.True(package.AssemblyReferences.All(x => reloadedPackage.AssemblyReferences.Contains(x)));//TODO
            }
        }

        [Test]
        public void GetPackages_TwoVersions()
        {
            using (var fileStream1225 = new FileStream("../../TestFiles/angularjs.1.2.25.nupkg", FileMode.Open))
            using (var fileStream1226 = new FileStream("../../TestFiles/angularjs.1.2.26.nupkg", FileMode.Open))
            {
                _azureServerPackageRepository.AddPackage(TestPackage.MakePackage("angular","1.2.25",fileStream1225));
                _azureServerPackageRepository.AddPackage(TestPackage.MakePackage("angular", "1.2.26", fileStream1226));

                var reloadedPackages = _azureServerPackageRepository.GetPackages().ToList();

                Assert.AreEqual(2, reloadedPackages.Count());

                var latestPackage = reloadedPackages.Single(x => x.IsLatestVersion);

                Assert.AreEqual("1.2.26",latestPackage.Version.ToString());
                Assert.True(latestPackage.IsAbsoluteLatestVersion);
                Assert.True(latestPackage.IsLatestVersion);
            }
        }
        
        [Test]
        public void Search_SinglePackage_EmptyString()
        {
            using (var fileStream = new FileStream("../../TestFiles/angularjs.1.2.25.nupkg", FileMode.Open))
            {
                var package = TestPackage.MakePackage("angular","1.2.26",fileStream);
                _azureServerPackageRepository.AddPackage(package);

                var searchedPackages = _azureServerPackageRepository.Search("", new List<string>(), true);
                Assert.AreEqual(1,searchedPackages.Count());
            }
        }

        [Test]
        public void Search_TwoVersions_OnePackage_EmptyString()
        {
            using (var fileStream1225 = new FileStream("../../TestFiles/angularjs.1.2.25.nupkg", FileMode.Open))
            using (var fileStream1226 = new FileStream("../../TestFiles/angularjs.1.2.26.nupkg", FileMode.Open))
            {
                _azureServerPackageRepository.AddPackage(TestPackage.MakePackage("angular", "1.2.25", fileStream1225));
                _azureServerPackageRepository.AddPackage(TestPackage.MakePackage("angular", "1.2.26", fileStream1226));

                var packages = _azureServerPackageRepository.Search("  ", new List<string>(), false)
                    .Where(x => x.IsLatestVersion)
                    .OrderBy(x => x.Id)
                    .Skip(0)
                    .Take(30);

                Assert.AreEqual(1, packages.Count());

            }
        }
    }
}
