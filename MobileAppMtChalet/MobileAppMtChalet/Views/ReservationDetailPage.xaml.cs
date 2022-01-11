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
        public ReservationDetailPage() {
            InitializeComponent();

            _reservationDetailViewModel = Startup.Resolve<ReservationDetailViewModel>();
            BindingContext = _reservationDetailViewModel;
        }
    }
}