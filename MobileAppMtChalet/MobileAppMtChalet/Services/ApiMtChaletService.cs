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
        public async Task AddReservation(Reservation reservation) {
            throw new NotImplementedException();
        }

        public Task DeleteReservation(Reservation reservation) {
            throw new NotImplementedException();
        }

        public Task EditReservation(Reservation oldReservation, Reservation newReservation) {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Reservation>> GetReservationOnDate(string date) {
            var response = await _httpClient.GetAsync("MtChalet/" + date);
            response.EnsureSuccessStatusCode();

            var responseAsString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Reservation>>(responseAsString);
        }

        public async Task<IEnumerable<Room>> GetRooms() {
            var response = await _httpClient.GetAsync(@"MtChalet/rooms");
            response.EnsureSuccessStatusCode();

            var responseAsString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Room>>(responseAsString);
        }
    }
}
