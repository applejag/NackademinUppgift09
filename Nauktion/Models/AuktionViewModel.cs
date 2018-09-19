using System;
using System.ComponentModel.DataAnnotations;

namespace Nauktion.Models
{
    public class AuktionViewModel
    {
        public int AuktionID { get; set; }

        [Required(ErrorMessage = "Du måste ange titeln.")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Titeln måste vara mellan 1 och 50 karaktärer.")]
        public string Titel { get; set; }

        [Required(ErrorMessage = "Du måste ange en beskrivning.")]
        public string Beskrivning { get; set; }

        [Required(ErrorMessage = "Du måste ange ett start datum.")]
        public DateTime StartDatum { get; set; }

        [Required(ErrorMessage = "Du måste ange ett avslutande datum.")]
        public DateTime SlutDatum { get; set; }

        [Required(ErrorMessage = "Du måste ange ett utropspris.")]
        [Range(0, int.MaxValue, ErrorMessage = "Utropspriset måste vara positivt.")]
        public int Utropspris { get; set; }
    }
}