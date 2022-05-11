using Microsoft.Extensions.DependencyInjection;
using System;
using MobileAppMtChalet.Services;
using MobileAppMtChalet.ViewModels;
using MobileAppMtChalet.Config;

namespace MobileAppMtChalet {
    public static class Startup {
        private static IServiceProvider serviceProvider;
        public static void ConfigureServices() {
            var services = new ServiceCollection();

            //add services
            services.AddHttpClient<IMtChaletService, ApiMtChaletService>(c => {
                c.BaseAddress = new Uri(ApiConfig.ApiURI);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            //add viewmodels
            services.AddTransient<BaseViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<ReservationsViewModel>();
            services.AddTransient<NewReservationViewModel>();
            services.AddTransient<ReservationDetailViewModel>();
            services.AddTransient<NewReservationStep2ViewModel>();
            services.AddTransient<SummaryViewModel>();


            serviceProvider = services.BuildServiceProvider();
        }

        public static T Resolve<T>() => serviceProvider.GetService<T>();
    }
}
