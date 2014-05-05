//-----------------------------------------------------------------------
// <copyright file="PackageLocator.cs" company="A-IT">
//     Copyright (c) A-IT. All rights reserved.
// </copyright>
// <author>Szymon M Sasin</author>
//-----------------------------------------------------------------------

namespace Nuget.Server.AzureStorage.Services
{
    using NuGet;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    internal sealed class AzurePackageLocator : IPackageLocator
    {
        public string GetContainerName(IPackage package)
        {
            return this.GetAzureFriendlyString(package.Id);
        }

        public string GetItemName(IPackage package)
        {
            return this.GetAzureFriendlyString(package.Version.ToString());
        }

        private string GetAzureFriendlyString(string packageId)
        {
            return packageId.ToLower().Replace(".", "-");
        }
    }
}
