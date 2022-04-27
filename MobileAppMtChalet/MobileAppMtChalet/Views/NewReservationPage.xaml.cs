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

            PrepareGrid();

            _isSecondStep = false;
            _newReservationViewModel = Startup.Resolve<NewReservationViewModel>();
            BindingContext = _newReservationViewModel;

        }

        async void GoToNextStep_Clicked(Object sender, EventArgs e) {


            var m = DeviceDisplay.MainDisplayInfo;

            if (!_isSecondStep) {
                await StepsGrid.TranslateTo(StepsGrid.TranslationX - (m.Width / m.Density), 0, 500);
                _isSecondStep = true;
            }
            else {
                await StepsGrid.TranslateTo(StepsGrid.TranslationX + (m.Width / m.Density), 0, 500);
                _isSecondStep = false;
            }


        }


        void PrepareGrid() {
            var m = DeviceDisplay.MainDisplayInfo;
            
            StepsGrid.ColumnDefinitions[0].Width = m.Width / m.Density;
            StepsGrid.ColumnDefinitions[1].Width = m.Width / m.Density;
        }

        protected override void OnSizeAllocated(double width, double height) {
            base.OnSizeAllocated(width, height); // Important!

            PrepareGrid();
           
            var m = DeviceDisplay.MainDisplayInfo;
            if (!_isSecondStep) {
                StepsGrid.TranslationX = 0;
            }
            else {
                StepsGrid.TranslationX = -(m.Width / m.Density);
                }
            
        }


    }
}