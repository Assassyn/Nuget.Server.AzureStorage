namespace Nuget.Server.AzureStorage.Doman.Entities
{
    using NuGet;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Versioning;

    /// <summary>
    /// Serializable implementation of PackageAssemblyReference interface.
    /// </summary>
    internal sealed class AzurePackageAssemblyReference : IPackageAssemblyReference
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the effective path.
        /// </summary>
        /// <value>
        /// The effective path.
        /// </value>
        public string EffectivePath { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the target framework.
        /// </summary>
        /// <value>
        /// The target framework.
        /// </value>
        public FrameworkName TargetFramework { get; set; }

        /// <summary>
        /// Gets or sets the supported frameworks.
        /// </summary>
        /// <value>
        /// The supported frameworks.
        /// </value>
        public IEnumerable<FrameworkName> SupportedFrameworks { get; set; }

        /// <summary>
        /// Gets the stream.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Stream GetStream()
        {
            throw new NotImplementedException();
        }
    }
}