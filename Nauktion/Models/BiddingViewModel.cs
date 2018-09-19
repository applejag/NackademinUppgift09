using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace Nauktion.Models
{
    public class BiddingViewModel
    {
        public int AuktionID { get; set; }

        [Remote(controller: "AuktionAPI", action: "VerifyBudSumma", AdditionalFields = nameof(AuktionID))]
        [Range(0, int.MaxValue, ErrorMessage = "Budet måste vara positivt!")]
        public int Summa { get; set; }

        [NotMapped]
        public bool Disabled { get; set; }
    }
}