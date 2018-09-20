﻿using System;
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

            var viewModel = new AuktionBudPageinatedViewModel(ref page, auktions);

            foreach (AuktionModel a in auktions
                .Skip(viewModel.StartIndex)
                .Take(AuktionBudPageinatedViewModel.MODELS_PER_PAGE))
            {
                List<BudModel> buds = await service.ListBudsAsync(a.AuktionID);
                viewModel.Models.Add(new AuktionBudViewModel(a, buds));
            }

            return viewModel;
        }

        [NotNull, ItemNotNull]
        public static async Task<SearchResultsViewModel> SearchAuktionBudsPaginatedAsync(this IAuktionService service, SearchViewModel search, int page = 1)
        {
            List<AuktionModel> auktions = (await service.ListAuktionsAsync(true))
                .Where(search.Matches).ToList();

            var viewModel = new AuktionBudPageinatedViewModel(ref page, auktions);

            foreach (AuktionModel a in auktions)
            {
                List<BudModel> buds = await service.ListBudsAsync(a.AuktionID);
                viewModel.Models.Add(new AuktionBudViewModel(a, buds));
            }

            viewModel.Models = viewModel.Models
                .OrderBy(a => a, search.GetSortingComparer())
                .Skip(viewModel.StartIndex)
                .Take(AuktionBudPageinatedViewModel.MODELS_PER_PAGE)
                .ToList();

            return new SearchResultsViewModel
            {
                AuktionModel = viewModel,
                SearchModel = search
            };
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