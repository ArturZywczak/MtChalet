using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Auth0.OidcClient;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using MobileAppMtChalet.Config;
using MobileAppMtChalet.Droid.Services;
using MobileAppMtChalet.Models;
using MobileAppMtChalet.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(AuthenticationService))]
namespace MobileAppMtChalet.Droid.Services {
    class AuthenticationService : IAuthenticationService {
        private Auth0Client _auth0Client;

        public AuthenticationService() {
            _auth0Client = new Auth0Client(new Auth0ClientOptions {
                Domain = AuthenticationConfig.Domain,
                ClientId = AuthenticationConfig.ClientId
            });
        }

        public AuthenticationResult AuthenticationResult { get; private set; }

        public async Task<AuthenticationResult> Authenticate() {

            var auth0LoginResult = await _auth0Client.LoginAsync(new { audience = AuthenticationConfig.Audience });

            AuthenticationResult authenticationResult;

            if (!auth0LoginResult.IsError) {
                authenticationResult = new AuthenticationResult() {
                    AccessToken = auth0LoginResult.AccessToken,
                    IdToken = auth0LoginResult.IdentityToken,
                    UserClaims = auth0LoginResult.User.Claims,
                    User_Id = auth0LoginResult.User.FindFirst(c => c.Type == "sub")?.Value
                };
            }
            else
                authenticationResult = new AuthenticationResult(auth0LoginResult.IsError, auth0LoginResult.Error);

            AuthenticationResult = authenticationResult;
            return authenticationResult;
        }

        public async Task<bool> Logout() {
            BrowserResultType browserResult = await _auth0Client.LogoutAsync();
            //TODO add something if logout failed (can it?)
            return true;
        }
    }
}