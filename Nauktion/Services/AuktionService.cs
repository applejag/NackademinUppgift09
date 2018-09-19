using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nauktion.Helpers;
using Nauktion.Models;
using Nauktion.Repositories;

namespace Nauktion.Services
{
    public class AuktionService : IAuktionService
    {
        private readonly IAuktionRepository _repository;
        private readonly UserManager<NauktionUser> _userManager;

        public AuktionService(IAuktionRepository repository, 
            UserManager<NauktionUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public async Task<List<AuktionModel>> ListAuktionsAsync(bool includeClosed = false)
        {
            List<AuktionModel> list = await _repository.ListAuktionsAsync();
            return list.WhereIf(!includeClosed, a => !a.IsClosed())
                .OrderBy(a => a.StartDatum)
                .ToList();
        }

        public async Task<AuktionModel> GetAuktionAsync(int auktionID)
        {
            return await _repository.GetAuktionAsync(auktionID);
        }

        public async Task<List<BudModel>> ListBudsAsync(int auktionID)
        {
            List<BudModel> list = await _repository.ListBudsAsync(auktionID);
            return list.OrderByDescending(b => b.Summa)
                .ToList();
        }

        public async Task CreateBudAsync(BiddingViewModel model, NauktionUser budgivare)
        {
            await _repository.CreateBudAsync(new BudModel
            {
                AuktionID = model.AuktionID,
                Summa = model.Summa,
                Budgivare = budgivare.Id
            });
        }

        public async Task<string> ValidateBud(int auktionID, int summa, NauktionUser budgivare)
        {
            AuktionModel auktion = await GetAuktionAsync(auktionID);

            if (auktion is null)
                return "Auktionen finns inte.";

            if (auktion.IsClosed())
                return "Du kan inte buda på en stängd auktion.";

            if (summa <= auktion.Utropspris)
                return "Budet måste vara större än utropspriset.";
            
            List<BudModel> budModels = await ListBudsAsync(auktionID);
            BudModel highestBid = budModels.FirstOrDefault();

            if (highestBid is null)
                return null;

            if (summa <= highestBid.Summa)
                return "Budet måste vara större än det högsta budet.";

            NauktionUser highestBidder = await _userManager.FindByIdAsync(highestBid.Budgivare);
            if (highestBidder?.Id == budgivare.Id)
            {
                return  "Du kan inte buda när du har högsta budet.";
            }

            return null;
        }

        public async Task CreateAuktionAsync(AuktionViewModel model, NauktionUser skapare)
        {
            await _repository.CreateAuktionAsync(new AuktionModel
            {
                Titel = model.Titel,
                Beskrivning = model.Beskrivning,
                StartDatum = model.StartDatum,
                SlutDatum = model.SlutDatum,
                Gruppkod = _repository.Gruppkod,
                Utropspris = model.Utropspris,
                SkapadAv = skapare.Id
            });
        }

        public async Task AlterAuktionAsync(AuktionViewModel model, NauktionUser skapare)
        {
            await _repository.AlterAuktionAsync(new AuktionModel
            {
                AuktionID = model.AuktionID,
                Titel = model.Titel,
                Beskrivning = model.Beskrivning,
                StartDatum = model.StartDatum,
                SlutDatum = model.SlutDatum,
                Gruppkod = _repository.Gruppkod,
                Utropspris = model.Utropspris,
                SkapadAv = skapare.Id
            });
        }

        public async Task DeleteAuktionAsync(int auktionID)
        {
            await _repository.DeleteAuktionAsync(auktionID);
        }
    }
}