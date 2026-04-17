using Domain.Interfaces;
using Domain.Models;
using Framework.Extensions;
using Framework.Repository;

namespace Framework.Queries;

public class ReadTshirtById : TshirtRepository, IReadTshirtById
{
    public ReadTshirtById(string connectionString) : base(connectionString) { }

    public async Task<Tshirt?> ReadByIdAsync(int tshirtId)
    {
        return await QueryFirstOrDefaultAsync<Tshirt>("ReadTshirtById", tshirtId.ToReadByIdParameters());
    }
}
