
using System;
using System.Text;
using Npgsql;

// Small interactive program to connect to PostgreSQL and print server version.
Console.WriteLine("PostgreSQL version retriever (Npgsql)");

string Prompt(string label, string? defaultValue = null)
{
	Console.Write(label + (defaultValue is not null ? $" [{defaultValue}]" : "") + ": ");
	var input = Console.ReadLine();
	if (string.IsNullOrEmpty(input) && defaultValue is not null)
		return defaultValue;
	return input ?? string.Empty;
}

string ReadPassword(string prompt)
{
	Console.Write(prompt);
	var sb = new StringBuilder();
	ConsoleKeyInfo key;
	while ((key = Console.ReadKey(true)).Key != ConsoleKey.Enter)
	{
		if (key.Key == ConsoleKey.Backspace)
		{
			if (sb.Length > 0)
			{
				sb.Length--;
				Console.Write("\b \b");
			}
			continue;
		}
		// ignore other control characters
		if (char.IsControl(key.KeyChar))
			continue;
		sb.Append(key.KeyChar);
		Console.Write('*');
	}
	Console.WriteLine();
	return sb.ToString();
}

try
{
	var host = Prompt("Host", "localhost");
	var port = Prompt("Port", "5432");
	var database = Prompt("Database", "postgres");
	var username = Prompt("Username", Environment.UserName);
	var password = ReadPassword("Password: ");

	var cs = new NpgsqlConnectionStringBuilder
	{
		Host = host,
		Port = int.TryParse(port, out var p) ? p : 5432,
		Database = database,
		Username = username,
		Password = password,
		SslMode = SslMode.Prefer
	};

	Console.WriteLine($"Connecting to {cs.Host}:{cs.Port} database={cs.Database} as {cs.Username}...");

	using var conn = new NpgsqlConnection(cs.ConnectionString);
	conn.Open();

	using var cmd = new NpgsqlCommand("SELECT version();", conn);
	var version = cmd.ExecuteScalar()?.ToString();

	Console.WriteLine("PostgreSQL server version: " + (version ?? "(unknown)"));
	conn.Close();
}
catch (Exception ex)
{
	Console.WriteLine("Error: " + ex.Message);
	Environment.ExitCode = 1;
}
