using MobileAppMtChalet.Services.Interfaces;
using MobileAppMtChalet.ViewModels;
using MobileAppMtChalet.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MobileAppMtChalet {
    public partial class AppShell : Xamarin.Forms.Shell {
        private readonly BaseViewModel _baseViewModel;

        public AppShell() {
            InitializeComponent();
            Routing.RegisterRoute(nameof(NewReservationPage), typeof(NewReservationPage));
            Routing.RegisterRoute(nameof(ReservationDetailPage), typeof(ReservationDetailPage));
            Routing.RegisterRoute(nameof(NewReservationStep2Page), typeof(NewReservationStep2Page));
            Routing.RegisterRoute(nameof(SummaryPage), typeof(SummaryPage));

            _baseViewModel = Startup.Resolve<BaseViewModel>();
            BindingContext = _baseViewModel;
        }

        private async void OnMenuItemClicked(object sender, EventArgs e) {
            var authenticationService = DependencyService.Get<IAuthenticationService>();
            await authenticationService.Logout();

            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
