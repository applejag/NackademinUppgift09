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
        Uri GetBaseAddress();

        [NotNull, ItemNotNull]
        Task<List<AuktionModel>> ListAuktions(int gruppkod);

        [NotNull, ItemCanBeNull]
        Task<AuktionModel> GetAuktion(int gruppkod, int id);
    }
}