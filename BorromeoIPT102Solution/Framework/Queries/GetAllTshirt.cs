using Domain.Interfaces;
using Domain.Models;
using Framework.Repository;

namespace Framework.Queries;

public class GetAllTshirt : TshirtRepository, IGetTshirtAll
{
    public GetAllTshirt(string connectionString) : base(connectionString) { }

    public async Task<IEnumerable<Tshirt>> GetAllAsync()
    {
        return await QueryAsync<Tshirt>("GetAllTshirt");
    }
}
