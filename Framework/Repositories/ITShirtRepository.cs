using System.Collections.Generic;
using Domain.Models;

namespace Framework.Repositories
{
    public interface ITShirtRepository
    {
        IEnumerable<TShirt> GetAll();
        IEnumerable<TShirt> Search(string keyword);
        void Add(TShirt TShirt);
        void Update(TShirt TShirt);
        void Delete(int id);
    }
}
