using Otus.Teaching.Pcf.ReceivingFromPartner.Core.Abstractions.Gateways;
using Otus.Teaching.Pcf.ReceivingFromPartner.Core.Domain;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Otus.Teaching.Pcf.ReceivingFromPartner.Integration
{
    public class PreferencesFromDistributedCache : IPreferencesFromDistributedCache
    {
        private readonly HttpClient _httpClient;

        public PreferencesFromDistributedCache(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Preference>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("api/v1/preferences");

            response.EnsureSuccessStatusCode();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var res = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Preference>>(res, options);
        }

        public async Task<Preference> GetByIdAsync(Guid preferenceId)
        {
            var response = await _httpClient.GetStringAsync($"api/v1/promocodes/{preferenceId}");

            return JsonSerializer.Deserialize<Preference>(response);
        }
    }
}
