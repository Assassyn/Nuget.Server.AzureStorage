using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using NuGet;

namespace Nuget.Server.AzureStorage.Doman.Entities
{
    public class AzurePackageReferenceSet
    {
        public IEnumerable<string> References { get; set; }
        public string FrameWorkName { get; set; }

        public AzurePackageReferenceSet(){}
        public AzurePackageReferenceSet(PackageReferenceSet referenceSet)
        {
            References = referenceSet.References.ToList();
            FrameWorkName = referenceSet.TargetFramework.ToString();
        }

        public PackageReferenceSet GetReferenceSet()
        {
            return new PackageReferenceSet(new FrameworkName(FrameWorkName),References);
        }

    }
}
