using System;
namespace Database;

public class Settings
{
	public string DbHost { get; set; } = "localhost";
	public string DbPort { get; set; } = "3306";
	public string DbUser { get; set; } = "root";
	public string DbPassword { get; set; } = "password";
	public string DbDatabase { get; set; } = "db";
}
