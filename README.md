# powershell-npgsql

Use PostgreSQL with PowerShell and the Npgsql driver

To use PostgreSQL with Powershell, we need to build a small project to retrieve DLL
and all dependancies

## build and publish

### Install Dotnet

You need to install dotnet to build this project, we use dotnet 8.0 

```shell
winget install --id Microsoft.DotNet.SDK.8 -e
```

### Install dependancies

```shell
dotnet restore
```

### Build the project

```shell
dotnet build
```

### Publish the project

To retrieve Npgsql driver with all dependancies (prevent resolution failed)

```shell
dotnet publish --sc true -o .\build
```

## Test 

Now we can test if execution is correct and if we can connect to the database.

```shell
.\build\powershell-npgsql.exe      
PostgreSQL version retriever (Npgsql)
Host [localhost]: 
Port [5432]: 
Database [postgres]: 
Username [ChristopheChauvet]: 
Password: ********
Connecting to localhost:5432 database=postgres as ChristopheChauvet...
PostgreSQL server version: PostgreSQL 17.2 on x86_64-windows, compiled by msvc-19.42.34435, 64-bit
```

If the program execute without errors, you can create an archive to deploy libraries with your `PowerShell` script

```shell
Compress-Archive -Path .\build\*.dll -DestinationPath .\PowerShell-Npgsql-9.0.4.zip
```

In the build folder, you have all DLL availables, on your powershell script just import
this forlder with `Add-Type` command

