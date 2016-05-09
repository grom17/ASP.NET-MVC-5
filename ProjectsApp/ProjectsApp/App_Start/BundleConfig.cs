using System.Web.Optimization;

namespace ProjectsApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/staff").IncludeDirectory(
            "~/Scripts/Staff", "*.js", true));

            bundles.Add(new ScriptBundle("~/bundles/common").IncludeDirectory(
            "~/Scripts/Common", "*.js", true));

            bundles.Add(new ScriptBundle("~/bundles/noty").IncludeDirectory(
            "~/Scripts/Noty", "*.js", true));

            bundles.Add(new ScriptBundle("~/bundles/dtables").IncludeDirectory(
            "~/Scripts/DataTables", "*.js", true));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/dtables").IncludeDirectory(
                      "~/Content/DataTables", "*.css", true));
        }
    }
}
