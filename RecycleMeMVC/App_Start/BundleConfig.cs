using System.Web;
using System.Web.Optimization;

namespace RecycleMeMVC
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-1.10.3.custom.min.js"));


            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/init").Include(
                       "~/Scripts/Account/LoginReg.js"));

            bundles.Add(new ScriptBundle("~/bundles/header").Include(
                      "~/Scripts/Home/Header.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                    "~/Scripts/kendo.web.min.js",
                    "~/Scripts/knockout-3.1.0.js",
                    "~/Scripts/knockout-kendo.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-editable.js",
                       "~/Scripts/External/bootstrap-datepicker.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/layout").Include(
              "~/Scripts/string.min.js",
              "~/Scripts/datajs-1.1.2.js",
               "~/Scripts/Main/Global.js",
              "~/Scripts/Main/Common.js",
              "~/Scripts/Main/Config.js",
              "~/Scripts/Main/Helper.js",
              "~/Scripts/External/Sortable.min.js",
              "~/Scripts/bootstrap-editable.min.js",
              "~/Scripts/knockout.x-editable.min.js",
              "~/Scripts/toastr.min.js",
              "~/Scripts/Chart.min.js",
              "~/Scripts/toastr.min.js",
              "~/Scripts/moment.min.js",
              "~/Scripts/underscore.js"));

            bundles.Add(new ScriptBundle("~/bundles/azure").Include(
                       "~/Scripts/Main/azure.js"));

            bundles.Add(new ScriptBundle("~/bundles/modal").Include(
           "~/Scripts/Main/modal.js"));



            bundles.Add(new ScriptBundle("~/bundles/modernizr-custom").Include(
                        "~/Scripts/modernizr.custom.js"));

            bundles.Add(new ScriptBundle("~/bundles/trade").Include(
             "~/Scripts/Home/TradeItems.js",
             "~/Scripts/masonry.pkgd.min.js",
             "~/Scripts/imagesloaded.js",
             "~/Scripts/classie.js",
             "~/Scripts/AnimOnScroll.js"));


            bundles.Add(new ScriptBundle("~/bundles/hubnotification").Include(
             "~/Scripts/Main/HubNotification.js"));

            bundles.Add(new ScriptBundle("~/bundles/profilebar").Include(
            "~/Scripts/Profile/ProfileBar.js"));

            bundles.Add(new ScriptBundle("~/bundles/userspartial").Include(
            "~/Scripts/Profile/Users.js"));


            bundles.Add(new ScriptBundle("~/bundles/pillpartial").Include(
             "~/Scripts/Profile/Pill.js",
             "~/Scripts/Profile/tagging.js",
             "~/Scripts/External/fuelux.wizard.min.js",
             "~/Scripts/External/ace-elements.min.js",
             "~/Scripts/External/bootbox.min.js",
             "~/Scripts/Profile/TagDrop.js"));

            bundles.Add(new ScriptBundle("~/bundles/timeline").Include(
           "~/Scripts/Profile/TimeLine.js"));

            bundles.Add(new ScriptBundle("~/bundles/dropzone").Include(
            "~/Scripts/MJZ.dropzone.js"));

            bundles.Add(new ScriptBundle("~/bundles/tradeexchange").Include(
           "~/Scripts/modernizr.custom.js",
           "~/Scripts/External/Sortable.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/tradepartial").Include(
           "~/Scripts/Profile/Trade.js"));

            bundles.Add(new ScriptBundle("~/bundles/exchangepartial").Include(
           "~/Scripts/Profile/Exchange.js"));


            bundles.Add(new ScriptBundle("~/bundles/tradeexchangepartial").Include(
            "~/Scripts/jquery-ui.js",
            "~/Scripts/Profile/Chart.js"));


            bundles.Add(new ScriptBundle("~/bundles/bid").Include(
           "~/Scripts/Profile/BidItems.js",
           "~/Scripts/masonry.pkgd.min.js",
           "~/Scripts/imagesloaded.js",
           "~/Scripts/classie.js",
           "~/Scripts/AnimOnScroll.js"));

            bundles.Add(new ScriptBundle("~/bundles/messagepartial").Include(
            "~/Scripts/Profile/Message.js"));


            //bundles.Add(new ScriptBundle("~/bundles/login-forgery").Include(
            //          "~/Scripts/Main/login-forgery.js"));

            //bundles.Add(new ScriptBundle("~/bundles/login-post").Include(
            //           "~/Scripts/Main/login-post.js"));

            bundles.Add(new StyleBundle("~/Content/Assets/Recycle").Include(
                      "~/Content/Styles/Main/bootstrap.css",
                        "~/Content/Assets/css/bootstrap-editable.css",
                      "~/Content/Styles/Main/site.css",
                      "~/Content/toastr.min.css",
                       "~/Content/Styles/Main/recycleme.css"));

            bundles.Add(new StyleBundle("~/Content/Styles/Recycle").Include(
                      "~/Content/Styles/Home/Header.css"));

            bundles.Add(new StyleBundle("~/Content/Assets/Fonts").Include(
                     "~/Content/Assets/css/font-awesome.min.css"));

            bundles.Add(new StyleBundle("~/Content/Assets/MainFonts").Include(
                    "~/Content/Assets/css/recycleme-fonts.css"));

            bundles.Add(new StyleBundle("~/Content/knockout-kendo").Include(
                   "~/Content/kendo.common.min.css",
                   "~/Content/kendo.default.min.css",
                   "~/Content/kendo.dataviz.min.css"));

            bundles.Add(new StyleBundle("~/Content/Styles/Trade").Include(
                      "~/Content/Styles/Home/default.css",
                      "~/Content/Styles/Home/component.css",
                      "~/Content/Styles/Home/TradeItems.css"));

            bundles.Add(new StyleBundle("~/Content/Login").Include(
                      "~/Content/Assets/css/bootstrap-editable.css",
                      "~/Content/Styles/Account/LoginReg.css"));

            bundles.Add(new StyleBundle("~/Content/Dashboard").Include(
                     "~/Content/Styles/Profile/Dashboard.css"));

            bundles.Add(new StyleBundle("~/Content/ProfileBar").Include(
                   "~/Content/Styles/Profile/ProfileBar.css"));

            bundles.Add(new StyleBundle("~/Content/Statistics").Include(
                 "~/Content/Styles/Profile/Statistics.css"));

            bundles.Add(new StyleBundle("~/Content/UsersPartial").Include(
                "~/Content/Styles/Profile/Users.css"));

            bundles.Add(new StyleBundle("~/Content/PillPartial").Include(
               "~/Content/Styles/Profile/Pill.css",
               "~/Content/Styles/Profile/tag-basic-style.css"));

            bundles.Add(new StyleBundle("~/Content/timeline").Include(
              "~/Content/Styles/Profile/TimeLine.css"));

            bundles.Add(new StyleBundle("~/Content/dropzone").Include(
            "~/Content/Assets/css/dropzoneMJZ.css"));

            bundles.Add(new StyleBundle("~/Content/tradeexchage").Include(
           "~/Content/Styles/Profile/Exchange.css",
           "~/Content/Chart.css"));

            bundles.Add(new StyleBundle("~/Content/trade").Include(
           "~/Content/Styles/Profile/Trade.css"));

            bundles.Add(new StyleBundle("~/Content/bid").Include(
           "~/Content/Styles/Home/default.css",
           "~/Content/Styles/Home/component.css",
           "~/Content/Styles/Profile/BidItems.css"));



            bundles.Add(new StyleBundle("~/Content/message").Include(
           "~/Content/Styles/Profile/Message.css"));

            //bundles.Add(new ScriptBundle("~/Content/Assets/Ace/extra").Include(
            //         "~/Content/Assets/js/ace-extra.min.js"));


            //bundles.Add(new ScriptBundle("~/Content/Assets/Ace/js").Include(
            //         "~/Content/Assets/js/ace-elements.min.js",
            //         "~/Content/Assets/js/ace.min.js"));

            BundleTable.EnableOptimizations = true;

        }
    }
}
