using OtpNet;

using System.CommandLine;

using totpa.Repositories;
using totpa.Services;

using static totpa.OtpHelper;

namespace totpa;

internal class Program
{
	private static ITotpaRepository _repository;
	private static ITotpaSettingsProvider _settingsProvider;

	static async Task Main(string[] args)
	{
		_settingsProvider = new TotpaSettingsProvider();
		
		var rootCommand = new RootCommand("TOTP CLI tool");

		var addCommand = new Command("add", "Add a new TOTP account")
		{
			new Option<string>("-n", "Unique name for the account") { IsRequired = true },
			new Option<string>("-u", "TOTP URL") { IsRequired = true }
		};
		addCommand.SetHandler(async (context) => await add(
			context.ParseResult.GetValueForOption(addCommand.Options[0] as Option<string>),
			context.ParseResult.GetValueForOption(addCommand.Options[1] as Option<string>)
		));
		rootCommand.AddCommand(addCommand);

		var listCommand = new Command("list", "List all stored accounts");
		listCommand.SetHandler(async () => await list());
		rootCommand.AddCommand(listCommand);

		var getCommand = new Command("get", "Generate a TOTP code")
		{
			new Option<string>("-n", "Unique name for the account")
		};
		getCommand.SetHandler(async (context) => await get(
			context.ParseResult.GetValueForOption(getCommand.Options[0] as Option<string>)
		));
		rootCommand.AddCommand(getCommand);

		var settingsCommand = new Command("settings", "Configure storage type and options")
		{
			new Option<string>("-t", "Storage type (json, sqlite, sql, redis, wmi)") { IsRequired = true }
		};
		settingsCommand.SetHandler(async (context) => await storage(
			context.ParseResult.GetValueForOption(settingsCommand.Options[0] as Option<string>)
		));
		rootCommand.AddCommand(settingsCommand);

		await rootCommand.InvokeAsync(args);
	}
	private static async Task list()
	{
		_repository = InitializeRepository();
		var accounts = _repository.GetAccounts();
		foreach (var key in accounts.Keys)
		{
			Console.WriteLine(key);
		}
		await Task.CompletedTask;
	}

	private static async Task add(string n, string u)
	{
		_repository = InitializeRepository();
		_repository.AddAccount(n, u);
		await Task.CompletedTask;
	}

	private static async Task get(string n)
	{
		_repository = InitializeRepository();
		var totpUrl = _repository.GetAccount(n);
		if (string.IsNullOrEmpty(totpUrl))
		{
			Console.WriteLine("Error: Account not found.");
			return;
		}

		string secret = ExtractSecretFromTotpUrl(totpUrl);
		if (string.IsNullOrEmpty(secret))
		{
			Console.WriteLine("Error: Could not extract TOTP secret.");
			return;
		}

		var secretBytes = Base32Encoding.ToBytes(secret);
		var totp = new Totp(secretBytes);
		Console.WriteLine($"Generated TOTP Code for {n}: {totp.ComputeTotp()} (expires in {totp.RemainingSeconds()})");
		await Task.CompletedTask;
	}

	private static async Task storage(string t)
	{
		_settingsProvider.SaveStorage(t);
		await Task.CompletedTask;
	}

	private static ITotpaRepository InitializeRepository()
	{
		string storageProvider = _settingsProvider.GetStorageType();

		return storageProvider switch
		{
			"json" => new JsonTotpaRepository(),
			"sqlite" => new SQLiteTotpaRepository(),
			_ => new JsonTotpaRepository()
		};
	}

}