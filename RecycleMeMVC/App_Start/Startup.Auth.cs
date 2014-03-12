using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Facebook;
using Owin;
using System.Collections.Generic;

namespace RecycleMeMVC
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");


             List<string> scope = new List<string>() { "email", "user_about_me", "user_hometown", "friends_about_me", "friends_photos" };
             var x = new FacebookAuthenticationOptions();
             x.Scope.Add("user_birthday");
             x.Scope.Add("user_location");
             x.Scope.Add("email");

             x.AppId = "226723064158229";
             x.AppSecret = "57b65d4b283785e7b5ee9e0af93ab3e8";
             x.Provider = new FacebookAuthenticationProvider()
             {
                OnAuthenticated = async context =>
                {
                     //Get the access token from FB and store it in the database and
                    //use FacebookC# SDK to get more information about the user
                    context.Identity.AddClaim(
                    new System.Security.Claims.Claim("FacebookAccessToken",
                                                         context.AccessToken));
                }
             };
             x.SignInAsAuthenticationType = DefaultAuthenticationTypes.ExternalCookie;

             app.UseFacebookAuthentication(x);




            app.UseTwitterAuthentication(
               consumerKey: "olkei9dFx5ZIgcVHEHqEQ",
               consumerSecret: "Xdq3YWMYeJFL0b9QnNClfcdjhhSQprdj0sdHV6ovftM");

            //app.UseFacebookAuthentication(
            //   appId: "226723064158229",
            //   appSecret: "57b65d4b283785e7b5ee9e0af93ab3e8");

            app.UseGoogleAuthentication();
        }
    }
}