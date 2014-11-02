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
        
        public IEnumerable<FrameworkName> SupportedFrameworks { get; set; }
        public Stream GetStream()
        {
            throw new NotImplementedException();
        }

        public string Path { get; set; }
        public string EffectivePath { get; set; }
        public FrameworkName TargetFramework { get; set; }
        public string Name { get; set; }
    }

    public class AzureDtoAssemblyReference
    {
        public AzureDtoAssemblyReference() { }
        public AzureDtoAssemblyReference(IPackageAssemblyReference assemblyReference)
        {
            Name = assemblyReference.Name;
            TargetFrameworkString = assemblyReference.TargetFramework.ToString();
            EffectivePath = assemblyReference.EffectivePath;
            PathStr = assemblyReference.Path;
            SupportedFrameworkStrs = assemblyReference.SupportedFrameworks.Select(x => x.ToString());
        }
        public AzureAssemblyReference GetAzureAssemblyReference()
        {
            return new AzureAssemblyReference()
            {
                Name = Name,
                TargetFramework = new FrameworkName(TargetFrameworkString),
                EffectivePath = EffectivePath,
                Path = PathStr,
                SupportedFrameworks = SupportedFrameworkStrs.Select(x=>new FrameworkName(x)).ToList()
            };
        }
        public IEnumerable<string> SupportedFrameworkStrs { get; set; }
        public string PathStr { get; set; }
        public string EffectivePath { get; set; }
        public string TargetFrameworkString { get; set; }
        public string Name { get; set; }
    }
}
