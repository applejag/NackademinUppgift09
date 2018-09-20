using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Nauktion.Helpers;

namespace Nauktion.Models
{
    public class StatisticsViewModel
    {
        public StatisticsFilterModel Filter { get; set; }

        public List<AuktionBudViewModel> Auktions { get; set; }
    }
}