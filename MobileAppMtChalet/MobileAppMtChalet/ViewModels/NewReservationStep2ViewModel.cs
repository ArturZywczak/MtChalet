using MobileAppMtChalet.Models;
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

        private Reservation newReservation;

        public NewReservationStep2ViewModel() {
            newReservation = new Reservation();
        }

        void deserializeReservation() {
            newReservation =  JsonConvert.DeserializeObject<Reservation>(newReservationJson);
        }

    }

}