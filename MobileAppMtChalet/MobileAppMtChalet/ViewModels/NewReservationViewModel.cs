﻿using MobileAppMtChalet.Models;
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
        private readonly IMtChaletService _mtChaletService;

        private string name;
        private string surname;
        private int roomID;
        private int numberOfPeople;
        private DateTime startingDate;
        private DateTime endingDate;
        private string phone;
        private string email;
        private string extraInfo;
        private int employeeID;
        private DateTime creationDate;
        private bool roomChoosed;
        private bool roomChoosedInv;

        public List<Room> Rooms { get; set; }
        public ObservableCollection<int> AvaliableBeds { get; }
        public ObservableCollection<int> RoomIDs { get; }

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

        private bool ValidateSave() {
            return !String.IsNullOrWhiteSpace(surname)
                //&& RoomExists(roomID).Result
                && numberOfPeople > 0
                && startingDate > DateTime.Now
                && endingDate > startingDate
                ;
        }

        public bool RoomChoosed {
            get => roomChoosed;
            set => SetProperty(ref roomChoosed, value);
        }

        public bool RoomChoosedInv {
            get => roomChoosedInv;
            set => SetProperty(ref roomChoosedInv, value);
        }


        public string Name {
            get => name;
            set => SetProperty(ref name, value);
        }

        public string Surname {
            get => surname;
            set => SetProperty(ref surname, value);
        }

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

        public int NumberOfPeople {
            get => numberOfPeople;
            set => SetProperty(ref numberOfPeople, value);
        }

        public DateTime StartingDate {
            get => startingDate;
            set => SetProperty(ref startingDate, value);
        }
        public DateTime EndingDate {
            get => endingDate;
            set => SetProperty(ref endingDate, value);
        }
        public string Phone {
            get => phone;
            set => SetProperty(ref phone, value);
        }
        public string Email {
            get => email;
            set => SetProperty(ref email, value);
        }
        public string ExtraInfo {
            get => extraInfo;
            set => SetProperty(ref extraInfo, value);
        }


        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

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

        private async Task<bool> RoomExists(int roomId) {
            var rooms = await _mtChaletService.GetRooms();
            foreach (var room in rooms) if (room.RoomId == roomId) return true;
            return false;
        }

        private void ChangeSelectedRoom() {
            AvaliableBeds.Clear();
            for (int i = 1; i<=Rooms[roomID].RoomCap; i++) AvaliableBeds.Add(i);
        }
    }
}
