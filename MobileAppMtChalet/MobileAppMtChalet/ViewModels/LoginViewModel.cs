﻿using MobileAppMtChalet.Services;
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
            loggedUser = new Models.Employee();
        }

        private async void OnPreviewClicked(object obj) {
            await Shell.Current.GoToAsync($"//{nameof(ReservationsPage)}?UserID={loggedUser.Auth0ID}");
        }

        private async void OnLoginClicked(object obj) {
            Processing = true;
            var authenticationService = DependencyService.Get<IAuthenticationService>();
            var authenticationResult = await authenticationService.Authenticate();
            if (!authenticationResult.IsError) {
                loggedUser.Auth0ID = authenticationResult.User_Id;
                //TODO add api acces, check auth0id with user id in database, if error throw msg, if correct serialise and send to next screen, in next screen deserialise
                var test =  await _mtChaletService.GetEmployee(loggedUser.Auth0ID);
                
                string jsonString = JsonConvert.SerializeObject(test, new JsonSerializerSettings());

                // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
                await Shell.Current.GoToAsync($"//{nameof(ReservationsPage)}?UserData={jsonString}");
            }
            else LoginError = true;
        }
    }
}
