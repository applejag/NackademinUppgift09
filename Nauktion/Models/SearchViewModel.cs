using System;
using System.ComponentModel.DataAnnotations;
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
    }
}