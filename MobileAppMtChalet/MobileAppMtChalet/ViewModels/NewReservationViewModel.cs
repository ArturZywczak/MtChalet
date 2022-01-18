using MobileAppMtChalet.Models;
using MobileAppMtChalet.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileAppMtChalet.ViewModels {
    class NewReservationViewModel : BaseViewModel {
        #region Private & Public Binding
        private readonly IMtChaletService _mtChaletService;
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
                    RoomChoosed = true;
                    RoomChoosedInv = false;
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
        private bool roomChoosed;
        public bool RoomChoosed {
            get => roomChoosed;
            set => SetProperty(ref roomChoosed, value);
        }
        private bool roomChoosedInv;
        public bool RoomChoosedInv {
            get => roomChoosedInv;
            set => SetProperty(ref roomChoosedInv, value);
        }
        #endregion

        #region  Public Commands & Lists
        public List<Room> Rooms { get; set; }
        public ObservableCollection<int> AvaliableBeds { get; }
        public ObservableCollection<int> RoomIDs { get; }
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        #endregion

        public NewReservationViewModel(IMtChaletService mtChaletService) {
            _mtChaletService = mtChaletService;
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
            startingDate = DateTime.Now;
            endingDate = DateTime.Now;
            roomChoosed = false;
            roomChoosedInv = true;
            AvaliableBeds = new ObservableCollection<int>();
            RoomIDs = new ObservableCollection<int>();
            Rooms = new List<Room>();
            RoomID = -1;
            PrepareRoomInfo();
        }

        #region Comamnd functions
        private bool ValidateSave() {
            return !String.IsNullOrWhiteSpace(surname)
                //&& RoomExists(roomID).Result
                && numberOfPeople > 0
                && startingDate > DateTime.Now
                && endingDate > startingDate
                ;
        }
        private async void OnCancel() {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
        private async void OnSave() {
            Reservation newReservation = new Reservation() {
                Name = name,
                Surname = surname,
                RoomId = roomID,
                NumberOfPeople = numberOfPeople,
                StartingDate = startingDate,
                EndingDate = endingDate,
                Phone = phone,
                Email = email,
                ExtraInfo = extraInfo,
                EmployeeId = 0,
                CreationDate = DateTime.Now

            };

            await _mtChaletService.AddReservation(newReservation);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
        #endregion

        #region Other functions
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
        private void ChangeSelectedRoom() {
            AvaliableBeds.Clear();
            for (int i = 1; i <= Rooms[roomID].RoomCap; i++) AvaliableBeds.Add(i);
        }
        #endregion
    }
}
