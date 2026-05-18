using System.Collections.Generic;
using Domain.Models;

namespace Framework.Services
{
    public interface ITShirtService
    {
        IEnumerable<TShirt> GetAll();
        IEnumerable<TShirt> Search(string keyword);
        void Create(TShirt TShirt);
        void Edit(TShirt TShirt);
        void Remove(int id);
    }
}
