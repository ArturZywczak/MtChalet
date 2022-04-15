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
    public partial class SummaryPage : ContentPage {
        private readonly SummaryViewModel _summaryViewModel;
        public SummaryPage() {
            InitializeComponent();

            _summaryViewModel = Startup.Resolve<SummaryViewModel>();
            BindingContext = _summaryViewModel;
        }
    }
}