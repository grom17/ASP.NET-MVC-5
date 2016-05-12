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

            bundles.Add(new ScriptBundle("~/bundles/projects").IncludeDirectory(
            "~/Scripts/Projects", "*.js", true));

            bundles.Add(new ScriptBundle("~/bundles/common").IncludeDirectory(
            "~/Scripts/Common", "*.js", true));

            bundles.Add(new ScriptBundle("~/bundles/noty").IncludeDirectory(
            "~/Scripts/Noty", "*.js", true));

            bundles.Add(new ScriptBundle("~/bundles/dtables").IncludeDirectory(
            "~/Scripts/DataTables", "*.js", true));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/moment").Include(
                        "~/Scripts/moment.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").IncludeDirectory(
                      "~/Scripts/Bootstrap","*.js", true));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                      "~/Content/Bootstrap/bootstrap-datetimepicker.min.css",
                      "~/Content/Bootstrap/bootstrap-dtp-wrap.min.css",
                      "~/Content/Bootstrap/bootstrap-wrap.min.css",
                      "~/Content/Bootstrap/bootstrap.css"));

            bundles.Add(new StyleBundle("~/Content/dtables").IncludeDirectory(
                      "~/Content/DataTables", "*.css", true));
        }
    }
}
