using Dapper;
using Domain.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Framework.Repository;

public class TshirtRepository
{
    private readonly string _connectionString;

    public TshirtRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    protected async Task ExecuteAsync(string storedProcedure, DynamicParameters parameters)
    {
        using var conn = CreateConnection();
        await conn.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
    }

    protected async Task<IEnumerable<T>> QueryAsync<T>(string storedProcedure, DynamicParameters? parameters = null)
    {
        using var conn = CreateConnection();
        return await conn.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
    }

    protected async Task<T?> QueryFirstOrDefaultAsync<T>(string storedProcedure, DynamicParameters parameters)
    {
        using var conn = CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
    }
}
