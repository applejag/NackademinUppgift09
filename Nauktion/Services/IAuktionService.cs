using System.Collections.Generic;
using JetBrains.Annotations;
using Nauktion.Models;

namespace Nauktion.Services
{
    public interface IAuktionService
    {
        int GetGruppkod();

        [NotNull, ItemNotNull]
        List<AuktionModel> ListAuktions();

        [CanBeNull]
        AuktionModel GetAuktion(int id);
    }
}