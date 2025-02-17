using System.Management;

namespace totpa.Repositories;

public class WmiTotpaRepository : ITotpaRepository
{
	private const string Namespace = "root\\CIMV2";
	private const string ClassName = "TotpaAccounts";

	public WmiTotpaRepository()
	{
		EnsureClassExists();
	}

	private void EnsureClassExists()
	{
		var scope = new ManagementScope($"\\\\.\\{Namespace}");
		scope.Connect();
		using var searcher = new ManagementObjectSearcher(scope, new ObjectQuery($"SELECT * FROM meta_class WHERE __Class = '{ClassName}'"));
		if (searcher.Get().Count == 0)
		{
			using var managementClass = new ManagementClass(scope, new ManagementPath("__ClassCreation"), null);
			managementClass["__CLASS"] = ClassName;
			managementClass.Put();
		}
	}

	public void AddAccount(string name, string url)
	{
		var scope = new ManagementScope($"\\\\.\\{Namespace}");
		scope.Connect();
		using var managementClass = new ManagementClass(scope, new ManagementPath(ClassName), null);

		var newInstance = managementClass.CreateInstance();
		if (newInstance == null) throw new Exception("Failed to create WMI instance");
		newInstance["Name"] = name;
		newInstance["Url"] = url;
		newInstance.Put();
	}

	public Dictionary<string, string> GetAccounts()
	{
		var accounts = new Dictionary<string, string>();
		var scope = new ManagementScope($"\\\\.\\{Namespace}");
		scope.Connect();
		using var searcher = new ManagementObjectSearcher(scope, new ObjectQuery($"SELECT Name, Url FROM {ClassName}"));
		foreach (ManagementObject obj in searcher.Get())
		{
			accounts[obj["Name"].ToString()] = obj["Url"].ToString();
		}
		return accounts;
	}

	public string GetAccount(string name)
	{
		var scope = new ManagementScope($"\\\\.\\{Namespace}");
		scope.Connect();
		using var searcher = new ManagementObjectSearcher(scope, new ObjectQuery($"SELECT Url FROM {ClassName} WHERE Name = '{name}'"));
		foreach (ManagementObject obj in searcher.Get())
		{
			return obj["Url"].ToString();
		}
		return string.Empty;
	}
}
