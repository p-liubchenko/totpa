using Microsoft.Data.Sqlite;

namespace totpa.Repositories;

public class SQLiteTotpaRepository : ITotpaRepository
{
	private readonly string _dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".totpa/totpa.db");

	public SQLiteTotpaRepository()
	{
		if (!File.Exists(_dbPath))
		{
			//SqliteConnection.Cre(_dbPath);
			using var conn = new SqliteConnection($"Data Source={_dbPath}");
			conn.Open();
			using var cmd = new SqliteCommand("CREATE TABLE IF NOT EXISTS TotpAccounts (Name TEXT PRIMARY KEY, Url TEXT);", conn);
			cmd.ExecuteNonQuery();
		}
	}

	public void AddAccount(string name, string url)
	{
		using var conn = new SqliteConnection($"Data Source={_dbPath};Version=3;");
		conn.Open();
		using var cmd = new SqliteCommand("INSERT OR REPLACE INTO TotpAccounts (Name, Url) VALUES (@Name, @Url);", conn);
		cmd.Parameters.AddWithValue("@Name", name);
		cmd.Parameters.AddWithValue("@Url", url);
		cmd.ExecuteNonQuery();
	}

	public Dictionary<string, string> GetAccounts()
	{
		var accounts = new Dictionary<string, string>();
		using var conn = new SqliteConnection($"Data Source={_dbPath};Version=3;");
		conn.Open();
		using var cmd = new SqliteCommand("SELECT Name, Url FROM TotpAccounts;", conn);
		using var reader = cmd.ExecuteReader();
		while (reader.Read())
		{
			accounts[reader.GetString(0)] = reader.GetString(1);
		}
		return accounts;
	}

	public string GetAccount(string name)
	{
		using var conn = new SqliteConnection($"Data Source={_dbPath};Version=3;");
		conn.Open();
		using var cmd = new SqliteCommand("SELECT Url FROM TotpAccounts WHERE Name = @Name;", conn);
		cmd.Parameters.AddWithValue("@Name", name);
		return cmd.ExecuteScalar()?.ToString() ?? string.Empty;
	}
}
