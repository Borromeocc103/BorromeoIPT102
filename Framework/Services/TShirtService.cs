using System;
using System.Collections.Generic;
using Domain.Models;
using Framework.Repositories;

namespace Framework.Services
{
    public class TShirtService : ITShirtService
    {
        private readonly ITShirtRepository _repository;

        public TShirtService(ITShirtRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<TShirt> GetAll()
            => _repository.GetAll();

        public IEnumerable<TShirt> Search(string keyword)
            => _repository.Search(keyword);

        public void Create(TShirt TShirt)
        {
            if (string.IsNullOrWhiteSpace(TShirt.Brand))
                throw new Exception("Brand is required");

            _repository.Add(TShirt);
        }

        public void Edit(TShirt TShirt)
            => _repository.Update(TShirt);

        public void Remove(int id)
            => _repository.Delete(id);
    }
}
