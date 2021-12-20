using MobileAppMtChalet.Models;
using MobileAppMtChalet.ViewModels;
using MobileAppMtChalet.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileAppMtChalet.Views {
    public partial class ItemsPage : ContentPage {
        private readonly ItemsViewModel _itemsViewModel;
        public ItemsPage() {
            InitializeComponent();
            _itemsViewModel = Startup.Resolve<ItemsViewModel>();
            BindingContext = _itemsViewModel;
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            _itemsViewModel.OnAppearing();
        }
    }
}