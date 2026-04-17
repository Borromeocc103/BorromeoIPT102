using Domain.Interfaces;
using Framework.Extensions;
using Framework.Repository;

namespace Framework.Commands;

public class DeleteCommand : TshirtRepository, IDeleteCommand
{
    public DeleteCommand(string connectionString) : base(connectionString) { }

    public async Task DeleteAsync(int tshirtId)
    {
        await ExecuteAsync("DeleteTshirt", tshirtId.ToDeleteParameters());
    }
}
