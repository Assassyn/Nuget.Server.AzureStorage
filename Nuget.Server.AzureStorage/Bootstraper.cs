namespace Nuget.Server.AzureStorage
{
    using AutoMapper;
    using Nuget.Server.AzureStorage.Domain.Services;
    using Nuget.Server.AzureStorage.Doman.Entities;
    using NuGet;
    using NuGet.Server.Infrastructure;

    public static class Bootstraper
    {
        public static void SetUp()
        {
            NinjectBootstrapper.Kernel.Rebind<IServerPackageRepository>().To<AzureServerPackageRepository>();
            NinjectBootstrapper.Kernel.Bind<IPackageLocator>().To<AzurePackageLocator>();
            NinjectBootstrapper.Kernel.Bind<IAzurePackageSerializer>().To<AzurePackageSerializer>();

            SetUpMapper();
        }

        public static void SetUpMapper()
        {
            Mapper.CreateMap<IPackage, AzurePackage>();
            Mapper.CreateMap<PackageDependencySet, AzurePackageDependencySet>();
        }
    }
}
