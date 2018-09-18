using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Nauktion.Models;

namespace Nauktion.Repositories
{
    public interface IAuktionRepository
    {
        [NotNull, Pure]
        Uri GetBaseAddress();

        [Pure]
        int GetGruppkod();

        [NotNull, ItemNotNull, Pure]
        Task<List<AuktionModel>> ListAuktions();

        [NotNull, ItemCanBeNull, Pure]
        Task<AuktionModel> GetAuktion(int id);
    }
}