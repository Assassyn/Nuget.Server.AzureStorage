namespace Nuget.Server.AzureStorage.Doman.Entities
{
    using NuGet;
    using System;

    internal sealed class AzureVersionSpec
    {
        public AzureVersionSpec()
        {

        }

        public AzureVersionSpec(IVersionSpec version)
        {

            this.IsMaxInclusive = version.IsMaxInclusive;
            this.IsMinInclusive = version.IsMinInclusive;

            if (version.MaxVersion != null)
            {
                this.MaxVersionSpecial = version.MaxVersion.SpecialVersion;
                this.MaxVersionVersion = version.MaxVersion.Version.ToString();
            }

            if (version.MinVersion != null)
            {
                this.MinVersionSpecial = version.MinVersion.SpecialVersion;
                this.MinVersionVersion = version.MinVersion.Version.ToString();
            }
        }

        public bool IsMaxInclusive { get; set; }

        public bool IsMinInclusive { get; set; }

        public string MaxVersionSpecial { get; set; }
        public string MaxVersionVersion { get; set; }

        public string MinVersionSpecial { get; set; }
        public string MinVersionVersion { get; set; }

        public IVersionSpec ToVersionSpec()
        {
            var version = new VersionSpec
            {
                IsMaxInclusive = this.IsMaxInclusive,
                IsMinInclusive = this.IsMinInclusive,

            };

            if (this.IsMaxInclusive)
            {
                version.MaxVersion = new SemanticVersion(new Version(this.MaxVersionVersion), this.MaxVersionSpecial);
            }

            if (this.IsMinInclusive)
            {
                version.MinVersion = new SemanticVersion(new Version(this.MinVersionVersion), this.MinVersionSpecial);
            }

            return version;
        }
    }
}