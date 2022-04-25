using MobileAppMtChalet.Models;
using MobileAppMtChalet.Views;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace MobileAppMtChalet.ViewModels {

    [QueryProperty(nameof(NewReservationJson), "NewReservation")]
    [QueryProperty(nameof(UserID), "UserID")]
    public class NewReservationStep2ViewModel : BaseViewModel {


        private string newReservationJson;
        public string NewReservationJson {
            get => newReservationJson;
            set { 
                SetProperty(ref newReservationJson, value);
                DeserializeReservation();
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

        private string userID;
        public string UserID {
            get {
                return userID;
            }
            set {
                SetProperty(ref userID, value);
            }
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

            //Set below as empty instead of null to avoid passing null to validation function
            email = "";
            phone = "";
            surname = "";
        }
        private bool ValidateSave() {

            //must have surname and phone or email
            return surname.Length > 1 && (IsValidPhone(phone)|| IsValidEmail(email));
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


            await Shell.Current.GoToAsync($"{nameof(SummaryPage)}?NewReservation={jsonString}&UserID={UserID}");

            //await Shell.Current.GoToAsync("..");
        }

        void DeserializeReservation() {
            newReservation =  JsonConvert.DeserializeObject<Reservation>(newReservationJson);
        }

        //Validate email
        public static bool IsValidEmail(string email) {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match) {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e) {
                return false;
            }
            catch (ArgumentException e) {
                return false;
            }

            try {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException) {
                return false;
            }
        }

        public bool IsValidPhone(string phone) {

            //Obselete, GetInstance() takes too long, changed for regex
            /* if (string.IsNullOrWhiteSpace(phone))
                 return false;

             var phoneUtil = PhoneNumberUtil.GetInstance(); //This takes too long
             PhoneNumber parsedPhoneNumber;

             try { 
                 parsedPhoneNumber = phoneUtil.Parse(phone, "PL");
             }
             catch (Exception ex) {
                 Debug.Write(ex);
                 return false;
             }
             return phoneUtil.IsValidNumber(parsedPhoneNumber);*/

            if (string.IsNullOrWhiteSpace(phone))
                return false;

            //Remove spaces and '-' from phone number, regex accepts + or 00, every avaliable country code, then phone number
            return Regex.Match(phone.Trim(new char[] {' ','-'}), @"^(\+|00)?(297|93|244|1264|358|355|376|971|54|374|1684|1268|61|43|994|257|32|229|226|880|359|973|1242|387|590|375|501|1441|591|55|1246|673|975|267|236|1|61|41|56|86|225|237|243|242|682|57|269|238|506|53|5999|61|1345|357|420|49|253|1767|45|1809|1829|1849|213|593|20|291|212|34|372|251|358|679|500|33|298|691|241|44|995|44|233|350|224|590|220|245|240|30|1473|299|502|594|1671|592|852|504|385|509|36|62|44|91|246|353|98|964|354|972|39|1876|44|962|81|76|77|254|996|855|686|1869|82|383|965|856|961|231|218|1758|423|94|266|370|352|371|853|590|212|377|373|261|960|52|692|389|223|356|95|382|976|1670|258|222|1664|596|230|265|60|262|264|687|227|672|234|505|683|31|47|977|674|64|968|92|507|64|51|63|680|675|48|1787|1939|850|351|595|970|689|974|262|40|7|250|966|249|221|65|500|4779|677|232|503|378|252|508|381|211|239|597|421|386|46|268|1721|248|963|1649|235|228|66|992|690|993|670|676|1868|216|90|688|886|255|256|380|598|1|998|3906698|379|1784|58|1284|1340|84|678|681|685|967|27|260|263)?(9[976]\d|8[987530]\d|6[987]\d|5[90]\d|42\d|3[875]\d|2[98654321]\d|9[8543210]|8[6421]|6[6543210]|5[87654321]|4[987654310]|3[9643210]|2[70]|7|1)\d{4,20}$$").Success;
        }
    }

}