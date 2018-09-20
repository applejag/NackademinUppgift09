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
        public static async Task<PaginationViewModel> ListAuktionBudsPaginatedAsync(this IAuktionService service, int page = 1, bool includeClosed = false)
        {
            List<AuktionModel> auktions = await service.ListAuktionsAsync(includeClosed);
            var auktionBudView = new List<AuktionBudViewModel>();

            var viewModel = new PaginationViewModel(ref page, auktions.Count, auktionBudView);

            // Fill list please
            foreach (AuktionModel a in auktions
                .Skip(viewModel.StartIndex)
                .Take(PaginationViewModel.MODELS_PER_PAGE))
            {
                List<BudModel> buds = await service.ListBudsAsync(a.AuktionID);
                auktionBudView.Add(new AuktionBudViewModel(a, buds));
            }

            return viewModel;
        }

        [NotNull, ItemNotNull]
        public static async Task<PaginationViewModel> SearchAuktionBudsPaginatedAsync(this IAuktionService service, SearchViewModel search, int page = 1)
        {
            // Gather auktions list
            List<AuktionModel> auktions = (await service.ListAuktionsAsync(true))
                .Where(search.Matches).ToList();

            // Gather auktions+buds list
            var auktionBudView = new List<AuktionBudViewModel>();
            foreach (AuktionModel a in auktions)
            {
                List<BudModel> buds = await service.ListBudsAsync(a.AuktionID);
                auktionBudView.Add(new AuktionBudViewModel(a, buds));
            }

            // Order whole list
            auktionBudView.Sort(search.GetSortingComparer());

            // Prepare paginated model
            var pageinated = new PaginationViewModel(ref page, auktions.Count);

            pageinated.Model = new SearchResultsViewModel
            {
                SearchModel = search,
                // Take items for this page
                AuktionModel = auktionBudView
                    .Skip(pageinated.StartIndex)
                    .Take(PaginationViewModel.MODELS_PER_PAGE)
                    .ToList()
            };

            return pageinated;
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