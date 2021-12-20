using MobileAppMtChalet.Models;
using MobileAppMtChalet.Services;
using MobileAppMtChalet.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace MobileAppMtChalet.ViewModels {
    public class ItemsViewModel : BaseViewModel {
        private Item _selectedItem;

        public ObservableCollection<Item> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<Item> ItemTapped { get; }
        public AsyncCommand<DatePicker> DateSelected { get; }
        private readonly IMtChaletService _mtChaletService;

        public ItemsViewModel(IMtChaletService mtChaletService) {
            _mtChaletService = mtChaletService;
            Title = "Browse";
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<Item>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);


            DateSelected = new AsyncCommand<DatePicker>(GetReservationsOnDate);
        }

        async Task ExecuteLoadItemsCommand() {
            IsBusy = true;
            try {
                Items.Clear();
                var items = await DataStore.GetItemsAsync();
                foreach (var item in items) {
                    Items.Add(item);
                }
            }
            catch (Exception ex) {
                Debug.WriteLine(ex);
            }
            finally {
                IsBusy = false;
            }
 
        }

        async Task GetReservationsOnDate(DatePicker calendar) {
            await Application.Current.MainPage.DisplayAlert("eyooo", "egggg", "okeyyy");
        }


        public void OnAppearing() {
            IsBusy = true;
            SelectedItem = null;
        }

        public Item SelectedItem {
            get => _selectedItem;
            set {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }
        
        private string _selectedDate;
        public string SelectedDate {
            get {
                return _selectedDate;
            }
            set {
                string temp = value;
                temp = temp.Replace("/", string.Empty);
                temp = temp.Remove(8);
                _selectedDate = temp;
                test();
            }
        }

        //Tutaj zrobic pobieranie itemków
        private async void test() {
            IsBusy = true;
            try {
                Items.Clear();
                var test = await _mtChaletService.GetReservationOnDate(_selectedDate);
                var items = await DataStore.GetItemsAsync2(_selectedDate);
                foreach (var item in items) {
                    Items.Add(item);
                }
            }
            catch (Exception ex) {
                Debug.WriteLine(ex);
            }
            finally {
                IsBusy = false;
            }
        }

        private async void OnAddItem(object obj) {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        async void OnItemSelected(Item item) {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
        }
    }
}