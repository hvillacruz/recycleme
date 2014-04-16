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
                    "~/Scripts/knockout-3.1.0.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/Assets/Recycle").Include(
                      "~/Content/Styles/Main/bootstrap.css",
                      "~/Content/Styles/Main/site.css",
                       "~/Content/Styles/Main/recycleme.css"));

            bundles.Add(new StyleBundle("~/Content/Styles/Recycle").Include(
                      "~/Content/Styles/Home/Header.css"));

            bundles.Add(new StyleBundle("~/Content/Assets/Fonts").Include(
                     "~/Content/Assets/css/font-awesome.min.css",
                     "~/Content/Assets/css/recycleme-fonts.css"));

            //bundles.Add(new StyleBundle("~/Content/Styles/RecycleMe").Include(
            //       "~/Content/Styles/Main/recycleme.css"));

            //bundles.Add(new ScriptBundle("~/Content/Assets/Ace/extra").Include(
            //         "~/Content/Assets/js/ace-extra.min.js"));


            //bundles.Add(new ScriptBundle("~/Content/Assets/Ace/js").Include(
            //         "~/Content/Assets/js/ace-elements.min.js",
            //         "~/Content/Assets/js/ace.min.js"));

            BundleTable.EnableOptimizations = true;

        }
    }
}
