using System.Collections.Generic;
using System.Threading.Tasks;
using Nauktion.Models;
using Nauktion.Repositories;

namespace Nauktion.Services
{
    public class AuktionService : IAuktionService
    {
        private readonly IAuktionRepository _repository;

        public AuktionService(IAuktionRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<AuktionModel>> ListAuktions()
        {
            return await _repository.ListAuktions();
        }

        public async Task<AuktionModel> GetAuktion(int id)
        {
            return await _repository.GetAuktion(id);
        }
    }
}