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
    public partial class LoginPage : ContentPage {
        private readonly LoginViewModel _logInViewModel;
        public LoginPage() {
            InitializeComponent();
            //this.BindingContext = new LoginViewModel();

            _logInViewModel = Startup.Resolve<LoginViewModel>();
            BindingContext = _logInViewModel;
        }
    }
}