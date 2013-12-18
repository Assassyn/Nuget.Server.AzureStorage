namespace Nuget.Server.AzureStorage
{
    using NuGet.Server.Infrastructure;

    public static class Bootstraper
    {
        public static void SetUp()
        {
            NinjectBootstrapper.Kernel.Rebind<IServerPackageRepository>().To<AzureServerPackageRepository>();
        }
    }
}
