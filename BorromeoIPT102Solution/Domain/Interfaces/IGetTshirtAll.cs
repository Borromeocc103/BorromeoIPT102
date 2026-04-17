using Domain.Models;

namespace Domain.Interfaces;

public interface IGetTshirtAll
{
    Task<IEnumerable<Tshirt>> GetAllAsync();
}
