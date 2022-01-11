using Microsoft.Extensions.DependencyInjection;
using System;
using MobileAppMtChalet.Services;
using MobileAppMtChalet.ViewModels;

namespace MobileAppMtChalet {
    public static class Startup {
        private static IServiceProvider serviceProvider;
        public static void ConfigureServices() {
            var services = new ServiceCollection();

            //add services
            services.AddHttpClient<IMtChaletService, ApiMtChaletService>(c => {
                c.BaseAddress = new Uri("http://10.0.2.2:13123/api/");
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            //add viewmodels
            services.AddTransient<AboutViewModel>();
            services.AddTransient<BaseViewModel>();
            services.AddTransient<ItemDetailViewModel>();
            services.AddTransient<ItemsViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<NewItemViewModel>();
            services.AddTransient<ReservationsViewModel>();
            services.AddTransient<NewReservationViewModel>();
            services.AddTransient<ReservationDetailViewModel>();


            serviceProvider = services.BuildServiceProvider();
        }

        public static T Resolve<T>() => serviceProvider.GetService<T>();
    }
}
