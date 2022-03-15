using MobileAppMtChalet.Models;
using MobileAppMtChalet.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileAppMtChalet.ViewModels {
    [QueryProperty(nameof(ReservationId), nameof(ReservationId))]
    [QueryProperty(nameof(UserID), "UserID")]
    class ReservationDetailViewModel : BaseViewModel {
        #region Private & Public for binding values
        private readonly IMtChaletService _mtChaletService;
        private string reservationId;
        public string ReservationId {
            get {
                return reservationId;
            }
            set {
                reservationId = value;
                LoadReservationId(value);
            }
        }
        private string name;
        public string Name {
            get => name;
            set => SetProperty(ref name, value);
        }
        private string surname;
        public string Surname {
            get => surname;
            set => SetProperty(ref surname, value);
        }
        private int roomID;
        public int RoomID {
            get => roomID;
            set {
                SetProperty(ref roomID, value);
                if (roomID != -1) {
                    ChangeSelectedRoom();
                }
            }
        }
        private int numberOfPeople;
        public int NumberOfPeople {
            get => numberOfPeople;
            set => SetProperty(ref numberOfPeople, value);
        }
        private DateTime startingDate;
        public DateTime StartingDate {
            get => startingDate;
            set => SetProperty(ref startingDate, value);
        }
        private DateTime endingDate;
        public DateTime EndingDate {
            get => endingDate;
            set => SetProperty(ref endingDate, value);
        }
        private string phone;
        public string Phone {
            get => phone;
            set => SetProperty(ref phone, value);
        }
        private string email;
        public string Email {
            get => email;
            set => SetProperty(ref email, value);
        }
        private string extraInfo;
        public string ExtraInfo {
            get => extraInfo;
            set => SetProperty(ref extraInfo, value);
        }
        private int employeeID;
        private DateTime creationDate;
        public DateTime CreationDate {
            get => creationDate;
            set => SetProperty(ref creationDate, value);
        }
        private bool editMode;
        public bool EditMode {
            get => editMode;
            set {
                SetProperty(ref editMode, value);
                SelectedRoomID = RoomID - 1;
                SelectedBedCountID = NumberOfPeople - 1;
                EditModeInversed = !value;
                OnPropertyChanged();
            }
        }
        private bool editModeInv;
        public bool EditModeInversed {
            get => editModeInv;
            set {
                SetProperty(ref editModeInv, value);
                OnPropertyChanged();
            }
        }
        private string userID;
        public string UserID {
            get {
                return userID;
            }
            set {
                SetProperty(ref userID, value);
                IsLogged = (value.Length != 0);
            }
        }
        private string oldUserID;
        public string OldUserID {
            get {
                return oldUserID;
            }
            set {
                SetProperty(ref oldUserID, value);
            }
        }
        private bool isLogged;
        public bool IsLogged { 
            get => isLogged;
            set => SetProperty(ref isLogged, value);
        }
        //terrible hack, bug fix
        private int selectedRoomID;
        public int SelectedRoomID {
            get => selectedRoomID;
            set { 
                SetProperty(ref selectedRoomID, value);
                if (roomID != -1) {
                    ChangeSelectedRoom();
                }
            }
        }

        private int selectedBedCountID;
        public int SelectedBedCountID {
            get => selectedBedCountID;
            set => SetProperty(ref selectedBedCountID, value);
        }
        #endregion
        # region Public Lists & Commands
        public ObservableCollection<int> RoomIDs { get; }
        public List<Room> Rooms { get; set; }
        public ObservableCollection<int> AvaliableBeds { get; }
        public Command EditReservationCommand { get; }
        public Command SaveEditCommand { get; }
        public Command CancelCommand { get; }
        public Command DeleteCommand { get; }
        #endregion
        public ReservationDetailViewModel(IMtChaletService mtChaletService) {

            RoomID = -1;
            IsLogged = false;
            _mtChaletService = mtChaletService;
            SaveEditCommand = new Command(OnSave);
            CancelCommand = new Command(OnCancel);
            EditReservationCommand = new Command(OnEditReservation);
            DeleteCommand = new Command(OnDelete);
            EditMode = false;
            EditModeInversed = true;
            RoomIDs = new ObservableCollection<int>();
            Rooms = new List<Room>();
            AvaliableBeds = new ObservableCollection<int>();
            PrepareRoomInfo();
        }


        #region Commands Functions
        private async void OnCancel() {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
        private async void OnSave() { //TODO zamiast rezerwacji ma przygotować ReservationsEditHistory(zmienić nazwe?, ma dac stare id i dane pracownika)

            EditedReservation editedReservation = new EditedReservation() {
                OldReservationId = Int32.Parse(ReservationId),
                Name = name,
                Surname = surname,
                RoomId = SelectedRoomID + 1,
                NumberOfPeople = selectedBedCountID + 1,
                StartingDate = startingDate,
                EndingDate = endingDate,
                Phone = phone,
                Email = email,
                ExtraInfo = extraInfo,
                EmployeeId = oldUserID,
                CreationDate = creationDate,
                EditDate = DateTime.Now,
                EditedByEmployeeId = userID
                };

            await _mtChaletService.EditReservation(editedReservation);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
        private void OnEditReservation(object obj) {
            editMode = true;
            editModeInv = false;
        }
        private async void OnDelete() {

            await _mtChaletService.DeleteReservation(Int32.Parse(ReservationId));
            await Shell.Current.GoToAsync("..");
        }
        #endregion
        #region Other Functions
        private void ChangeSelectedRoom() {
            AvaliableBeds.Clear();
            if (editMode)
            for (int i = 1; i <= Rooms[selectedRoomID].RoomCap; i++) AvaliableBeds.Add(i); //here
            else
                for (int i = 1; i <= Rooms[roomID-1].RoomCap; i++) AvaliableBeds.Add(i); //here
        }
        async void PrepareRoomInfo() {
            try {
                var rooms = await _mtChaletService.GetRooms();
                foreach (var room in rooms) {
                    RoomIDs.Add(room.RoomId);
                    Rooms.Add(room);
                }
            }
            catch (Exception ex) {
                Debug.WriteLine(ex);
            }
        }
        public async void LoadReservationId(string reservationId) {
            try {
                var reservation = await _mtChaletService.GetReservation(reservationId);
                Name = reservation.Name;
                Surname = reservation.Surname;
                RoomID = reservation.RoomId;
                NumberOfPeople = reservation.NumberOfPeople;
                StartingDate = reservation.StartingDate;
                EndingDate = reservation.EndingDate;
                Phone = reservation.Phone;
                Email = reservation.Email;
                ExtraInfo = reservation.ExtraInfo;
                CreationDate = reservation.CreationDate;
            }
            catch (Exception) {
                Debug.WriteLine("Failed to Load Item");
            }
        }
        #endregion
    }
}
