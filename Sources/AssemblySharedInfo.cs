using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Retail")]
#endif
[assembly: AssemblyCompany( "Actis Systems" )]
[assembly: AssemblyProduct("Digillect� Common Libraries")]
[assembly: AssemblyCopyright("� 2011-2012 Actis Systems. All rights reserved.")]
[assembly: AssemblyTrademark("Digillect is a registered trademark of Actis Systems.")]

[assembly: AssemblyVersion(AssemblyInfo.Version)]
[assembly: AssemblyFileVersion(AssemblyInfo.FileVersion)]
[assembly: AssemblyInformationalVersion(AssemblyInfo.ProductVersion)]

[assembly: CLSCompliant(true)]
[assembly: ComVisible(false)]
[assembly: NeutralResourcesLanguage("en-US")]
[assembly: SatelliteContractVersion(AssemblyInfo.SatelliteContractVersion)]

internal static class AssemblyInfo
{
	public const string Major = "2";
	public const string Minor = "0";
	public const string Patch = "0";
	public const string SemVerSuffix = "-alpha";

	public const string Version = Major + "." + Minor + "." + Patch;
	public const string FileVersion = Major + "." + Minor + "." + Patch + SemVerSuffix;
	public const string ProductVersion = Major + "." + Minor;
	public const string SatelliteContractVersion = Major + "." + Minor + ".0.0";
}
