using RecycleMeDomainClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.OData.Builder;

namespace RecycleMeOdataWebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //var cors = new EnableCorsAttribute("*", "*", "*","DataServiceVersion");
            //config.EnableCors(cors);

            //config.EnableCors(new EnableCorsAttribute("*", "*", "*", "DataServiceVersion, MaxDataServiceVersion"));
         
            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            config.EnableQuerySupport();

            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();

            //ItemImageController
            builder.EntitySet<ItemImage>("ItemImage"); 
            
            //ItemCategoryController
            builder.EntitySet<ItemCategory>("ItemCategory");

            //ItemComment
            builder.EntitySet<ItemComment>("ItemComment");

            //ItemController
            builder.EntitySet<Item>("Item");
            builder.Entity<Item>().Collection.Action("UploadFile");
            
            ActionConfiguration action = builder.Entity<Item>().Collection.Action("DownloadFile");
            action.Parameter<string>("name");

            ActionConfiguration fbAction = builder.Entity<Item>().Collection.Action("PostFacebook");
            fbAction.Parameter<string>("UserId");

            
            //ItemFollower
            builder.EntitySet<ItemFollowers>("ItemFollower");
        
            
            //UserComment
            builder.EntitySet<UserComment>("UserComment");
           
            //UserFollower
            builder.EntitySet<UserFollower>("UserFollower");
            
            //UserController
            builder.EntitySet<User>("User");
            
            //UserFollowingController
            builder.EntitySet<UserFollowing>("UserFollowing");

            //MessageController
            builder.EntitySet<Message>("Message");

            //TradeController
            builder.EntitySet<Trade>("Trade");

            //TradeComment
            builder.EntitySet<TradeComment>("TradeComment");

            //TradeBuyerItem
            builder.EntitySet<TradeBuyerItem>("TradeBuyerItem");
            ActionConfiguration TradeBuyerItemDelete = builder.Entity<TradeBuyerItem>().Collection.Action("TradeBuyerItemDelete");
            TradeBuyerItemDelete.Parameter<string>("TradeId");


            //Notifications
            builder.EntitySet<Notification>("Notification"); 

            config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());

            // To disable tracing in your application, please comment out or remove the following line of code
            // For more information, refer to: http://www.asp.net/web-api
            //config.EnableSystemDiagnosticsTracing();

        }
    }
}
