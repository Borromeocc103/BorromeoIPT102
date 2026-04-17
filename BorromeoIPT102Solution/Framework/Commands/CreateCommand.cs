using Domain.Interfaces;
using Domain.Models;
using Framework.Extensions;
using Framework.Repository;

namespace Framework.Commands;

public class CreateCommand : TshirtRepository, ICreateCommand
{
    public CreateCommand(string connectionString) : base(connectionString) { }

    public async Task CreateAsync(Tshirt tshirt)
    {
        await ExecuteAsync("CreateTshirt", tshirt.ToCreateParameters());
    }
}
