using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Nauktion.Models
{
    public class SearchViewModel
    {
        //[BindProperty(Name = "q")]
        [Display(Name = "Sökfält", Prompt = "Sök här...")]
        [StringLength(250, ErrorMessage = "Ange högst 250 karaktärer i textfältet.")]
        public string Query { get; set; }

        //[BindProperty(Name = "o")]
        [Display(Name = "Visa öppna auktioner?")]
        public bool ShowOpen { get; set; } = true;

        //[BindProperty(Name = "c")]
        [Display(Name = "Visa stängda auktioner?")]
        public bool ShowClosed { get; set; } = false;

        //[BindProperty(Name = "s")]
        [Display(Name = "Sortera efter...")]
        public SortingMode SortBy { get; set; } = SortingMode.DateDesc;

        public bool Matches(AuktionModel auktion)
        {
            bool isClosed = auktion.IsClosed();
            if (isClosed && !ShowClosed)
                return false;
            if (!isClosed && !ShowOpen)
                return false;

            if (!string.IsNullOrWhiteSpace(Query))
            {
                Query = Query.Trim();

                if (auktion.Titel.IndexOf(Query, StringComparison.CurrentCultureIgnoreCase) == -1
                    && auktion.Beskrivning.IndexOf(Query, StringComparison.CurrentCultureIgnoreCase) == -1)
                    return false;
            }

            return true;
        }

        public IComparer<AuktionBudViewModel> GetSortingComparer()
        {
            return new AuktionSorter(SortBy);
        }


        public enum SortingMode
        {
            [Display(Name = "Pris, lägst till högst")]
            PriceAsc,

            [Display(Name = "Pris, högst till lägst")]
            PriceDesc,

            [Display(Name = "Datum, äldst först")]
            DateAsc,

            [Display(Name = "Datum, nyast först")]
            DateDesc,
        }

        private class AuktionSorter : IComparer<AuktionBudViewModel>
        {
            private readonly SortingMode _sortBy;

            public AuktionSorter(SortingMode sortBy)
            {
                _sortBy = sortBy;
            }

            public int Compare(AuktionBudViewModel x, AuktionBudViewModel y)
            {
                switch (_sortBy)
                {
                    case SortingMode.DateAsc:
                        return x.StartDatum.CompareTo(y.StartDatum);

                    case SortingMode.DateDesc:
                        return y.StartDatum.CompareTo(x.StartDatum);

                    case SortingMode.PriceAsc:
                        return x.MaxedPrice.CompareTo(y.MaxedPrice);

                    case SortingMode.PriceDesc:
                        return y.MaxedPrice.CompareTo(x.MaxedPrice);
                }

                return 0;
            }
        }
    }
}