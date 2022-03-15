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

            var response = await _httpClient.PostAsync("reservations", httpContent);
        }

        public async Task DeleteReservation(int reservationID) {

            var response = await _httpClient.DeleteAsync($"reservations/{reservationID}");
            response.EnsureSuccessStatusCode();

        }

        //TODO REWORK THIS
        public async Task EditReservation(EditedReservation reservation) {
            var updatedSegments = JsonConvert.SerializeObject(reservation).Trim('\\');

            var httpContent = new StringContent(updatedSegments, Encoding.UTF8, "application/json");


            var response = await _httpClient.PostAsync($"reservations/edit", httpContent);

            response.EnsureSuccessStatusCode();
        }

        public async Task<Reservation> GetReservation(string id) {
            var response = await _httpClient.GetAsync(@"reservations/id/" + id);
            response.EnsureSuccessStatusCode();

            var responseAsString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Reservation>(responseAsString);
        }

        public async Task<IEnumerable<Reservation>> GetReservationOnDate(string date) {
            var response = await _httpClient.GetAsync("reservations/date/" + date);
            response.EnsureSuccessStatusCode();

            var responseAsString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Reservation>>(responseAsString);
        }

        public async Task<IEnumerable<Room>> GetRooms() {
            var response = await _httpClient.GetAsync(@"rooms");
            response.EnsureSuccessStatusCode();

            var responseAsString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Room>>(responseAsString);
        }
    }
}
