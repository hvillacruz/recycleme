using System;
using System.IO;
using System.Net;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Linq;
using RecycleMeDomainClasses;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RecycleMeDataAccessLayer;
using System.Configuration;

namespace RecycleMeBusinessLogicLayer
{
    public class Twitter
    {
        public const string OauthVersion = "1.0";
        public const string OauthSignatureMethod = "HMAC-SHA1";

        public Twitter(string consumerKey, string consumerKeySecret, string accessToken, string accessTokenSecret)
        {
            this.ConsumerKey = consumerKey;
            this.ConsumerKeySecret = consumerKeySecret;
            this.AccessToken = accessToken;
            this.AccessTokenSecret = accessTokenSecret;
        }

        public string ConsumerKey { set; get; }
        public string ConsumerKeySecret { set; get; }
        public string AccessToken { set; get; }
        public string AccessTokenSecret { set; get; }


        public UserViewModel UserInfo(AspNetUsers User)
        {

            string resourceUrl = ConfigurationManager.AppSettings["TweeterShowApi"];

            var requestParameters = new SortedDictionary<string, string>();
            requestParameters.Add("screen_name", User.UserName);
            requestParameters.Add("include_entities", "true");
            var response = GetResponse(resourceUrl, Method.GET, requestParameters);
            dynamic info = System.Web.Helpers.Json.Decode(response);
            UserViewModel user = new UserViewModel();
            try
            {

                user = new UserViewModel()
                {
                    UserId = User.Id,
                    ExternalId = info.id_str,
                    ExternalUserName = info.screen_name,
                    FirstName = info.name,
                    Email = "",
                    LastName = "",
                    BirthDate = null,
                    Address = info.location,
                    Avatar = info.profile_image_url
                };
                user.Avatar = user.Avatar.Replace("_normal", "");
                return user;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return user;
            }

        }


        public string GetMentions(int Count)
        {
            string resourceUrl =
                string.Format("http://api.twitter.com/1/statuses/mentions.json");

            var requestParameters = new SortedDictionary<string, string>();
            requestParameters.Add("count", Count.ToString());
            requestParameters.Add("include_entities", "true");

            var response = GetResponse(resourceUrl, Method.GET, requestParameters);

            return response;
        }

        public string GetTweets(string ScreenName, int Count)
        {
            string resourceUrl =
                string.Format("https://api.twitter.com/1.1/statuses/user_timeline.json");

            var requestParameters = new SortedDictionary<string, string>();
            requestParameters.Add("count", Count.ToString());
            requestParameters.Add("screen_name", ScreenName);

            var response = GetResponse(resourceUrl, Method.GET, requestParameters);

            return response;
        }

        public string PostStatusUpdate(string Status, double Latitude, double Longitude)
        {
            const string resourceUrl = "http://api.twitter.com/1/statuses/update.json";

            var requestParameters = new SortedDictionary<string, string>();
            requestParameters.Add("status", Status);
            //requestParameters.Add("lat", Latitude.ToString());
            //requestParameters.Add("long", Longitude.ToString());

            return GetResponse(resourceUrl, Method.POST, requestParameters);
        }

         public string SendDirectMessage(string screenName, string text)
        {
            string resourceUrl =
                string.Format("https://api.twitter.com/1.1/direct_messages/new.json");

            var requestParameters = new SortedDictionary<string, string>();
            requestParameters.Add("screen_name", screenName);
            requestParameters.Add("text", text);

            var response = GetResponse(resourceUrl, Method.POST, requestParameters);

            return response;
            
        }

        private string GetResponse(string ResourceUrl, Method Method, SortedDictionary<string, string> RequestParameters)
        {
            ServicePointManager.Expect100Continue = false;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            WebRequest request = null;
            string resultString = string.Empty;

            if (Method == Method.POST)
            {
                var postBody = RequestParameters.ToWebString();
               
                request = (HttpWebRequest)WebRequest.Create(ResourceUrl);
                request.Method = Method.ToString();
                request.ContentType = "application/x-www-form-urlencoded";

                using (var stream = request.GetRequestStream())
                {
                    byte[] content = Encoding.ASCII.GetBytes(postBody);
                    stream.Write(content, 0, content.Length);
                }
            }
            else if (Method == Method.GET)
            {
                request = (HttpWebRequest)WebRequest.Create(ResourceUrl + "?"
                    + RequestParameters.ToWebString());
                request.Method = Method.ToString();
            }
            else
            {
                //other verbs can be addressed here...
            }

            if (request != null)
            {

                var authHeader = CreateHeader(ResourceUrl, Method, RequestParameters);
                request.Headers.Add("Authorization", authHeader);
                var response = request.GetResponse();

                using (var sd = new StreamReader(response.GetResponseStream()))
                {
                    resultString = sd.ReadToEnd();
                    response.Close();
                }
            }

            return resultString;
        }

        private string CreateOauthNonce()
        {
            return Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
        }

        private string CreateHeader(string ResourceUrl, Method Method,
                                    SortedDictionary<string, string> requestParameters)
        {
            var oauthNonce = CreateOauthNonce();
            // Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
            var oauthTimestamp = CreateOAuthTimestamp();
            var oauthSignature = CreateOauthSignature(ResourceUrl, Method, oauthNonce, oauthTimestamp, requestParameters);

            //The oAuth signature is then used to generate the Authentication header. 
            const string headerFormat = "OAuth oauth_nonce=\"{0}\", oauth_signature_method=\"{1}\", " +
                                        "oauth_timestamp=\"{2}\", oauth_consumer_key=\"{3}\", " +
                                        "oauth_token=\"{4}\", oauth_signature=\"{5}\", " +
                                        "oauth_version=\"{6}\"";

            var authHeader = string.Format(headerFormat,
                                           Uri.EscapeDataString(oauthNonce),
                                           Uri.EscapeDataString(OauthSignatureMethod),
                                           Uri.EscapeDataString(oauthTimestamp),
                                           Uri.EscapeDataString(ConsumerKey),
                                           Uri.EscapeDataString(AccessToken),
                                           Uri.EscapeDataString(oauthSignature),
                                           Uri.EscapeDataString(OauthVersion)
                );

            return authHeader;
        }

        private string CreateOauthSignature(string ResourceUrl, Method Method, string OAuthNonce, string OAuthTimestamp,
                                            SortedDictionary<string, string> requestParameters)
        {
            //firstly we need to add the standard oauth parameters to the sorted list
            requestParameters.Add("oauth_consumer_key", ConsumerKey);
            requestParameters.Add("oauth_nonce", OAuthNonce);
            requestParameters.Add("oauth_signature_method", OauthSignatureMethod);
            requestParameters.Add("oauth_timestamp", OAuthTimestamp);
            requestParameters.Add("oauth_token", AccessToken);
            requestParameters.Add("oauth_version", OauthVersion);

            var sigBaseString = requestParameters.ToWebString();

            var signatureBaseString = string.Concat(Method.ToString(), "&", Uri.EscapeDataString(ResourceUrl), "&",
                                                    Uri.EscapeDataString(sigBaseString.ToString()));


            //Using this base string, we then encrypt the data using a composite of the 
            //secret keys and the HMAC-SHA1 algorithm.
            var compositeKey = string.Concat(Uri.EscapeDataString(ConsumerKeySecret), "&",
                                             Uri.EscapeDataString(AccessTokenSecret));


            string oauthSignature;
            using (var hasher = new HMACSHA1(Encoding.ASCII.GetBytes(compositeKey)))
            {
                oauthSignature = Convert.ToBase64String(
                    hasher.ComputeHash(Encoding.ASCII.GetBytes(signatureBaseString)));
            }

            return oauthSignature;
        }

        private static string CreateOAuthTimestamp()
        {

            var nowUtc = DateTime.UtcNow;
            var timeSpan = nowUtc - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var timestamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString();

            return timestamp;
        }

    }

    public enum Method
    {
        POST,
        GET
    }

    public static class Extensions
    {
        public static string ToWebString(this SortedDictionary<string, string> Source)
        {
            var body = new StringBuilder();

            foreach (var requestParameter in Source)
            {
                body.Append(requestParameter.Key);
                body.Append("=");
                body.Append(Uri.EscapeDataString(requestParameter.Value));
                body.Append("&");
            }
            //remove trailing '&'
            body.Remove(body.Length - 1, 1);

            return body.ToString();
        }

        public static string PopulateTweetLinks(string TweetText)
        {
            string regexHtHyperLink =
                @"(http|ftp|https)://([\w+?\.\w+])+([a-zA-Z0-9\~\!\@\#\$\%\^\&\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*)?";

            var urlRx = new
               Regex(regexHtHyperLink, RegexOptions.IgnoreCase);

            MatchCollection matches = urlRx.Matches(TweetText);
            return matches.Cast<Match>().Aggregate(TweetText, (current, match) => current.Replace(match.Value, string.Format("({1})", match.Value, match.Value)));
        }
    }

}