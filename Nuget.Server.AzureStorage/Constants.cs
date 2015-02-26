namespace Nuget.Server.AzureStorage
{
    /// <summary>
    /// Constants for the Azure properties metadata
    /// </summary>
    public static class Constants
    {
        public const string Created = "Created";
        public const string LatestModificationDate = "LastModified";
        public const string LastUploadedVersion = "LastVersion";
        public const string LastAccessed = "LastAccessed";
        public const string Package = "Package";
        public const string PackageId = "PackageId";
    }

    public static class PkgConsts
    {
        public const string Id = "Id";
        public const string Version = "Version";
        public const string Title = "Title";
        public const string Authors = "Authors";
        public const string Owners = "Owners";
        public const string IconUrl = "IconUrl";
        public const string LicenseUrl = "LicenseUrl";
        public const string ProjectUrl = "ProjectUrl";
        public const string DevelopmentDependency = "DevelopmentDependency";
        public const string RequireLicenseAcceptance = "RequireLicenseAcceptance";
        public const string Description = "Description";
        public const string Summary = "Summary";
        public const string ReleaseNotes = "ReleaseNotes";
        public const string Tags = "Tags";
        public const string Dependencies = "Dependencies";
        public const string IsAbsoluteLatestVersion = "IsAbsoluteLatestVersion";
        public const string IsLatestVersion = "IsLatestVersion";
        public const string MinClientVersion = "MinClientVersion";
        public const string Listed = "Listed";
        public const string Published = "Published";
        public const string Language = "Language";
        public const string Copyright = "Copyright";
        public const string ReportAbuseUrl = "ReportAbuseUrl";
        public const string DownloadCount = "DownloadCount";
        public const string FrameworkAssemblies = "FrameworkAssemblies";
        public const string PackageAssemblyReferences = "PackageAssemblyReferences";
        public const string AssemblyReferences = "AssemblyReferences";
    }

    public static class NsasConstants
    {
        public const string ContainerPrefix = "nugetazure-";
    }
}