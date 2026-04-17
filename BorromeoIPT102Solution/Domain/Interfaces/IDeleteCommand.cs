namespace Domain.Interfaces;

public interface IDeleteCommand
{
    Task DeleteAsync(int tshirtId);
}
