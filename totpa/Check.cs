using System.Runtime.InteropServices;

namespace totpa;

class Check
{
	public static void IsWindows()
	{
		if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
		{
			throw new PlatformNotSupportedException("WMI is only supported on Windows");
		}
	}
}
