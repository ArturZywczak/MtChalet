using MobileAppMtChalet.Models;
using MobileAppMtChalet.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace MobileAppMtChalet.ViewModels {

    [QueryProperty(nameof(UserDataJson), "UserData")]
    public class BaseViewModel : INotifyPropertyChanged {

        private string userDataJson;
        public string UserDataJson {
            get => userDataJson;
            set {
                SetProperty(ref userDataJson, value);
                UserData = new Employee(value);

                if (userData.Role > 2) CanViewDetails = true;
                if (userData.Role > 1) CanEdit = true;
            }
        }

        private Employee userData;
        public Employee UserData {
            get => userData;
            set {
                SetProperty(ref userData, value);
            }
        }

        private bool canEdit = false;
        public bool CanEdit {
            get => canEdit;
            set => SetProperty(ref canEdit, value);
        }
        private bool canViewDetails = false;
        public bool CanViewDetails {
            get => canViewDetails;
            set => SetProperty(ref canViewDetails, value);
        }


        bool isBusy = false;
        public bool IsBusy {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = "";
        public string Title {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null) {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "") {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
