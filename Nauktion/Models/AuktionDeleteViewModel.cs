using System.ComponentModel.DataAnnotations;

namespace Nauktion.Models
{
    public class AuktionDeleteViewModel
    {
        public int AuktionID { get; set; }

        [Required]
        [Compare(nameof(AuktionID), ErrorMessage = "Var god fyll i rätt kod.")]
        public int? AuktionID_Confirm { get; set; }
    }
}