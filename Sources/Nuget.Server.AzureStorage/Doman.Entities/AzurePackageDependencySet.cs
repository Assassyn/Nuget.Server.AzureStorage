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
    using System.Linq;

    internal sealed class AzurePackageDependencySet
    {
        [JsonIgnore]
        public ICollection<PackageDependency> Dependencies
        {
            get
            {
                return (this.SeriazlizableDependencies == null ?
                    new PackageDependency[0] :
                    this.SeriazlizableDependencies.Select(SerializePackageDependency)).ToList();
            }
            set
            {
                this.SeriazlizableDependencies = value.Select(x => x.ToString());
            }
        }

        [JsonIgnore]
        public IEnumerable<FrameworkName> SupportedFrameworks
        {
            get
            {
                return this.SeriazlizableSupportedFrameworks == null ?
                    new FrameworkName[0] :
                    this.SeriazlizableSupportedFrameworks.Select(x => new FrameworkName(x));
            }
            set
            {
                this.SeriazlizableSupportedFrameworks = value.Select(x => x.FullName);
            }
        }

        [JsonIgnore]
        public FrameworkName TargetFramework
        {
            get
            {
                return string.IsNullOrWhiteSpace(this.SeriazlizableTargetFramework) ?
                    null :
                    new FrameworkName(this.SeriazlizableTargetFramework);
            }
            set
            {
                if (value != null)
                {
                    this.SeriazlizableTargetFramework = value.FullName;
                }
            }
        }

        public string SeriazlizableTargetFramework { get; set; }

        public IEnumerable<string> SeriazlizableDependencies { get; set; }

        public IEnumerable<string> SeriazlizableSupportedFrameworks { get; set; }

        private static PackageDependency SerializePackageDependency(string x)
        {
            var firstSpace = x.IndexOf(' ');
            var id = x.Substring(0, firstSpace);
            var version = x.Substring(firstSpace);
            if (string.IsNullOrWhiteSpace(version))
            {
                return new PackageDependency(id);
            }
            else
            {
                return new PackageDependency(id, VersionUtility.ParseVersionSpec(version));
            }
        }
    }
}