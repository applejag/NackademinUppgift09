using System.Collections.Generic;
using JetBrains.Annotations;
using Nauktion.Models;

namespace Nauktion.Repositories
{
    public interface IAuktionRepository
    {
        [NotNull, ItemNotNull]
        List<AuktionModel> ListAuktions(int gruppkod);

        [CanBeNull]
        AuktionModel GetAuktion(int gruppkod, int id);
    }
}