using Domain.Models;

namespace Domain.Interfaces;

public interface IReadTshirtById
{
    Task<Tshirt?> ReadByIdAsync(int tshirtId);
}
