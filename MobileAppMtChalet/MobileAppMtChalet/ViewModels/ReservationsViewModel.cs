﻿using MobileAppMtChalet.Models;
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
    [QueryProperty(nameof(UserID), "UserID")]
    public class ReservationsViewModel : BaseViewModel {

        #region Private and Public Binding
        private Reservation _selectedReservation;
        public Reservation SelectedReservation {
            get => _selectedReservation;
            set {
                SetProperty(ref _selectedReservation, value);
                OnReservationSelected(value);
            }
        }
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
        private string userID;
        public string UserID {
            get {
                return userID;
            }
            set {
                SetProperty(ref userID, value);
                AddReservationCommand.ChangeCanExecute();
            }
        }

        #endregion
        #region Public List & Commands
        public ObservableCollection<Models.Grouping<ReservationsByRoom, Reservation>> ReservationsList { get; set; }
        public ObservableCollection<Reservation> Reservations { get; }
        public Command LoadReservationsCommand { get; }
        public Command AddReservationCommand { get; }
        public Command<Reservation> ReservationTapped { get; }
        public AsyncCommand<DatePicker> DateSelected { get; }
        #endregion

        public ReservationsViewModel(IMtChaletService mtChaletService) {
           
            _mtChaletService = mtChaletService;
            Title = "Rezerwacje";
            Reservations = new ObservableCollection<Reservation>();
            LoadReservationsCommand = new Command(async () => await ExecuteLoadReservationsCommand());
            ReservationTapped = new Command<Reservation>(OnReservationSelected);

            AddReservationCommand = new Command(
                execute: () => {
                    OnAddReservation(this);
                },
                canExecute: () => {
                    if (UserID == null) return false;
                    else return UserID.Length != 0;
                });
            ReservationsList = new ObservableCollection<Models.Grouping<ReservationsByRoom, Reservation>>();
        }


        #region Commands functions
        private async void OnAddReservation(object obj) {
            await Shell.Current.GoToAsync(nameof(NewReservationPage));
        }
        async void OnReservationSelected(Reservation reservation) {
            if (reservation == null) return;
            await Shell.Current.GoToAsync($"{nameof(ReservationDetailPage)}?{nameof(ReservationDetailViewModel.ReservationId)}={reservation.ReservationId}&UserID={UserID}");
        }
        async Task ExecuteLoadReservationsCommand() {
            try {
                ReservationsList.Clear();
                var reservations = await _mtChaletService.GetReservationOnDate(_selectedDate);
                var rooms = await _mtChaletService.GetRooms();
                SetupRoomData(reservations, rooms);
            }
            catch (Exception ex) {
                Debug.WriteLine(ex);
            }
            finally {
                IsBusy = false;
            }
        }
        #endregion
        #region Other functions
        private void SetupRoomData(IEnumerable<Reservation> reservations, IEnumerable<Room> rooms) {

            foreach(var room in rooms) {
                var asset = new ReservationsByRoom();
                asset.Room = room;
                asset.FreeBeds = room.RoomCap;
                foreach(var reservation in reservations) {
                    if (reservation.RoomId == room.RoomId) { 
                        asset.Reservations.Add(reservation);
                        asset.FreeBeds -= reservation.NumberOfPeople;
                    }
                }

                var group = new Models.Grouping<ReservationsByRoom, Reservation>(asset, asset.Reservations);
                ReservationsList.Add(group);
            }

        }
        public void OnAppearing() {
            IsBusy = true;
            SelectedReservation = null;
        }
        #endregion

    }

}