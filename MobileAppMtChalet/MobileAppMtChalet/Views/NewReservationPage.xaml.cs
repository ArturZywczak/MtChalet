using MobileAppMtChalet.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace MobileAppMtChalet.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewReservationPage : ContentPage {
        private readonly NewReservationViewModel _newReservationViewModel;
        bool _isSecondStep;
        public NewReservationPage() {
            InitializeComponent();



            _isSecondStep = false;
            _newReservationViewModel = Startup.Resolve<NewReservationViewModel>();
            BindingContext = _newReservationViewModel;

        }






    }
}