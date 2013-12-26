//-----------------------------------------------------------------------
// <copyright file="PackageAssemblyReference.cs" company="Aranea It Ltd">
//     Copyright (c) Aranea It Ltd. All rights reserved.
// </copyright>
// <author>Szymon M Sasin</author>
//-----------------------------------------------------------------------

namespace Nuget.Server.AzureStorage
{
    using NuGet;
    using System;
    using System.Collections.Generic;

    /// <remarks>
    ///
    /// </remarks>
    public sealed class PackageAssemblyReference : IPackageAssemblyReference
    {
        public string Name
        {
            get;
            set;
        }

        public string EffectivePath
        {
            get;
            set;
        }

        public System.IO.Stream GetStream()
        {
            throw new NotImplementedException();
        }

        public string Path
        {
            get;
            set;
        }

        public System.Runtime.Versioning.FrameworkName TargetFramework
        {
            get;
            set;
        }

        public IEnumerable<System.Runtime.Versioning.FrameworkName> SupportedFrameworks
        {
            get;
            set;
        }
    }
}