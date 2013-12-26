//-----------------------------------------------------------------------
// <copyright file="AzureServerPackageRepository.cs" company="Aranea It Ltd">
//     Copyright (c) Aranea It Ltd. All rights reserved.
// </copyright>
// <author>Szymon M Sasin</author>
//-----------------------------------------------------------------------

namespace Nuget.Server.AzureStorage
{
    using AutoMapper;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Auth;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Newtonsoft.Json;
    using NuGet;
    using NuGet.Server.Infrastructure;
    using ObjectSerialization;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Versioning;

    /// <summary>
    /// An class used to provide diffrent FileSystem to <see cref="ServerPackageRepository"/>
    /// </summary>
    public class AzureServerPackageRepository : ServerPackageRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServerPackageRepository"/> class.
        /// </summary>
        public AzureServerPackageRepository() : base(new AzurePathResolver(), new AzureFileSystem()) { }
    }
}