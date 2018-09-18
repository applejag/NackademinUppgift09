using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Nauktion.Models;

namespace Nauktion.Services
{
    public interface IAuktionService
    {
        int GetGruppkod();

        [NotNull, ItemNotNull]
        Task<List<AuktionModel>> ListAuktions();

        [NotNull, ItemCanBeNull]
        Task<AuktionModel> GetAuktion(int id);
    }
}