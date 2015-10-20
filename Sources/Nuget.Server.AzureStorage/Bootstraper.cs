namespace Nuget.Server.AzureStorage
{
    using AutoMapper;
    using Nuget.Server.AzureStorage.Domain.Services;
    using Nuget.Server.AzureStorage.Doman.Entities;
    using NuGet;
    using NuGet.Server;
    using NuGet.Server.Infrastructure;

    public static class Bootstraper
    {
        public static void SetUp()
        {
            NinjectBootstrapper.Kernel.Rebind<IServerPackageRepository>().To<AzureServerPackageRepository>();
            NinjectBootstrapper.Kernel.Rebind<IPackageService>().To<AzurePackageService>();
            NinjectBootstrapper.Kernel.Bind<IPackageLocator>().To<AzurePackageLocator>();
            NinjectBootstrapper.Kernel.Bind<IAzurePackageSerializer>().To<AzurePackageSerializer>();

            SetUpMapper();
        }

        public static void SetUpMapper()
        {
            Mapper.CreateMap<IPackage, AzurePackage>();
            Mapper.CreateMap<PackageDependencySet, AzurePackageDependencySet>()
                .ForMember(x => x.SeriazlizableDependencies, opt => opt.Ignore())
                .ForMember(x => x.SeriazlizableSupportedFrameworks, opt => opt.Ignore())
                .ForMember(x => x.SeriazlizableTargetFramework, opt => opt.Ignore());
        }
    }
}
