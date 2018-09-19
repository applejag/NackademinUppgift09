using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Nauktion.Models;

namespace Nauktion.Repositories
{
    public interface IAuktionRepository
    {
        [NotNull]
        Uri BaseAddress { get; }

        int Gruppkod { get; }

        [NotNull, ItemNotNull, Pure]
        Task<List<AuktionModel>> ListAuktionsAsync();

        [NotNull, ItemCanBeNull, Pure]
        Task<AuktionModel> GetAuktionAsync(int auktionID);

        [NotNull, ItemNotNull, Pure]
        Task<List<BudModel>> ListBudsAsync(int auktionID);

        [NotNull]
        Task CreateBudAsync([NotNull] BudModel model);

        [NotNull]
        Task CreateAuktionAsync([NotNull] AuktionModel model);

        [NotNull]
        Task AlterAuktionAsync([NotNull] AuktionModel model);

        [NotNull]
        Task DeleteAuktionAsync(int auktionID);
    }
}