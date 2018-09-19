using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Nauktion.Models;

namespace Nauktion.Repositories
{
    public class AuktionRepository : IAuktionRepository
    {
        public Uri BaseAddress { get; } = new Uri(@"http://nackowskis.azurewebsites.net/api/");

        public int Gruppkod { get; } = 1;

        private readonly HttpClient _client;

        public AuktionRepository()
        {
            _client = new HttpClient {BaseAddress = BaseAddress};
        }

        ~AuktionRepository()
        {
            _client?.Dispose();
        }

        public async Task<List<BudModel>> ListBudsAsync(int auktionID)
        {
            HttpResponseMessage response = await _client.GetAsync($"bud/{Gruppkod}/{auktionID}");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<List<BudModel>>()
                ?? new List<BudModel>();
        }

        public async Task CreateBudAsync(BudModel model)
        {
            var formdata = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                [nameof(model.AuktionID)] = model.AuktionID.ToString(),
                [nameof(model.Budgivare)] = model.Budgivare,
                [nameof(model.Summa)] = model.Summa.ToString()
            });

            HttpResponseMessage response = await _client.PostAsync("bud", formdata);

            response.EnsureSuccessStatusCode();
        }

        public async Task CreateAuktionAsync(AuktionModel model)
        {
            var formdata = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                [nameof(model.Titel)] = model.Titel,
                [nameof(model.Beskrivning)] = model.Beskrivning,
                [nameof(model.StartDatum)] = model.StartDatum.ToString("u"),
                [nameof(model.SlutDatum)] = model.SlutDatum.ToString("u"),
                [nameof(model.Utropspris)] = model.Utropspris?.ToString() ?? "0",
                [nameof(model.Gruppkod)] = model.Gruppkod.ToString(),
                [nameof(model.SkapadAv)] = model.SkapadAv
            });

            HttpResponseMessage response = await _client.PostAsync("Auktion", formdata);

            response.EnsureSuccessStatusCode();
        }

        public async Task AlterAuktionAsync(AuktionModel model)
        {
            var formdata = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                [nameof(model.AuktionID)] = model.AuktionID.ToString(),
                [nameof(model.Titel)] = model.Titel,
                [nameof(model.Beskrivning)] = model.Beskrivning,
                [nameof(model.StartDatum)] = model.StartDatum.ToString("u"),
                [nameof(model.SlutDatum)] = model.SlutDatum.ToString("u"),
                [nameof(model.Utropspris)] = model.Utropspris?.ToString() ?? "0",
                [nameof(model.Gruppkod)] = model.Gruppkod.ToString(),
                [nameof(model.SkapadAv)] = model.SkapadAv
            });

            HttpResponseMessage response = await _client.PutAsync("Auktion", formdata);

            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAuktionAsync(int auktionID)
        {
            HttpResponseMessage response = await _client.DeleteAsync($"Auktion/{Gruppkod}/{auktionID}");

            response.EnsureSuccessStatusCode();
        }

        public async Task<List<AuktionModel>> ListAuktionsAsync()
        {
            HttpResponseMessage response = await _client.GetAsync($"Auktion/{Gruppkod}");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<List<AuktionModel>>()
                ?? new List<AuktionModel>();
        }

        public async Task<AuktionModel> GetAuktionAsync(int auktionID)
        {
            HttpResponseMessage response = await _client.GetAsync($"Auktion/{Gruppkod}/{auktionID}");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<AuktionModel>();
        }
    }
}