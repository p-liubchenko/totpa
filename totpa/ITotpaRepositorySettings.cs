namespace totpa;

public interface ITotpaRepositorySettings
{
	static abstract bool ValidateSettings(Dictionary<string, string> settings);
	static abstract Dictionary<string, string> ParseSettings(string rawSettings);
}
