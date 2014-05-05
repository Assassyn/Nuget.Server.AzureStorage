//-----------------------------------------------------------------------
// <copyright file="PackageMetadata.cs" company="Aranea It Ltd">
//     Copyright (c) Aranea It Ltd. All rights reserved.
// </copyright>
// <author>Szymon M Sasin</author>
//-----------------------------------------------------------------------

namespace Nuget.Server.AzureStorage
{
    using NuGet;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Implementation of package metadata.
    /// </summary>
    internal sealed class PackageMetadata : IPackageMetadata
    {
        /// <summary>
        /// Gets or sets the authors.
        /// </summary>
        /// <value>
        /// The authors.
        /// </value>
        public IEnumerable<string> Authors
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the copyright.
        /// </summary>
        /// <value>
        /// The copyright.
        /// </value>
        public string Copyright
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the dependency sets.
        /// </summary>
        /// <value>
        /// The dependency sets.
        /// </value>
        public IEnumerable<PackageDependencySet> DependencySets
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the framework assemblies.
        /// </summary>
        /// <value>
        /// The framework assemblies.
        /// </value>
        public IEnumerable<FrameworkAssemblyReference> FrameworkAssemblies
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the icon URL.
        /// </summary>
        /// <value>
        /// The icon URL.
        /// </value>
        public Uri IconUrl
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        public string Language
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the license URL.
        /// </summary>
        /// <value>
        /// The license URL.
        /// </value>
        public Uri LicenseUrl
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the minimum client version.
        /// </summary>
        /// <value>
        /// The minimum client version.
        /// </value>
        public Version MinClientVersion
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the owners.
        /// </summary>
        /// <value>
        /// The owners.
        /// </value>
        public IEnumerable<string> Owners
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the package assembly references.
        /// </summary>
        /// <value>
        /// The package assembly references.
        /// </value>
        public ICollection<PackageReferenceSet> PackageAssemblyReferences
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the project URL.
        /// </summary>
        /// <value>
        /// The project URL.
        /// </value>
        public Uri ProjectUrl
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the release notes.
        /// </summary>
        /// <value>
        /// The release notes.
        /// </value>
        public string ReleaseNotes
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [require license acceptance].
        /// </summary>
        /// <value>
        /// <c>true</c> if [require license acceptance]; otherwise, <c>false</c>.
        /// </value>
        public bool RequireLicenseAcceptance
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the summary.
        /// </summary>
        /// <value>
        /// The summary.
        /// </value>
        public string Summary
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        public string Tags
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public SemanticVersion Version
        {
            get;
            set;
        }


        public bool DevelopmentDependency
        {
            get;
            set;
        }
    }
}