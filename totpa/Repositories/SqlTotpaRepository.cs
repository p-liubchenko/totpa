using Microsoft.Data.SqlClient;

using System.Text;

namespace totpa.Repositories;

public class SqlTotpaRepository : ITotpaRepository
{
	private readonly string _connectionString;

	public SqlTotpaRepository(string connectionString)
	{
		_connectionString = connectionString;
		EnsureDatabaseSetup();
	}

	private void EnsureDatabaseSetup()
	{
		using var connection = new SqlConnection(_connectionString);
		connection.Open();
		using var command = new SqlCommand(@"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='TotpaAccounts' AND xtype='U')
                CREATE TABLE TotpaAccounts (
                    Name NVARCHAR(255) PRIMARY KEY,
                    Url NVARCHAR(1024) NOT NULL
                );
            ", connection);
		command.ExecuteNonQuery();
	}

	public void AddAccount(string name, string url)
	{
		using var connection = new SqlConnection(_connectionString);
		connection.Open();
		using var command = new SqlCommand(@"
                INSERT INTO TotpaAccounts (Name, Url)
                VALUES (@Name, @Url)
                ON DUPLICATE KEY UPDATE Url = @Url;
            ", connection);
		command.Parameters.AddWithValue("@Name", name);
		command.Parameters.AddWithValue("@Url", url);
		command.ExecuteNonQuery();
	}

	public Dictionary<string, string> GetAccounts()
	{
		var accounts = new Dictionary<string, string>();
		using var connection = new SqlConnection(_connectionString);
		connection.Open();
		using var command = new SqlCommand("SELECT Name, Url FROM TotpaAccounts;", connection);
		using var reader = command.ExecuteReader();
		while (reader.Read())
		{
			accounts[reader.GetString(0)] = reader.GetString(1);
		}
		return accounts;
	}

	public string GetAccount(string name)
	{
		using var connection = new SqlConnection(_connectionString);
		connection.Open();
		using var command = new SqlCommand("SELECT Url FROM TotpaAccounts WHERE Name = @Name;", connection);
		command.Parameters.AddWithValue("@Name", name);
		return command.ExecuteScalar()?.ToString() ?? string.Empty;
	}
}
