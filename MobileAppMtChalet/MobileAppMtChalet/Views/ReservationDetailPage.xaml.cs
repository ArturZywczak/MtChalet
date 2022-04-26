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
    public partial class ReservationDetailPage : ContentPage {
        private readonly ReservationDetailViewModel _reservationDetailViewModel;
        bool IsBoxOut;
        public ReservationDetailPage() {
            InitializeComponent();
            IsBoxOut = false;
            _reservationDetailViewModel = Startup.Resolve<ReservationDetailViewModel>();
            BindingContext = _reservationDetailViewModel;
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            _reservationDetailViewModel.OnAppearing();
        }

        async void ShowExtraInfo_Clicked(Object sender, EventArgs e) {
            if (!IsBoxOut) {
                await EmployeeInfoBox.TranslateTo(EmployeeInfoBox.TranslationX - 280, 0, 500);
                IsBoxOut = true;
            }
            else {
                await EmployeeInfoBox.TranslateTo(EmployeeInfoBox.TranslationX + 280, 0, 500);
                IsBoxOut = false;
            }

            
        }
    }
}