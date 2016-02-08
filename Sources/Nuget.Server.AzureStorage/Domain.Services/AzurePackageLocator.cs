namespace Nuget.Server.AzureStorage.Domain.Services
{
    using NuGet;

    public class AzurePackageLocator : IPackageLocator
    {
        public string GetContainerName(IPackage package)
        {
            return this.GetAzureFriendlyString(NsasConstants.ContainerPrefix+package.Id);
        }

        public string GetItemName(IPackage package)
        {
            return this.GetAzureFriendlyString(package.Version.ToString());
        }

        private string GetAzureFriendlyString(string packageId)
        {
            //return packageId.ToLower().Replace(".", "-").Replace("_","-");
            return packageId.ToLower().Replace("_", "-");
        }
    }
}