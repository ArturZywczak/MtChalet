using MobileAppMtChalet.Models;
using MobileAppMtChalet.Views;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace MobileAppMtChalet.ViewModels {

    [QueryProperty(nameof(NewReservationJson), "NewReservation")]
    public class NewReservationStep2ViewModel : BaseViewModel {


        private string newReservationJson;
        public string NewReservationJson {
            get => newReservationJson;
            set { 
                SetProperty(ref newReservationJson, value);
                deserializeReservation();
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

        private Reservation newReservation;
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        public NewReservationStep2ViewModel() {
            newReservation = new Reservation();
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }
        private bool ValidateSave() {
            //must have surname and phone or email
            return //surname.Length > 1 && (phone.Length > 5 || email.Length > 5);
                true;
            //TODO also can create some warning about adding res without name and phone

        }
        private async void OnCancel() {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
        private async void OnSave() {

            newReservation.Name = name;
            newReservation.Surname = surname;
            newReservation.Phone = phone;
            newReservation.Email = email;
            newReservation.ExtraInfo = extraInfo;

            //TODO Employee stuff

            //TODO send to summary page
            string jsonString = JsonConvert.SerializeObject(newReservation, new JsonSerializerSettings { DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ" }); //Date format! Throws exception on deserialising if using default format


            await Shell.Current.GoToAsync($"{nameof(SummaryPage)}?NewReservation={jsonString}");

            //await Shell.Current.GoToAsync("..");
        }

        void deserializeReservation() {
            newReservation =  JsonConvert.DeserializeObject<Reservation>(newReservationJson);
        }

    }

}