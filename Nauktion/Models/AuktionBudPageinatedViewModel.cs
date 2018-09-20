using System;
using System.Collections.Generic;
using Nauktion.Helpers;

namespace Nauktion.Models
{
    public class AuktionBudPageinatedViewModel : PaginationViewModel
    {
        public List<AuktionBudViewModel> Auktions { get; set; } = new List<AuktionBudViewModel>();

        public AuktionBudPageinatedViewModel(ref int page, int totalCount, object model = null) 
            : base(ref page, totalCount, model)
        {
        }

        public AuktionBudPageinatedViewModel()
        {
        }
    }
}