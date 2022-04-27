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
        bool _isBoxOut;
        public ReservationDetailPage() {
            InitializeComponent();
            _isBoxOut = false;
            _reservationDetailViewModel = Startup.Resolve<ReservationDetailViewModel>();
            BindingContext = _reservationDetailViewModel;

        }

        async void ShowExtraInfo_Clicked(Object sender, EventArgs e) {
            if (!_isBoxOut) {
                await EmployeeInfoBox.TranslateTo(EmployeeInfoBox.TranslationX - 280, 0, 500);
                _isBoxOut = true;
            }
            else {
                await EmployeeInfoBox.TranslateTo(EmployeeInfoBox.TranslationX + 280, 0, 500);
                _isBoxOut = false;
            }

            
        }
    }
}