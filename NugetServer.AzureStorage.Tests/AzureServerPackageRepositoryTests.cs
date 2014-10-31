using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using NuGet;
using Nuget.Server.AzureStorage;
using Nuget.Server.AzureStorage.Domain.Services;
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
            _storageAccount = CloudStorageAccount.Parse("UseDevelopmentStorage=true;");
            _storageAccount.DeleteAllBlobContainers(NsasConstants.ContainerPrefix);

            _azureServerPackageRepository = new AzureServerPackageRepository(
                new AzurePackageLocator(),
                new AzurePackageSerializer(),
                _storageAccount);
        }

        [Test]
        public void SantityCheck()
        {
            Assert.True(_azureServerPackageRepository != null);
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
    }
}
