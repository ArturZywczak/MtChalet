using MobileAppMtChalet.Models;
using MobileAppMtChalet.Services;
using MobileAppMtChalet.Views;
using System;
using System.Collections.Generic;
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
        public ObservableCollection<Models.Grouping<ReservationsByRoom, Reservation>> ReservationsList { get; set; }
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
            ReservationsList = new ObservableCollection<Models.Grouping<ReservationsByRoom, Reservation>>();
        }

        private void SetupRoomData(IEnumerable<Reservation> reservations, IEnumerable<Room> rooms) {

            foreach(var room in rooms) {
                var asset = new ReservationsByRoom();
                asset.Room = room;
                
                foreach(var reservation in reservations) {
                    if (reservation.RoomId == room.RoomId) asset.Reservations.Add(reservation);
                }

                var group = new Models.Grouping<ReservationsByRoom, Reservation>(asset, asset.Reservations);
                ReservationsList.Add(group);
            }

        }

        async Task ExecuteLoadReservationsCommand() {
            try {
                ReservationsList.Clear();
                var reservations = await _mtChaletService.GetReservationOnDate(_selectedDate);
                var rooms = await _mtChaletService.GetRooms();
                SetupRoomData(reservations,rooms);
            }
            catch (Exception ex) {
                Debug.WriteLine(ex);
            }
            finally {
                IsBusy = false;
            }
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