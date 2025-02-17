namespace totpa;

public static class OtpHelper
{

	internal static string ExtractSecretFromTotpUrl(string totpUrl)
	{
		var match = System.Text.RegularExpressions.Regex.Match(totpUrl, @"secret=([^&]+)");
		return match.Success ? Uri.UnescapeDataString(match.Groups[1].Value) : string.Empty;
	}
}
