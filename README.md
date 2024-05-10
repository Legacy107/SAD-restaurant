# Koala restaurant management system

## Description
A restaurant management system using MAUI, .NET 8, and EF Core. The system is designed to support the following operations:
- View menu
- Place order
- Handle and serve order
- Check-in and reservation
- Make payment
- Manage menu

## Development

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/)
- Set up MAUI development environment by following the instructions [here](https://learn.microsoft.com/en-au/dotnet/maui/get-started/installation?view=net-maui-8.0&tabs=vswin).
- Install EF Core CLI by running the following command:
```bash
dotnet tool install --global dotnet-ef
```

### Database
All database related logic is in the `Database` project. The database is created using EF Core.

Database configuration can be found in the `Settings.cs` file in the `Database` project directory.

After making changes to the models, migrate the database by running the following command in the `Database` project directory:
```bash
dotnet ef migrations add <migration-name>
```

Then create or update the database by running the following command in the `Database` project directory:
```bash
dotnet ef database update
```
