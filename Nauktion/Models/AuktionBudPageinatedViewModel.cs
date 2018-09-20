using System;
using System.Collections.Generic;
using Nauktion.Helpers;

namespace Nauktion.Models
{
    public class AuktionBudPageinatedViewModel
    {
        public const int MODELS_PER_PAGE = 8;

        public List<AuktionBudViewModel> Models { get; set; } = new List<AuktionBudViewModel>();
        public int Page { get; set; }
        public int NumOfPages { get; set; }
        public int TotalModelCount { get; set; }

        public int StartIndex => (Page - 1) * MODELS_PER_PAGE;
        public int StopIndex => Math.Min(Page * MODELS_PER_PAGE - 1, TotalModelCount - 1);

        public AuktionBudPageinatedViewModel(ref int page, List<AuktionModel> auktions)
        {
            NumOfPages = MathHelpers.DivCeil(auktions.Count, MODELS_PER_PAGE);
            TotalModelCount = auktions.Count;
            Page = page = Math.Clamp(page, 1, NumOfPages);
        }
    }
}