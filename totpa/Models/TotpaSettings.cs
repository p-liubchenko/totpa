namespace totpa.Models;

public class TotpaSettings
{
	public string StorageType { get; set; } = "json";
	public bool UseSecureSettings { get; set; } = false;
	public Dictionary<string, string> ConnectionStrings { get; set; } = new();
	public Dictionary<string, Dictionary<string, string>> RepositorySettings { get; set; } = new();
}
