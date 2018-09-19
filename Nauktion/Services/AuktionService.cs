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

        public async Task<List<AuktionModel>> ListAuktionsAsync(bool includeClosed = false)
        {
            List<AuktionModel> list = await _repository.ListAuktionsAsync();
            return list.WhereIf(!includeClosed, a => !a.IsClosed)
                .OrderBy(a => a.StartDatum)
                .ToList();
        }

        public async Task<AuktionModel> GetAuktionAsync(int id)
        {
            return await _repository.GetAuktionAsync(id);
        }
    }
}