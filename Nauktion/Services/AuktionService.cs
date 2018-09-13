using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Nauktion.Models;
using Nauktion.Repositories;

namespace Nauktion.Services
{
    public class AuktionService : IAuktionService
    {
        private readonly IAuktionRepository _repository;
        private readonly IConfiguration _configuration;

        public AuktionService(IAuktionRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        public int GetGruppkod()
        {
            return _configuration.GetValue<int>("NackowskisGruppkod");
        }

        public List<AuktionModel> ListAuktions()
        {
            return _repository.ListAuktions(GetGruppkod());
        }

        public AuktionModel GetAuktion(int id)
        {
            return _repository.GetAuktion(GetGruppkod(), id);
        }
    }
}