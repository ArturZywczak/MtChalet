using MobileAppMtChalet.Models;
using MobileAppMtChalet.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileAppMtChalet.ViewModels {
    [QueryProperty(nameof(ReservationId), nameof(ReservationId))]
    class ReservationDetailViewModel : BaseViewModel {
        private readonly IMtChaletService _mtChaletService;

        private string reservationId;
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

        public ReservationDetailViewModel(IMtChaletService mtChaletService) {

            _mtChaletService = mtChaletService;
        }

        public string ReservationId {
            get {
                return reservationId;
            }
            set {
                reservationId = value;
                LoadReservationId(value);
            }
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
            set => SetProperty(ref roomID, value);
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
            }
            catch (Exception) {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}
