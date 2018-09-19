using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Nauktion.Models;

namespace Nauktion.Services
{
    public interface IAuktionService
    {
        [NotNull, ItemNotNull, Pure]
        Task<List<AuktionModel>> ListAuktionsAsync(bool includeClosed = false);

        [NotNull, ItemCanBeNull, Pure]
        Task<AuktionModel> GetAuktionAsync(int id);

        [NotNull, ItemNotNull, Pure]
        Task<List<BudModel>> ListBudsAsync(int auktionID);

        [NotNull]
        Task CreateBudAsync(BudModel model);
    }
}