using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Nauktion.Models;

namespace Nauktion.Repositories
{
    public interface IAuktionRepository
    {
        [NotNull, ItemNotNull, Pure]
        Task<List<AuktionModel>> ListAuktionsAsync();

        [NotNull, ItemCanBeNull, Pure]
        Task<AuktionModel> GetAuktionAsync(int id);

        [NotNull, ItemNotNull, Pure]
        Task<List<BudModel>> ListBudsAsync(int auktionID);
    }
}