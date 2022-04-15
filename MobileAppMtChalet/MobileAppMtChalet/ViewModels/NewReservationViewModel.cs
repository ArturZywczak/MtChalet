using MobileAppMtChalet.Models;
using MobileAppMtChalet.Services;
using MobileAppMtChalet.Views;
using Newtonsoft.Json;
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
        
        private int roomID;
        public int RoomID {
            get => roomID;
            set {
                if (value != roomID) Unvalidate();
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
            set {
                if (value != numberOfPeople) Unvalidate();
                SetProperty(ref numberOfPeople, value); 
            }
        }
        private DateTime startingDate;
        public DateTime StartingDate {
            get => startingDate;
            set {
                if (value != startingDate) Unvalidate();
                EndingDate = value.AddDays(1);
                SetProperty(ref startingDate, value);

            }
        }
        private DateTime endingDate;
        public DateTime EndingDate {
            get => endingDate;
            set {
                if (value != endingDate) Unvalidate();
                SetProperty(ref endingDate, value); 
            }
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

        bool valid;
        private string saveButtonText;
        public string SaveButtonText {
            get => saveButtonText;
            set => SetProperty(ref saveButtonText, value);
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
            endingDate = DateTime.Now.AddDays(1);
            roomChoosed = false;
            roomChoosedInv = true;
            AvaliableBeds = new ObservableCollection<int>();
            RoomIDs = new ObservableCollection<int>();
            Rooms = new List<Room>();
            RoomID = -1;
            numberOfPeople = -1;

            valid = false;
            saveButtonText = "Sprawdź dostępność";
            PrepareRoomInfo();
        }

        #region Comamnd functions
        private bool ValidateSave() {
            return numberOfPeople >= 0
                && startingDate >= DateTime.Now.AddDays(-1)
                && endingDate > startingDate
                && RoomID != -1
                && numberOfPeople != -1
                ;
        }
        private async void OnCancel() {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
        private async void OnSave() {
            //TODO pause input, add loading screen

            if (valid) {

                Reservation newReservation = new Reservation() {
                RoomId = roomID + 1,
                NumberOfPeople = numberOfPeople + 1,
                StartingDate = startingDate,
                EndingDate = endingDate,

                //EmployeeId = "0",
                CreationDate = DateTime.Now

            };
                //TODO go to next page, entering details

                //TEST JSON
                string jsonString = JsonConvert.SerializeObject(newReservation, new JsonSerializerSettings { DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ" }); //Date format! Throws exception on deserialising if using default format

                await Shell.Current.GoToAsync($"{nameof(NewReservationStep2Page)}?NewReservation={jsonString}");

                //await Shell.Current.GoToAsync("..");
            }
            else {
                //check if reservation is valid

                
                bool enoughBeds = true;

                //If reservation is longer than i day check avaliability for every day
                for (int i = 0; i < (endingDate - startingDate).TotalDays; i++) {
                    //Get starting date, format it
                    string day = startingDate.AddDays(i).Day.ToString(); if (day.Length == 1) day = "0" + day;
                    string month = startingDate.AddDays(i).Month.ToString(); if (month.Length == 1) month = "0" + month;
                    string year = startingDate.AddDays(i).Year.ToString();
                    string result = day + month + year;

                    //get reservations on this day
                    var reservationsOnDay = await _mtChaletService.GetReservationOnDate(result);

                    var freeBeds = Rooms[roomID].RoomCap - numberOfPeople - 1; //count avaliable beds including currently created reservation, -1 because it takes index

                    //Count remaining beds, if not enough to enter currently created reservation throw error
                    foreach (Reservation res in reservationsOnDay) {
                        if (res.RoomId == roomID+1) freeBeds -= res.NumberOfPeople; //+1 because it takes index
                        if (freeBeds < 0) {
                            enoughBeds = false;
                            break;
                            } 
                    }

                    if (!enoughBeds) break;
                }

                if (enoughBeds) {
                    //TODO TEMP Reservation to DB

                    valid = true;
                    SaveButtonText = "Dalej";
                }
                else {
                    //TODO some kind of error msg
                    valid = false;
                }
               


            }
           

            //await _mtChaletService.AddReservation(newReservation);

            // This will pop the current page off the navigation stack
            //await Shell.Current.GoToAsync("..");
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

        private void Unvalidate() {
            valid = false;
            SaveButtonText = "Sprawdź dostępność";
        }
        #endregion
    }
}
