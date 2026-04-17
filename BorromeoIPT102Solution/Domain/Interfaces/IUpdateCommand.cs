using Domain.Models;

namespace Domain.Interfaces;

public interface IUpdateCommand
{
    Task UpdateAsync(Tshirt tshirt);
}
