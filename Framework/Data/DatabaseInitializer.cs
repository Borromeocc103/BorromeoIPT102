using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Framework.Data
{
    public class DatabaseInitializer
    {
        private readonly string _connectionString;
        private readonly string _dbName;

        public DatabaseInitializer(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
            var builder = new SqlConnectionStringBuilder(_connectionString);
            _dbName = builder.InitialCatalog;
        }

        public void Initialize()
        {
            EnsureDatabaseExists();
            EnsureTableExists();
            EnsureStoredProceduresExist();
        }

        private void EnsureDatabaseExists()
        {
            var builder = new SqlConnectionStringBuilder(_connectionString)
            {
                InitialCatalog = "master"
            };
            using var conn = new SqlConnection(builder.ConnectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = $@"
                IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = '{_dbName}')
                BEGIN
                    CREATE DATABASE [{_dbName}]
                END";
            cmd.ExecuteNonQuery();
        }

        private void EnsureTableExists()
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();

            // Rename old table names if needed
            foreach (var oldName in new[] { "Employee", "Barber" })
            {
                cmd.CommandText = $@"
                    IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='dbo' AND TABLE_NAME='{oldName}')
                    AND NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='dbo' AND TABLE_NAME='TShirt')
                    BEGIN EXEC sp_rename 'dbo.{oldName}', 'TShirt' END";
                cmd.ExecuteNonQuery();
            }

            // Create table if not exists
            cmd.CommandText = @"
                IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='dbo' AND TABLE_NAME='TShirt')
                BEGIN
                    CREATE TABLE [dbo].[TShirt]
                    (
                        Id      INT IDENTITY(1,1) PRIMARY KEY,
                        Brand   NVARCHAR(100) NOT NULL,
                        Type    NVARCHAR(100) NOT NULL,
                        Design  NVARCHAR(100) NOT NULL,
                        Price   DECIMAL(18,2) NOT NULL
                    )
                END";
            cmd.ExecuteNonQuery();

            // Migrate old column names -> new ones
            var renames = new[]
            {
                ("FirstName",    "Brand"),
                ("LastName",     "Type"),
                ("Position",     "Design"),
                ("Specialty",    "Design"),
                ("Service",      "Design"),
                ("Salary",       "Price"),
                ("ServiceFee",   "Price"),
                ("CustomerName", "Brand"),
                ("ContactNumber","Type"),
            };

            foreach (var (oldCol, newCol) in renames)
            {
                // Skip if target column already exists
                cmd.CommandText = $@"
                    IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='TShirt' AND COLUMN_NAME='{oldCol}')
                    AND NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='TShirt' AND COLUMN_NAME='{newCol}')
                    BEGIN EXEC sp_rename 'dbo.TShirt.{oldCol}', '{newCol}', 'COLUMN' END";
                cmd.ExecuteNonQuery();
            }

            // Add missing columns if table exists but columns are missing
            var columns = new[]
            {
                ("Brand",  "NVARCHAR(100) NOT NULL DEFAULT ''"),
                ("Type",   "NVARCHAR(100) NOT NULL DEFAULT ''"),
                ("Design", "NVARCHAR(100) NOT NULL DEFAULT ''"),
                ("Price",  "DECIMAL(18,2) NOT NULL DEFAULT 0"),
            };
            foreach (var (col, def) in columns)
            {
                cmd.CommandText = $@"
                    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='TShirt' AND COLUMN_NAME='{col}')
                    BEGIN ALTER TABLE [dbo].[TShirt] ADD [{col}] {def} END";
                cmd.ExecuteNonQuery();
            }
        }

        private void EnsureStoredProceduresExist()
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            foreach (var (name, sql) in GetStoredProcedures())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = $@"
                    IF EXISTS (SELECT 1 FROM sys.objects WHERE type='P' AND name='{name}')
                        DROP PROCEDURE [dbo].[{name}]";
                cmd.ExecuteNonQuery();
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
        }

        private static IEnumerable<(string Name, string Sql)> GetStoredProcedures()
        {
            yield return ("GetAllTShirts", @"
                CREATE PROCEDURE [dbo].[GetAllTShirts]
                AS BEGIN
                    SELECT * FROM TShirt ORDER BY Id DESC
                END");

            yield return ("AddTShirt", @"
                CREATE PROCEDURE [dbo].[AddTShirt]
                    @Brand  NVARCHAR(100),
                    @Type   NVARCHAR(100),
                    @Design NVARCHAR(100),
                    @Price  DECIMAL(18,2)
                AS BEGIN
                    INSERT INTO TShirt (Brand, Type, Design, Price)
                    VALUES (@Brand, @Type, @Design, @Price)
                END");

            yield return ("UpdateTShirt", @"
                CREATE PROCEDURE [dbo].[UpdateTShirt]
                    @Id     INT,
                    @Brand  NVARCHAR(100),
                    @Type   NVARCHAR(100),
                    @Design NVARCHAR(100),
                    @Price  DECIMAL(18,2)
                AS BEGIN
                    UPDATE TShirt
                    SET Brand  = @Brand,
                        Type   = @Type,
                        Design = @Design,
                        Price  = @Price
                    WHERE Id = @Id
                END");

            yield return ("DeleteTShirt", @"
                CREATE PROCEDURE [dbo].[DeleteTShirt]
                    @Id INT
                AS BEGIN
                    DELETE FROM TShirt WHERE Id = @Id
                END");

            yield return ("SearchTShirt", @"
                CREATE PROCEDURE [dbo].[SearchTShirt]
                    @Keyword NVARCHAR(100)
                AS BEGIN
                    SELECT * FROM TShirt
                    WHERE Brand  LIKE '%' + @Keyword + '%'
                       OR Type   LIKE '%' + @Keyword + '%'
                       OR Design LIKE '%' + @Keyword + '%'
                    ORDER BY Id DESC
                END");
        }
    }
}
