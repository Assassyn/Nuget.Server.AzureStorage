namespace Nuget.Server.AzureStorage
{
    /// <summary>
    /// Constants for the Azure properties metadata
    /// </summary>
    internal sealed class AzurePropertiesConstants
    {
        /// <summary>
        /// The created
        /// </summary>
        public const string Created = "Creted";

        /// <summary>
        /// The latest modification date
        /// </summary>
        public const string LatestModificationDate = "LastModified";

        /// <summary>
        /// The last uploaded version
        /// </summary>
        public const string LastUploadedVersion = "LastVersion";

        /// <summary>
        /// The last accessed
        /// </summary>
        public const string LastAccessed = "LastAccessed";

        public const string Package = "Package";

        ///// <summary>
        ///// Subset of Constants for Package Metadata.
        ///// </summary>
        //public sealed class Package
        //{
        //    /// <summary>
        //    /// The identifier
        //    /// </summary>
        //    public const string Id = "Package.Id";

        //    /// <summary>
        //    /// The version
        //    /// </summary>
        //    public const string Version = "Package.Version";
        //}

        public const string PackageId = "PackageId";
    }
}