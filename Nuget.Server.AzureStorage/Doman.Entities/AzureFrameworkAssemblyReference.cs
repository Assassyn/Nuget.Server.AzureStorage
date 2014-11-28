using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using NuGet;

namespace Nuget.Server.AzureStorage.Doman.Entities
{
    public class AzureFrameworkAssemblyReference
    {
        public string AssemblyName { get; set; }
        public List<AzureFrameworkName> SupportedFrameworks { get; set; }

        public AzureFrameworkAssemblyReference(){}
        public AzureFrameworkAssemblyReference(FrameworkAssemblyReference reference)
        {
            AssemblyName = reference.AssemblyName;
            SupportedFrameworks = reference.SupportedFrameworks
                .Select(x=>new AzureFrameworkName(x)).ToList();
        }

        public FrameworkAssemblyReference GetFrameworkAssemblyReference()
        {
            return new FrameworkAssemblyReference(
                AssemblyName,
                SupportedFrameworks.Select(x=>x.GetFrameworkName()));
        }
    }

    public class AzureFrameworkName
    {
        public string Identifier { get; set; }
        public string FrameworkVersion { get; set; }
        public string Profile { get; set; }

        public AzureFrameworkName(){}
        public AzureFrameworkName(FrameworkName frameworkName)
        {
            Identifier = frameworkName.Identifier;
            FrameworkVersion = frameworkName.Version.ToString();
            Profile = frameworkName.Profile;
        }

       
        public FrameworkName GetFrameworkName()
        {
            return new FrameworkName(Identifier, new Version(FrameworkVersion),Profile);
        }

        
    }
}
