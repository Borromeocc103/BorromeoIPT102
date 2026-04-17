using Domain.Models;

namespace Domain.Interfaces;

public interface ICreateCommand
{
    Task CreateAsync(Tshirt tshirt);
}
