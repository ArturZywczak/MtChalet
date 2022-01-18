using MobileAppMtChalet.Services.Interfaces;
using MobileAppMtChalet.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace MobileAppMtChalet.ViewModels {
    public class LoginViewModel : BaseViewModel {
        public Command LoginCommand { get; }

        public LoginViewModel() {
            LoginCommand = new Command(OnLoginClicked);
        }

        private async void OnLoginClicked(object obj) {


            var authenticationService = DependencyService.Get<IAuthenticationService>();
            var authenticationResult = await authenticationService.Authenticate();
            if (!authenticationResult.IsError) {
                loggedUser.Auth0ID = authenticationResult.User_Id;
                // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
                await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
            }
        }
    }
}
