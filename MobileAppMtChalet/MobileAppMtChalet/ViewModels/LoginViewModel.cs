using MobileAppMtChalet.Models;
using MobileAppMtChalet.Services;
using MobileAppMtChalet.Services.Interfaces;
using MobileAppMtChalet.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace MobileAppMtChalet.ViewModels {
    public class LoginViewModel : BaseViewModel {

        private bool processing;
        public bool Processing {
            get => processing;
            set {
                SetProperty(ref processing, value);
                ProcessingInv = !value;
                OnPropertyChanged();
            }
        }
        private bool processingInv;
        public bool ProcessingInv {
            get => processingInv;
            set {
                SetProperty(ref processingInv, value);
                OnPropertyChanged();
            }
        }
        private bool loginError;
        public bool LoginError {
            get => loginError;
            set {
                SetProperty(ref loginError, value);
                Processing = false;
                OnPropertyChanged();
            }
        }
        
        public Command LoginCommand { get; }
        public Command PreviewCommand { get; }

        private readonly IMtChaletService _mtChaletService;

        public LoginViewModel(IMtChaletService mtChaletService) {
            _mtChaletService = mtChaletService;
            LoginCommand = new Command(OnLoginClicked);
            PreviewCommand = new Command(OnPreviewClicked);
            processing = false;
            processingInv = true;
            loginError = false;
        }

        private async void OnPreviewClicked(object obj) {
            Employee prevOnly = new Employee() {
                Auth0ID = "PREVIEV_ONLY",
                EmployeeId = 0,
                Role = 0
            };

            string prevUser = prevOnly.Serialize();

            await Shell.Current.GoToAsync($"//{nameof(ReservationsPage)}?UserData={prevUser}");
        }

        private async void OnLoginClicked(object obj) {
            Processing = true;
            var authenticationService = DependencyService.Get<IAuthenticationService>();
            var authenticationResult = await authenticationService.Authenticate();
            if (!authenticationResult.IsError) {
                //TODO add api acces, check auth0id with user id in database, if error throw msg, if correct serialise and send to next screen, in next screen deserialise
                var test =  await _mtChaletService.GetEmployee(authenticationResult.User_Id);
                
                string jsonString = test.Serialize();

                // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
                await Shell.Current.GoToAsync($"//{nameof(ReservationsPage)}?UserData={jsonString}");
                
            }
            else LoginError = true;

            Processing = false;
        }
    }
}
