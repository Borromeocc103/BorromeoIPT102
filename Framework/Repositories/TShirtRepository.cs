using System.Collections.Generic;
using System.Data;
using Dapper;
using Domain.Models;
using Framework.Data;

namespace Framework.Repositories
{
    public class TShirtRepository : ITShirtRepository
    {
        private readonly AppDbContext _context;

        public TShirtRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<TShirt> GetAll()
        {
            using var connection = _context.CreateConnection();
            return connection.Query<TShirt>(
                "GetAllTShirts",
                commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<TShirt> Search(string keyword)
        {
            using var connection = _context.CreateConnection();
            return connection.Query<TShirt>(
                "SearchTShirt",
                new { Keyword = keyword },
                commandType: CommandType.StoredProcedure);
        }

        public void Add(TShirt TShirt)
        {
            using var connection = _context.CreateConnection();
            connection.Execute(
                "AddTShirt",
                new
                {
                    TShirt.Brand,
                    TShirt.Type,
                    TShirt.Design,
                    TShirt.Price
                },
                commandType: CommandType.StoredProcedure);
        }

        public void Update(TShirt TShirt)
        {
            using var connection = _context.CreateConnection();
            connection.Execute(
                "UpdateTShirt",
                TShirt,
                commandType: CommandType.StoredProcedure);
        }

        public void Delete(int id)
        {
            using var connection = _context.CreateConnection();
            connection.Execute(
                "DeleteTShirt",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
        }
    }
}
