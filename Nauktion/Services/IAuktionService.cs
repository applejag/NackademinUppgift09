using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Nauktion.Models;

namespace Nauktion.Services
{
    public interface IAuktionService
    {
        [NotNull, ItemNotNull, Pure]
        Task<List<AuktionModel>> ListAuktions();

        [NotNull, ItemCanBeNull, Pure]
        Task<AuktionModel> GetAuktion(int id);
    }
}