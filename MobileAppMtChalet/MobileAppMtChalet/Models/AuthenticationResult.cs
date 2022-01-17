using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace MobileAppMtChalet.Models {
    public class AuthenticationResult {
        public string IdToken { get; set; }

        public string AccessToken { get; set; }

        public IEnumerable<Claim> UserClaims { get; set; }

        public bool IsError { get; }

        public string Error { get; }
        public string Email { get; set; }


        public AuthenticationResult() { }

        public AuthenticationResult(bool isError, string error) {
            IsError = isError;
            Error = error;
        }
    }
}
