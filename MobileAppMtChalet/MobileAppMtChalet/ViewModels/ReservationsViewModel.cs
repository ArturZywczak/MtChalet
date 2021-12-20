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
    public class ReservationsViewModel : BaseViewModel {
        private Reservation _selectedReservation;
        private readonly IMtChaletService _mtChaletService;
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
                IsBusy = true;
            }
        }
        public Reservation SelectedReservation {
            get => _selectedReservation;
            set {
                SetProperty(ref _selectedReservation, value);
                OnReservationSelected(value);
            }
        }
        public ObservableCollection<Reservation> Reservations { get; }
        public Command LoadReservationsCommand { get; }
        public Command AddReservationCommand { get; }
        public Command<Reservation> ReservationTapped { get; }
        public AsyncCommand<DatePicker> DateSelected { get; }

        public ReservationsViewModel(IMtChaletService mtChaletService) {

            _mtChaletService = mtChaletService;
            Title = "Rezerwacje";
            Reservations = new ObservableCollection<Reservation>();
            LoadReservationsCommand = new Command(async () => await ExecuteLoadReservationsCommand());
            ReservationTapped = new Command<Reservation>(OnReservationSelected);
            AddReservationCommand = new Command(OnAddReservation);
            DateSelected = new AsyncCommand<DatePicker>(GetReservationsOnDate);

        }

        async Task ExecuteLoadReservationsCommand() {
            try {
                Reservations.Clear();
                var reservations = await _mtChaletService.GetReservationOnDate(_selectedDate);
                foreach (var res in reservations) {
                    Reservations.Add(res);
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
            SelectedReservation = null;
        }

        private async void OnAddReservation(object obj) {
            //TODO dodać dodawanie rezerwacji
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        async void OnReservationSelected(Reservation reservation) {
            if (reservation == null) return;

            //TODO szczegóły rezerwacji
            //await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
        }
    }

}