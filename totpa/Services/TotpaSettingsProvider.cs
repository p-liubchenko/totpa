using System.Text.Json;

namespace totpa.Services;

class TotpaSettingsProvider : ITotpaSettingsProvider
{
	private readonly string SettingsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".totpa");
	private readonly string SettingsFile;
	public TotpaSettingsProvider()
	{
		SettingsFile = Path.Combine(SettingsDirectory, "totpa_settings.json");
		EnsureSettingsFileExists();
	}

	private void EnsureSettingsFileExists()
	{
		if (!Directory.Exists(SettingsDirectory))
		{
			Directory.CreateDirectory(SettingsDirectory);
		}
		if (!File.Exists(SettingsFile))
		{
			File.WriteAllText(SettingsFile, JsonSerializer.Serialize(new Dictionary<string, string>()));
		}
	}

	public string GetStorageType()
	{
		if (File.Exists(SettingsFile))
		{
			var settings = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(SettingsFile));

			if (settings != null && settings.ContainsKey("storage"))
			{
				return settings["storage"];
			}
		}
		return string.Empty;
	}

	public void SaveStorage(string storageType)
	{
		var settings = new Dictionary<string, string> { { "storage", storageType } };
		File.WriteAllText(SettingsFile, JsonSerializer.Serialize(settings));
		Console.WriteLine($"Storage type set to: {storageType}");
	}

	public string ReadConnectionString()
	{
		return string.Empty;
	}
}