using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Watr.Exchange.Security.Core;

namespace Watr.Exchange.Security.Blazor
{
    public class SecurityProvider : ISecurityProvider
    {
        protected IAccessTokenProvider TokenProvider { get; }
        protected AuthenticationStateProvider AuthenticationStateProvider { get; }
        protected NavigationManager NavigationManager { get; }
        public SecurityProvider(IAccessTokenProvider tokenProvider, AuthenticationStateProvider authState, NavigationManager navMan)
        {
            TokenProvider = tokenProvider;
            AuthenticationStateProvider = authState;
            AuthenticationStateProvider.AuthenticationStateChanged += AuthenticationStateProvider_AuthenticationStateChanged;
            NavigationManager = navMan;
            _ = Init();
        }

        private void AuthenticationStateProvider_AuthenticationStateChanged(Task<AuthenticationState> task)
        {
            _ = Init();
        }

        public bool IsAuthenticated { get; protected set; }

        public string? UserName {get; protected set;}

        public event PropertyChangedEventHandler? PropertyChanged;

        private async Task Init()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            IsAuthenticated = user.Identity?.IsAuthenticated ?? false;
            UserName = IsAuthenticated
                                ? user.FindFirst(ClaimTypes.Name)?.Value
                                : null;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsAuthenticated)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserName)));
        }

        public async Task<string?> GetAccessToken(string[] scopes, CancellationToken token = default)
        {
            var result = await TokenProvider.RequestAccessToken(
                new AccessTokenRequestOptions { Scopes = scopes });
            await Init();
            if (result.TryGetToken(out var tkn))
                return tkn.Value;

            if (result.Status == AccessTokenResultStatus.RequiresRedirect)
            {
                // no active session for these scopes → send the user to login
                var returnUrl = Uri.EscapeDataString(NavigationManager.Uri);
                NavigationManager.NavigateToLogin(
                    $"authentication/login?returnUrl={returnUrl}");
            }

            return null;
        }

        public async Task<string?> Login(string[] scopes, bool forceInteractive = false, CancellationToken token = default)
        {
            if (!forceInteractive)
            {
                var silent = await TokenProvider.RequestAccessToken(
                    new AccessTokenRequestOptions { Scopes = scopes });
                await Init();
                if (silent.TryGetToken(out var tkn))
                    return tkn.Value;
                // if silent failed due to no session, fall through to interactive
            }

            // interactive redirect to B2C sign-in
            var returnUrl = Uri.EscapeDataString(NavigationManager.Uri);
            NavigationManager.NavigateToLogin(
                $"authentication/login?returnUrl={returnUrl}");

            // after redirect you’ll get a token on return
            return null;
        }

        public Task Logout(CancellationToken token = default)
        {
            // Blazor’s built-in handler will clear the session
            var returnUrl = Uri.EscapeDataString(NavigationManager.BaseUri);
            NavigationManager.NavigateToLogout("authentication/logout");
            return Task.CompletedTask;
        }
    }
}
