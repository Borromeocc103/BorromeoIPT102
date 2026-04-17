using Domain.Interfaces;
using Domain.Models;
using Framework.Extensions;
using Framework.Repository;

namespace Framework.Commands;

public class UpdateCommand : TshirtRepository, IUpdateCommand
{
    public UpdateCommand(string connectionString) : base(connectionString) { }

    public async Task UpdateAsync(Tshirt tshirt)
    {
        await ExecuteAsync("UpdateTshirt", tshirt.ToUpdateParameters());
    }
}
