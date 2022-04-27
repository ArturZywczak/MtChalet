using MobileAppMtChalet.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileAppMtChalet.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReservationsPage : ContentPage {
        private readonly ReservationsViewModel _reservationsViewModel;
        public ReservationsPage() {
            _reservationsViewModel = Startup.Resolve<ReservationsViewModel>();
            BindingContext = _reservationsViewModel;

            InitializeComponent();

        }

        protected override void OnAppearing() {
            _reservationsViewModel.OnAppearing();
        }

        
    }
}