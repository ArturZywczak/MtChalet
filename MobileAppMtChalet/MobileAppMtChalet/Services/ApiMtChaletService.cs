using MobileAppMtChalet.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MobileAppMtChalet.Services {
    class ApiMtChaletService : IMtChaletService {
        private readonly HttpClient _httpClient;
        public ApiMtChaletService(HttpClient httpClient) {
            _httpClient = httpClient;
        }
        public async Task AddReservation(Reservations reservation) {
            throw new NotImplementedException();
        }

        public Task DeleteReservation(Reservations reservation) {
            throw new NotImplementedException();
        }

        public Task EditReservation(Reservations oldReservation, Reservations newReservation) {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Reservations>> GetReservationOnDate(string date) {
            var response = await _httpClient.GetAsync("WeatherForecast/" + date);
            response.EnsureSuccessStatusCode();

            var responseAsString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Reservations>>(responseAsString);
        }
    }
}
