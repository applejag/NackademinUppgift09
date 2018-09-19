using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nauktion.Helpers;
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

        public async Task<List<AuktionModel>> ListAuktions(bool includeClosed = false)
        {
            List<AuktionModel> list = await _repository.ListAuktions();
            return list.WhereIf(!includeClosed, a => !a.IsClosed)
                .OrderBy(a => a.StartDatum)
                .ToList();
        }

        public async Task<AuktionModel> GetAuktion(int id)
        {
            return await _repository.GetAuktion(id);
        }
    }
}