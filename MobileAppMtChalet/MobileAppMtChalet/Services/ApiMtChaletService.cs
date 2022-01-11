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
            // Serialize our concrete class into a JSON String
            var stringPayload = JsonConvert.SerializeObject(reservation).Trim('\\');
            
            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("MtChalet", httpContent);
            //throw new NotImplementedException();
        }

        public Task DeleteReservation(Reservation reservation) {
            throw new NotImplementedException();
        }

        public Task EditReservation(Reservation oldReservation, Reservation newReservation) {
            throw new NotImplementedException();
        }

        public async Task<Reservation> GetReservation(string id) {
            var response = await _httpClient.GetAsync(@"MtChalet/id/" + id);
            response.EnsureSuccessStatusCode();

            var responseAsString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Reservation>(responseAsString);
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
