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
        [NotNull]
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

        public async Task<List<AuktionModel>> ListAuktionsAsync()
        {
            int gruppkod = Gruppkod;
            HttpResponseMessage response = await _client.GetAsync($"Auktion/{gruppkod}");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<List<AuktionModel>>();
        }

        public async Task<AuktionModel> GetAuktionAsync(int id)
        {
            int gruppkod = Gruppkod;
            HttpResponseMessage response = await _client.GetAsync($"Auktion/{gruppkod}/{id}");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<AuktionModel>();
        }
    }
}