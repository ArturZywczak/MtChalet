using MobileAppMtChalet.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace MobileAppMtChalet.Views {
    public partial class ItemDetailPage : ContentPage {
        public ItemDetailPage() {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}