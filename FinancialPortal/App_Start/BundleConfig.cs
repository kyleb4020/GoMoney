using System.Web;
using System.Web.Optimization;

namespace FinancialPortal
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/FinancialPortalJS.js",
                      "~/Scripts/jquery.metisMenu.js",
                      "~/Scripts/pace.js",
                      "~/Scripts/siminta.js",
                      "~/Scripts/raphael-2.1.0.min.js",
                      "~/Scripts/morris.js",
                      "~/Scripts/jquery.dataTables.min.js",
                      "~/Scripts/dataTables.responsive.min.js",
                      "~/Scripts/dataTables.buttons.min.js",
                      "~/Scripts/buttons.colVis.min.js",
                      "~/Scripts/excanvas.min.js",
                      "~/Scripts/bootstrap-multiselect.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      //"~/Content/site.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/pace-theme-big-counter.css",
                      "~/Content/style.css",
                      "~/Content/main-style.css",
                      "~/Content/morris-0.4.3.min.css",
                      "~/Content/dataTables.bootstrap.css",
                      "~/Content/jquery.dataTables.min.css",
                      "~/Content/responsive.dataTables.min.css",
                      "~/Content/buttons.dataTables.min.css",
                      "~/Content/social-button.css",
                      "~/Content/timeline.css",
                      "~/Content/bootstrap-multiselect.css"));
        }
    }
}
