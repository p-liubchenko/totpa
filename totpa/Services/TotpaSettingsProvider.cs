using System.Text.Json;

using totpa.Models;

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
			File.WriteAllText(SettingsFile, JsonSerializer.Serialize(new TotpaSettings()));
		}
	}

	public TotpaSettings LoadSettings()
	{
		if (!File.Exists(SettingsFile)) return new TotpaSettings();
		return JsonSerializer.Deserialize<TotpaSettings>(File.ReadAllText(SettingsFile)) ?? new TotpaSettings();
	}

	public void SaveSettings(TotpaSettings settings)
	{
		File.WriteAllText(SettingsFile, JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true }));
	}

	public string GetStorageType()
	{
		var settings = LoadSettings();

		return settings.StorageType;
	}

	public void SaveStorage(string storageType)
	{
		var settings = LoadSettings();
		settings.StorageType = storageType;
		SaveSettings(settings);
	}

	public string ReadConnectionString()
	{
		return string.Empty;
	}
}