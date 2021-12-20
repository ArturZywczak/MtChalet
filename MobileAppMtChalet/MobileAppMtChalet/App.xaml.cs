﻿using MobileAppMtChalet.Services;
using MobileAppMtChalet.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileAppMtChalet {
    public partial class App : Application {

        public App() {
            InitializeComponent();
            Startup.ConfigureServices();
            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart() {
        }

        protected override void OnSleep() {
        }

        protected override void OnResume() {
        }
    }
}
