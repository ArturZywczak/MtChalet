using MobileAppMtChalet.ViewModels;
using MobileAppMtChalet.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MobileAppMtChalet {
    public partial class AppShell : Xamarin.Forms.Shell {
        public AppShell() {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(NewReservationPage), typeof(NewReservationPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e) {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
