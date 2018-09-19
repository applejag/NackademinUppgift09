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
        Task<AuktionModel> GetAuktionAsync(int auktionID);

        [NotNull, ItemNotNull, Pure]
        Task<List<BudModel>> ListBudsAsync(int auktionID);

        [NotNull]
        Task CreateBudAsync(BiddingViewModel model, [NotNull] NauktionUser budgivare);

        [NotNull, ItemCanBeNull, Pure]
        Task<string> ValidateBud(int auktionID, int summa, [NotNull] NauktionUser budgivare);

        [NotNull]
        Task CreateAuktionAsync([NotNull] AuktionViewModel model, [NotNull] NauktionUser skapare);

        [NotNull]
        Task AlterAuktionAsync([NotNull] AuktionViewModel model);

        [NotNull]
        Task DeleteAuktionAsync(int auktionID);
    }
}