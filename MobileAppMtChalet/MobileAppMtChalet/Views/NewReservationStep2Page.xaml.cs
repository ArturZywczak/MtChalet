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
    public partial class NewReservationStep2Page : ContentPage {
        private readonly NewReservationStep2ViewModel _newReservationStep2ViewModel;
        public NewReservationStep2Page() {
            InitializeComponent();

            _newReservationStep2ViewModel = Startup.Resolve<NewReservationStep2ViewModel>();
            BindingContext = _newReservationStep2ViewModel;
        }
    }
}