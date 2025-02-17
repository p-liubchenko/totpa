using StackExchange.Redis;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace totpa.Repositories;

public class RedisTotpaRepository : ITotpaRepository
{
	private readonly ConnectionMultiplexer _redis;
	private readonly IDatabase _db;

	public RedisTotpaRepository(string connectionString)
	{
		_redis = ConnectionMultiplexer.Connect(connectionString);
		_db = _redis.GetDatabase();
	}

	public void AddAccount(string name, string url) => _db.StringSet(name, url);

	public Dictionary<string, string> GetAccounts()
	{
		var server = _redis.GetServer(_redis.GetEndPoints()[0]);
		var keys = server.Keys();
		var accounts = new Dictionary<string, string>();
		foreach (var key in keys)
		{
			accounts[key] = _db.StringGet(key);
		}
		return accounts;
	}

	public string GetAccount(string name) => _db.StringGet(name);
}