using Microsoft.Data.SqlClient;

namespace BorromeoWPF.Data;

public class DatabaseInitializer
{
    private readonly string _connectionString;

    public DatabaseInitializer(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void Initialize()
    {
        EnsureDatabaseExists();
        EnsureTableExists();
        EnsureStoredProcedures();
    }

    private void EnsureDatabaseExists()
    {
        // Connect to master to create the DB if it doesn't exist
        var builder = new SqlConnectionStringBuilder(_connectionString);
        var dbName = builder.InitialCatalog;
        builder.InitialCatalog = "master";

        using var conn = new SqlConnection(builder.ConnectionString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = $"""
            IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = '{dbName}')
            BEGIN
                CREATE DATABASE [{dbName}]
            END
            """;
        cmd.ExecuteNonQuery();
    }

    private void EnsureTableExists()
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = """
            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Tshirt' AND xtype='U')
            BEGIN
                CREATE TABLE [dbo].[Tshirt]
                (
                    [TshirtId]   INT           NOT NULL IDENTITY(1,1) PRIMARY KEY,
                    [TshirtName] NVARCHAR(100) NOT NULL,
                    [Quantity]   INT           NOT NULL,
                    [Price]      DECIMAL(10,2) NOT NULL,
                    [Brand]      NVARCHAR(100) NOT NULL
                )
            END
            """;
        cmd.ExecuteNonQuery();
    }

    private void EnsureStoredProcedures()
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        ExecuteScript(conn, "CreateTshirt", """
            CREATE PROCEDURE [dbo].[CreateTshirt]
                @Name     NVARCHAR(100),
                @Quantity INT,
                @Price    DECIMAL(10,2),
                @Brand    NVARCHAR(100)
            AS
            BEGIN
                SET NOCOUNT ON;
                INSERT INTO [dbo].[Tshirt] (TshirtName, Quantity, Price, Brand)
                VALUES (@Name, @Quantity, @Price, @Brand);
            END
            """);

        ExecuteScript(conn, "UpdateTshirt", """
            CREATE PROCEDURE [dbo].[UpdateTshirt]
                @TshirtId INT,
                @Name     NVARCHAR(100),
                @Quantity INT,
                @Price    DECIMAL(10,2),
                @Brand    NVARCHAR(100)
            AS
            BEGIN
                SET NOCOUNT ON;
                UPDATE [dbo].[Tshirt]
                SET TshirtName = @Name,
                    Quantity   = @Quantity,
                    Price      = @Price,
                    Brand      = @Brand
                WHERE TshirtId = @TshirtId;
            END
            """);

        ExecuteScript(conn, "DeleteTshirt", """
            CREATE PROCEDURE [dbo].[DeleteTshirt]
                @TshirtId INT
            AS
            BEGIN
                SET NOCOUNT ON;
                DELETE FROM [dbo].[Tshirt] WHERE TshirtId = @TshirtId;
            END
            """);

        ExecuteScript(conn, "GetAllTshirt", """
            CREATE PROCEDURE [dbo].[GetAllTshirt]
            AS
            BEGIN
                SET NOCOUNT ON;
                SELECT TshirtId, TshirtName AS Name, Quantity, Price, Brand
                FROM [dbo].[Tshirt];
            END
            """);

        ExecuteScript(conn, "ReadTshirtById", """
            CREATE PROCEDURE [dbo].[ReadTshirtById]
                @TshirtId INT
            AS
            BEGIN
                SET NOCOUNT ON;
                SELECT TshirtId, TshirtName AS Name, Quantity, Price, Brand
                FROM [dbo].[Tshirt]
                WHERE TshirtId = @TshirtId;
            END
            """);
    }

    private static void ExecuteScript(SqlConnection conn, string procName, string createScript)
    {
        var cmd = conn.CreateCommand();
        // Only create if it doesn't exist yet
        cmd.CommandText = $"""
            IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = '{procName}')
            BEGIN
                EXEC sp_executesql N'{createScript.Replace("'", "''")}' 
            END
            """;
        cmd.ExecuteNonQuery();
    }
}
