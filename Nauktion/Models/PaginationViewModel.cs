using System;
using System.Collections.Generic;
using Nauktion.Helpers;

namespace Nauktion.Models
{
    public class PaginationViewModel
    {
        public const int MODELS_PER_PAGE = 8;

        public object Model { get; set; }
        public int Page { get; set; }
        public int NumOfPages { get; set; }
        public int TotalModelCount { get; set; }

        public int StartIndex => (Page - 1) * MODELS_PER_PAGE;
        public int StopIndex => Math.Min(Page * MODELS_PER_PAGE - 1, TotalModelCount - 1);

        public PaginationViewModel()
        {
        }

        public PaginationViewModel(ref int page, int totalCount, object model = null)
        {
            NumOfPages = MathHelpers.DivCeil(totalCount, MODELS_PER_PAGE);
            TotalModelCount = totalCount;
            Page = page = Math.Max(Math.Min(page, NumOfPages), 1);
            Model = model;
        }

        public PaginationViewModel GetWithoutModel()
        {
            return new PaginationViewModel
            {
                Page = Page,
                NumOfPages = NumOfPages,
                TotalModelCount = TotalModelCount
            };
        }

        public PaginationViewModel GetWithOtherModel(object model)
        {
            return new PaginationViewModel
            {
                Page = Page,
                NumOfPages = NumOfPages,
                TotalModelCount = TotalModelCount,
                Model = model
            };
        }
    }
}