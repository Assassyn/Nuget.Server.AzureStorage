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
                var package = TestPackage.MakePackage(fileStream);
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
                var package = TestPackage.MakePackage(fileStream);
                _azureServerPackageRepository.AddPackage(package);

                var packages = _azureServerPackageRepository.GetPackages();
                Assert.AreEqual(1, packages.Count());

                var reloadedPackage = packages.Single();
                Assert.AreEqual(package.Id,reloadedPackage.Id);
                Assert.AreEqual(package.Version.ToString(), reloadedPackage.Version.ToString());
                Assert.AreEqual(package.Title,reloadedPackage.Title);
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
                //Assert.True(package.FrameworkAssemblies.All(x => reloadedPackage.FrameworkAssemblies.Contains(x)));
                Assert.AreEqual(package.Published.ToString(), reloadedPackage.Published.ToString());
                Assert.AreEqual(package.Id, reloadedPackage.Id);
            }
            
        }
    }
}
