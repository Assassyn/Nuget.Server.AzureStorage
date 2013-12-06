using NuGet.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nuget.Server.AzureStorage
{
    public class ServerPackageRepository : IServerPackageRepository
    {
        public ServerPackageRepository()
        {

        }

        public NuGet.Server.DataServices.Package GetMetadataPackage(NuGet.IPackage package)
        {
            throw new NotImplementedException();
        }

        public void RemovePackage(string packageId, NuGet.SemanticVersion version)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<NuGet.IPackage> GetUpdates(IEnumerable<NuGet.IPackage> packages, bool includePrerelease, bool includeAllVersions, IEnumerable<System.Runtime.Versioning.FrameworkName> targetFrameworks, IEnumerable<NuGet.IVersionSpec> versionConstraints)
        {
            throw new NotImplementedException();
        }

        public IQueryable<NuGet.IPackage> Search(string searchTerm, IEnumerable<string> targetFrameworks, bool allowPrereleaseVersions)
        {
            throw new NotImplementedException();
        }

        public void AddPackage(NuGet.IPackage package)
        {
            throw new NotImplementedException();
        }

        public IQueryable<NuGet.IPackage> GetPackages()
        {
            throw new NotImplementedException();
        }

        public void RemovePackage(NuGet.IPackage package)
        {
            throw new NotImplementedException();
        }

        public string Source
        {
            get { throw new NotImplementedException(); }
        }

        public bool SupportsPrereleasePackages
        {
            get { throw new NotImplementedException(); }
        }
    }
}
