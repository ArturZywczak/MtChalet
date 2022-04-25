using MobileAppMtChalet.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MobileAppMtChalet.Services.Interfaces {
    public interface IAuthenticationService {
        Task<AuthenticationResult> Authenticate();
        Task<bool> Logout();
        AuthenticationResult AuthenticationResult { get; }
    }
}
