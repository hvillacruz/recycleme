using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Twitter;
using Owin;
using System.Collections.Generic;
using System.Threading.Tasks;

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


            List<string> scope = new List<string>();// { "email", "user_about_me", "user_hometown", "friends_about_me", "friends_photos", "manage_pages", "publish_actions", "publish_stream" };
            var x = new FacebookAuthenticationOptions();
            x.Scope.Add("user_birthday");
            x.Scope.Add("user_location");
            x.Scope.Add("email");
            x.Scope.Add("publish_actions");
            x.Scope.Add("manage_pages");
            x.Scope.Add("publish_stream");
            x.Scope.Add("user_notes");
            x.Scope.Add("user_online_presence");
            x.Scope.Add("user_photo_video_tags");
            x.Scope.Add("user_photos");
            x.Scope.Add("user_questions");
            x.Scope.Add("user_relationship_details");
            x.Scope.Add("user_relationships");
            x.Scope.Add("user_religion_politics");
            x.Scope.Add("user_status");
            x.Scope.Add("user_subscriptions");
            x.Scope.Add("user_videos");
            x.Scope.Add("user_website");
            x.Scope.Add("user_work_history");

            x.Scope.Add("friends_about_me");
            x.Scope.Add("friends_actions.books");
            x.Scope.Add("friends_actions.music");
            x.Scope.Add("friends_actions.news");
            x.Scope.Add("friends_actions.video");
            x.Scope.Add("friends_activities");
            x.Scope.Add("friends_birthday");
            x.Scope.Add("friends_checkins");
            x.Scope.Add("friends_education_history");
            x.Scope.Add("friends_events");
            x.Scope.Add("friends_games_activity");
            x.Scope.Add("friends_groups");
            x.Scope.Add("friends_hometown");
            x.Scope.Add("friends_interests");
            x.Scope.Add("friends_likes");
            x.Scope.Add("friends_location");
            x.Scope.Add("friends_notes");
            x.Scope.Add("friends_online_presence");
            x.Scope.Add("friends_photo_video_tags");
            x.Scope.Add("friends_photos");
            x.Scope.Add("friends_questions");
            x.Scope.Add("friends_relationship_details");
            x.Scope.Add("friends_relationships");
            x.Scope.Add("friends_religion_politics");
            x.Scope.Add("friends_status");
            x.Scope.Add("friends_subscriptions");
            x.Scope.Add("friends_videos");
            x.Scope.Add("friends_website");
            x.Scope.Add("friends_work_history");

            x.Scope.Add("ads_management");
            x.Scope.Add("ads_read");
            x.Scope.Add("create_event");
            x.Scope.Add("create_note");
            x.Scope.Add("email");
            x.Scope.Add("export_stream");
            x.Scope.Add("manage_friendlists");
            x.Scope.Add("manage_notifications");
            x.Scope.Add("manage_pages");
            x.Scope.Add("photo_upload");
            x.Scope.Add("publish_actions");
            x.Scope.Add("publish_checkins");
            x.Scope.Add("publish_stream");
            x.Scope.Add("read_friendlists");
            x.Scope.Add("read_insights");
            x.Scope.Add("read_mailbox");
            x.Scope.Add("read_page_mailboxes");
            x.Scope.Add("read_requests");
            x.Scope.Add("read_stream");
            x.Scope.Add("rsvp_event");
            x.Scope.Add("share_item");
            x.Scope.Add("sms");
            x.Scope.Add("status_update");
            x.Scope.Add("video_upload");
            x.Scope.Add("xmpp_login");




            x.AppId = "226723064158229";
            x.AppSecret = "57b65d4b283785e7b5ee9e0af93ab3e8";
            x.Provider = new FacebookAuthenticationProvider()
            {
                OnAuthenticated = async context =>
                {
                    //Get the access token from FB and store it in the database and
                    //use FacebookC# SDK to get more information about the user
                    context.Identity.AddClaim(
                    new System.Security.Claims.Claim("FacebookAccessToken",context.AccessToken));
                }
            };
            x.SignInAsAuthenticationType = DefaultAuthenticationTypes.ExternalCookie;

            app.UseFacebookAuthentication(x);



            var tw = new TwitterAuthenticationOptions
            {

                ConsumerKey = "olkei9dFx5ZIgcVHEHqEQ",
                ConsumerSecret = "Xdq3YWMYeJFL0b9QnNClfcdjhhSQprdj0sdHV6ovftM",
                SignInAsAuthenticationType = DefaultAuthenticationTypes.ExternalCookie,
                Provider = new TwitterAuthenticationProvider()
                {
                    OnAuthenticated = (context) =>
                    {
                        context.Identity.AddClaim(
                            new System.Security.Claims.Claim("urn:twitter:access_token", context.AccessToken));

                        context.Identity.AddClaim(
                            new System.Security.Claims.Claim("urn:twitter:access_token_secret", context.AccessTokenSecret));
                        return Task.FromResult(0);
                    }
                }

            };

            app.UseTwitterAuthentication(tw);


            //app.UseTwitterAuthentication(
            //   consumerKey: "olkei9dFx5ZIgcVHEHqEQ",
            //   consumerSecret: "Xdq3YWMYeJFL0b9QnNClfcdjhhSQprdj0sdHV6ovftM");

            //app.UseFacebookAuthentication(
            //   appId: "226723064158229",
            //   appSecret: "57b65d4b283785e7b5ee9e0af93ab3e8");

            app.UseGoogleAuthentication();
        }
    }
}