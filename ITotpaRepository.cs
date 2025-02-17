using System;

interface ITotpaRepository
{
	void AddAccount(string name, string url);
	Dictionary<string, string> GetAccounts();
	string GetAccount(string name);
}