namespace Nuget.Server.AzureStorage
{
    using AutoMapper;
    using NuGet;
    using NuGet.Server.Infrastructure;

    public static class Bootstraper
    {
        public static void SetUp()
        {
            NinjectBootstrapper.Kernel.Rebind<IServerPackageRepository>().ToConstant<IServerPackageRepository>(new AzureServerPackageRepository());
        }
    }
}
