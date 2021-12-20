using MobileAppMtChalet.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MobileAppMtChalet.Services {
    public class MockDataStore : IDataStore<Item> {
        readonly List<Item> items;

        public MockDataStore() {
            items = new List<Item>()
            {
                new Item { Id = Guid.NewGuid().ToString(), Text = "First item", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Second item", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Third item", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Fourth item", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Fifth item", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Sixth item", Description="This is an item description." }
            };
        }

        public async Task<bool> AddItemAsync(Item item) {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item) {
            var oldItem = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id) {
            var oldItem = items.Where((Item arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string id) {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false) {
            return await Task.FromResult(items);
        }

        public async Task<IEnumerable<Item>> GetItemsAsync2(string date, bool forceRefresh = false) {
            
            HttpClient client = new HttpClient();

            date = date.Replace("/", string.Empty);
            date = date.Remove(8);
            //var json = new WebClient().DownloadString("http://localhost:13122/api/WeatherForecast/" + date);
            string json = "";
            try {
                HttpResponseMessage response = await client.GetAsync("http://localhost:13122/api/WeatherForecast/" + date);
            response.EnsureSuccessStatusCode();
            json = await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e) {
                string test = e.Message;
            }

            List<Reservation> reservations = JsonConvert.DeserializeObject<List<Reservation>>(json);

            foreach (Item x in items) {
                x.Text += "inny";
            }
            return await Task.FromResult(items);
        }
    }
}