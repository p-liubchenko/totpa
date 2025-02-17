using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace totpa.Repositories;

class JsonTotpaRepository : ITotpaRepository
{
	private readonly string DataFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".totpa/.totpa_store.json");

	public void AddAccount(string name, string url)
	{
		var accounts = GetAccounts();
		if (accounts.ContainsKey(name))
		{
			Console.WriteLine("Error: Account name already exists.");
			return;
		}
		accounts[name] = url;
		File.WriteAllText(DataFile, JsonSerializer.Serialize(accounts));
		Console.WriteLine("TOTP account added successfully.");
	}

	public Dictionary<string, string> GetAccounts()
	{
		if (!File.Exists(DataFile)) return new Dictionary<string, string>();
		return JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(DataFile)) ?? new Dictionary<string, string>();
	}

	public string GetAccount(string name)
	{
		var accounts = GetAccounts();
		return accounts.ContainsKey(name) ? accounts[name] : null;
	}

}
