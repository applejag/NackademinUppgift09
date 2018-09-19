using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Nauktion.Models;
using Nauktion.Services;

namespace Nauktion.Helpers
{
    public static class AuktionServiceExtensions
    {
        [NotNull, ItemNotNull]
        public static async Task<List<AuktionBudViewModel>> ListAuktionBudsAsync(this IAuktionService service, bool includeClosed = false)
        {
            List<AuktionModel> auktions = await service.ListAuktionsAsync(includeClosed);
            var viewModels = new List<AuktionBudViewModel>();

            foreach (AuktionModel a in auktions)
            {
                List<BudModel> buds = await service.ListBudsAsync(a.AuktionID);
                viewModels.Add(new AuktionBudViewModel(a, buds));
            }

            return viewModels;
        }

        [NotNull, ItemNotNull]
        public static async Task<AuktionBudPageinatedViewModel> ListAuktionBudsPaginatedAsync(this IAuktionService service, int page = 1, bool includeClosed = false)
        {
            List<AuktionModel> auktions = await service.ListAuktionsAsync(includeClosed);

            var viewModel = new AuktionBudPageinatedViewModel
            {
                NumOfPages = MathHelpers.DivCeil(auktions.Count, AuktionBudPageinatedViewModel.MODELS_PER_PAGE),
                TotalModelCount = auktions.Count
            };

            viewModel.Page = page = Math.Clamp(page, 1, viewModel.NumOfPages);

            foreach (AuktionModel a in auktions
                .Skip((page - 1) * AuktionBudPageinatedViewModel.MODELS_PER_PAGE)
                .Take(AuktionBudPageinatedViewModel.MODELS_PER_PAGE))
            {
                List<BudModel> buds = await service.ListBudsAsync(a.AuktionID);
                viewModel.Models.Add(new AuktionBudViewModel(a, buds));
            }

            return viewModel;
        }

        [NotNull, ItemCanBeNull]
        public static async Task<AuktionBudViewModel> GetAuktionBudsAsync(this IAuktionService service, int id)
        {
            AuktionModel auktion = await service.GetAuktionAsync(id);
            if (auktion is null) return null;

            return new AuktionBudViewModel(
                auktion,
                await service.ListBudsAsync(auktion.AuktionID)
            );
        }
    }
}