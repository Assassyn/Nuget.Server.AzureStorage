//-----------------------------------------------------------------------
// <copyright file="AzurePathResolver.cs" company="Aranea It Ltd">
//     Copyright (c) Aranea It Ltd. All rights reserved.
// </copyright>
// <author>Szymon M Sasin</author>
//-----------------------------------------------------------------------

namespace Nuget.Server.AzureStorage
{
    using NuGet;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <remarks>
    /// 
    /// </remarks>
    public sealed class AzurePathResolver : IPackagePathResolver
    {
        public string GetInstallPath(IPackage package)
        {
            return this.GetPackageDirectory(package);
        }

        public string GetPackageDirectory(string packageId, SemanticVersion version)
        {
            return packageId;
        }

        public string GetPackageDirectory(IPackage package)
        {
            return this.GetPackageDirectory(package.Id, package.Version);
        }

        public string GetPackageFileName(string packageId, SemanticVersion version)
        {
            return GetAzureFriendlyName(packageId) + "|" + version.ToString();
        }

        private static string GetAzureFriendlyName(string packageId)
        {
            return packageId.ToLower().Replace(".","-");
        }

        public string GetPackageFileName(IPackage package)
        {
            return this.GetPackageFileName(package.Id, package.Version);
        }
    }
}
