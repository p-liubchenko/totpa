namespace totpa;

interface ITotpaSettingsProvider
{
	string GetStorageType();
	void SaveStorage(string storageType);
}