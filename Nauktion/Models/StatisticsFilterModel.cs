using System;
using System.ComponentModel.DataAnnotations;

namespace Nauktion.Models
{
    public class StatisticsFilterModel
    {
        [Display(Name = "Visa dina auktioner.")]
        public bool ShowMine { get; set; } = true;

        [Display(Name = "Visa andras auktioner.")]
        public bool ShowOthers { get; set; } = true;
        
        [Display(Name = "Årtal")]
        [Required]
        public int TimeYear { get; set; } = DateTime.Today.Year;

        [Display(Name = "Månad")]
        [Required]
        public Month TimeMonth { get; set; } = (Month) DateTime.Today.Month;

        public enum Month
        {
            [Display(Name = "Januari")] January,
            [Display(Name = "Februari")] February,
            [Display(Name = "Mars")] March,
            [Display(Name = "April")] April,
            [Display(Name = "Maj")] May,
            [Display(Name = "Juni")] June,
            [Display(Name = "Juli")] July,
            [Display(Name = "Augusti")] August,
            [Display(Name = "September")] September,
            [Display(Name = "Oktober")] October,
            [Display(Name = "November")] November,
            [Display(Name = "December")] December
        }
    }
}