using System;
using Nauktion.Helpers;

namespace Nauktion.Models
{
    public class AuktionModel
    {
        public int AuktionID { get; set; }
        public string Titel { get; set; }
        public string Beskrivning { get; set; }
        public DateTime StartDatum { get; set; }
        public DateTime SlutDatum { get; set; }
        public int? Utropspris { get; set; }
        public string SkapadAv { get; set; }
        public int Gruppkod { get; set; }

        public bool IsClosed()
        {
            return SlutDatum < DateTime.Now;
        }
    }
}