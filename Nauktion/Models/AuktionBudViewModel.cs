using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Nauktion.Helpers;

namespace Nauktion.Models
{
    public class AuktionBudViewModel
    {
        public AuktionBudViewModel([NotNull] AuktionModel auktion, 
            [NotNull, ItemNotNull] List<BudModel> bud)
        {
            AuktionID = auktion.AuktionID;
            Titel = auktion.Titel;
            Beskrivning = auktion.Beskrivning;
            StartDatum = auktion.StartDatum;
            SlutDatum = auktion.SlutDatum;
            Utropspris = auktion.Utropspris ?? 0;
            SkapadAv = auktion.SkapadAv;
            Gruppkod = auktion.Gruppkod;

            Bids = bud.Select(b => new BudViewModel(b)).ToList();
            HighestBid = Bids.Count == 0
                ? Utropspris : Bids.Max(b => b.Summa);

            DateTime now = DateTime.Now;
            TimeUntilEnd = SlutDatum - now;
            TimeUntilEndFormatted = DateTimeHelpers.FormatRemainingTime(SlutDatum);
            TimeSinceStart = now - StartDatum;
            TimeSinceStartFormatted = DateTimeHelpers.FormatRemainingTime(StartDatum);
            IsClosed = SlutDatum < now;
        }

        public int AuktionID { get; }
        public string Titel { get; }
        public string Beskrivning { get; }
        public DateTime StartDatum { get; }
        public DateTime SlutDatum { get; }
        public int Utropspris { get; }
        public string SkapadAv { get; }
        public int Gruppkod { get; }

        public List<BudViewModel> Bids { get; }
        public int HighestBid { get; }

        public TimeSpan TimeUntilEnd { get; }
        public string TimeUntilEndFormatted { get; }
        public TimeSpan TimeSinceStart { get; }
        public string TimeSinceStartFormatted { get; }
        public bool IsClosed { get; }

        public class BudViewModel
        {
            public BudViewModel(BudModel bud)
            {
                BudID = bud.BudID;
                AuktionID = bud.AuktionID;
                Summa = bud.Summa;
                Budgivare = bud.Budgivare;
            }

            public int BudID { get; }
            public int AuktionID { get; }
            public int Summa { get; }
            public string Budgivare { get; }
        }
    }
}
