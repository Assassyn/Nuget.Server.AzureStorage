using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Nuget.Server.AzureStorage")]
[assembly: AssemblyDescription("Azure Storage provider for the NuGet.Server")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Aranea IT")]
[assembly: AssemblyProduct("Nuget.Server.AzureStorage")]
[assembly: AssemblyCopyright("Copyright Aranea IT©  2013")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("07ad4bda-7b48-4f2f-b9cd-dab5328282cb")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("2.8.2.10")]
[assembly: AssemblyFileVersion("2.8.2.10")]

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(Nuget.Server.AzureStorage.Bootstraper), "SetUp")]