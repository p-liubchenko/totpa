namespace totpa;

interface ITotpaSettingsProvider
{
	string GetStorageType();
	string ReadConnectionString();
	void SaveStorage(string storageType);
}