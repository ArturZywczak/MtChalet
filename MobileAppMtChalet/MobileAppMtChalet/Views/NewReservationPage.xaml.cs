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
    public partial class NewReservationPage : ContentPage {
        private readonly NewReservationViewModel _newReservationViewModel;
        public NewReservationPage() {
            InitializeComponent();

            _newReservationViewModel = Startup.Resolve<NewReservationViewModel>();
            BindingContext = _newReservationViewModel;
        }
    }
}