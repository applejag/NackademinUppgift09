using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Nauktion.Models;

namespace Nauktion.Repositories
{
    public class AuktionRepository : IAuktionRepository
    {
        private readonly HttpClient _client;

        public AuktionRepository()
        {
            _client = new HttpClient {BaseAddress = GetBaseAddress()};
        }

        ~AuktionRepository()
        {
            _client?.Dispose();
        }

        public Uri GetBaseAddress()
        {
            return new Uri(@"http://nackowskis.azurewebsites.net/api/");
        }

        public async Task<List<AuktionModel>> ListAuktions(int gruppkod)
        {
            HttpResponseMessage response = await _client.GetAsync($"Auktion/{gruppkod}");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<List<AuktionModel>>();
        }

        public async Task<AuktionModel> GetAuktion(int gruppkod, int id)
        {
            HttpResponseMessage response = await _client.GetAsync($"Auktion/{gruppkod}/{id}");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<AuktionModel>();
        }
    }
}