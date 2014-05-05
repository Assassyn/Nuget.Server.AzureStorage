namespace Nuget.Server.AzureStorage
{
    using AutoMapper;
    using Nuget.Server.AzureStorage.Services;
    using NuGet;
    using NuGet.Server.Infrastructure;

    public static class Bootstraper
    {
        public static void SetUp()
        {
            NinjectBootstrapper.Kernel.Rebind<IServerPackageRepository>().To<AzureServerPackageRepository>();
            NinjectBootstrapper.Kernel.Bind<IPackageLocator>().To<AzurePackageLocator>();
        }
    }
}
