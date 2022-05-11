using Auth0.OidcClient;
using Auth0XamarinForms.iOS.Services;
using Foundation;
using MobileAppMtChalet.Config;
using MobileAppMtChalet.Models;
using MobileAppMtChalet.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(AuthenticationService))]
namespace Auth0XamarinForms.iOS.Services {
    public class AuthenticationService : IAuthenticationService {
        private Auth0Client _auth0Client;

        public AuthenticationService() {
            _auth0Client = new Auth0Client(new Auth0ClientOptions {
                Domain = AuthenticationConfig.Domain,
                ClientId = AuthenticationConfig.ClientId
            });
        }

        public AuthenticationResult AuthenticationResult { get; private set; }

        public async Task<AuthenticationResult> Authenticate() {
            var auth0LoginResult = await _auth0Client.LoginAsync(new { audience = 
                AuthenticationConfig.Audience });
            AuthenticationResult authenticationResult;

            if (!auth0LoginResult.IsError) {
                authenticationResult = new AuthenticationResult() {
                    AccessToken = auth0LoginResult.AccessToken,
                    IdToken = auth0LoginResult.IdentityToken,
                    UserClaims = auth0LoginResult.User.Claims
                };
            }
            else
                authenticationResult = new AuthenticationResult(auth0LoginResult.IsError, 
                    auth0LoginResult.Error);

            AuthenticationResult = authenticationResult;
            return authenticationResult;
        }

        public Task<bool> Logout() {
            throw new NotImplementedException();
        }

        Task<AuthenticationResult> IAuthenticationService.Authenticate() {
            throw new NotImplementedException();
        }
    }
}
