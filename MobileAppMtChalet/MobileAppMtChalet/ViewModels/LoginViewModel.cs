using MobileAppMtChalet.Services.Interfaces;
using MobileAppMtChalet.Views;
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

        public LoginViewModel() {
            LoginCommand = new Command(OnLoginClicked);
            PreviewCommand = new Command(OnPreviewClicked);
            processing = false;
            processingInv = true;
            loginError = false;
        }

        private async void OnPreviewClicked(object obj) {
            await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }

        private async void OnLoginClicked(object obj) {
            Processing = true;
            var authenticationService = DependencyService.Get<IAuthenticationService>();
            var authenticationResult = await authenticationService.Authenticate();
            if (!authenticationResult.IsError) {
                loggedUser.Auth0ID = authenticationResult.User_Id;
                // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
                await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
            }
            else LoginError = true;
        }
    }
}
