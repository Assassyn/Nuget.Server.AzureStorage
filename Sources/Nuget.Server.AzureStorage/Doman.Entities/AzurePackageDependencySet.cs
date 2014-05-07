//-----------------------------------------------------------------------
// <copyright file="AzurePackageDependencySet.cs" company="A-IT">
//     Copyright (c) A-IT. All rights reserved.
// </copyright>
// <author>Szymon M Sasin</author>
//-----------------------------------------------------------------------

namespace Nuget.Server.AzureStorage.Doman.Entities
{
    using Newtonsoft.Json;
    using NuGet;
    using System.Collections.Generic;
    using System.Runtime.Versioning;

    internal sealed class AzurePackageDependencySet
    {
        public ICollection<PackageDependency> Dependencies { get; set; }

        public IEnumerable<FrameworkName> SupportedFrameworks { get; set; }

        public FrameworkName TargetFramework { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(new SerializationInfo
            {
                TargetFramework = string.Format(
                    "{0}|{1}|{2}",
                    this.TargetFramework.Identifier,
                    this.TargetFramework.Version.ToString(),
                    this.TargetFramework.Profile)
            });
        }

        private class SerializationInfo
        {
            public string TargetFramework { get; set; }

            public IEnumerable<string> Dependencies { get; set; }
        }
    }
}