using MobileAppMtChalet.Models;
using MobileAppMtChalet.Services;
using MobileAppMtChalet.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace MobileAppMtChalet.ViewModels {
    [QueryProperty(nameof(NewReservationJson), "NewReservation")]
    class SummaryViewModel : BaseViewModel {
        private readonly IMtChaletService _mtChaletService;

        private string newReservationJson;
        public string NewReservationJson {
            get => newReservationJson;
            set {
                SetProperty(ref newReservationJson, value);
                deserializeReservation();
            }
        }

        private Reservation newReservation;

        Dictionary<string,string> reservationDetails = new Dictionary<string, string>();
        public Dictionary<string, string> ReservationDetails { get { return reservationDetails; } }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        public SummaryViewModel(IMtChaletService mtChaletService) {
            _mtChaletService = mtChaletService;
            newReservation = new Reservation();
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave() {
            //must have surname and phone or email

            return true;

        }
        private async void OnCancel() {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        void deserializeReservation() {
            newReservation = JsonConvert.DeserializeObject<Reservation>(newReservationJson);

            ReservationDetails.Add("Imię", newReservation.Name);
            ReservationDetails.Add("Nazwisko", newReservation.Surname);
            ReservationDetails.Add("Nr Pokoju", newReservation.RoomId.ToString());
            ReservationDetails.Add("Ilość osób", newReservation.NumberOfPeople.ToString());
            ReservationDetails.Add("Data przyjazdu", newReservation.StartingDate.ToString("dd/MM/yyyy"));
            ReservationDetails.Add("Data Wyjazdu", newReservation.EndingDate.ToString("dd/MM/yyyy"));
            ReservationDetails.Add("Nr Telefonu", newReservation.Phone);
            ReservationDetails.Add("Email", newReservation.Email);
            ReservationDetails.Add("Dodatkowe Informacje", newReservation.ExtraInfo);
        }
        private async void OnSave() {

            await _mtChaletService.AddReservation(newReservation);


            await Shell.Current.GoToAsync($"//ReservationsPage?UserData={UserData.Serialize()}");
        }


    }
}
