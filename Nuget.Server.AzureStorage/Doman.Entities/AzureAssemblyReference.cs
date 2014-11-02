using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using NuGet;

namespace Nuget.Server.AzureStorage.Doman.Entities
{
    public class AzureAssemblyReference : IPackageAssemblyReference
    {
        
        public IEnumerable<FrameworkName> SupportedFrameworks { get; private set; }
        public Stream GetStream()
        {
            throw new NotImplementedException();
        }

        public string Path { get; private set; }
        public string EffectivePath { get; private set; }
        public FrameworkName TargetFramework { get; private set; }
        public string Name { get; private set; }
    }
}
