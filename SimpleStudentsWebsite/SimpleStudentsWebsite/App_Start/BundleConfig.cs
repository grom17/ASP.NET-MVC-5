using System.Web.Optimization;

namespace SimpleStudentsWebsite
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/jquery").IncludeDirectory(
                        "~/Scripts/JQuery","*.js", true));

            bundles.Add(new ScriptBundle("~/bundles/common").IncludeDirectory(
                        "~/Scripts/Common", "*.js", true));

            bundles.Add(new ScriptBundle("~/bundles/noty").IncludeDirectory(
                        "~/Scripts/DataTables", "*.js", true));

            bundles.Add(new ScriptBundle("~/bundles/datatables").IncludeDirectory(
                        "~/Scripts/noty", "*.js", true));

            bundles.Add(new ScriptBundle("~/bundles/login").IncludeDirectory(
                        "~/Scripts/Login", "*.js", true));

            bundles.Add(new ScriptBundle("~/bundles/students").IncludeDirectory(
                        "~/Scripts/Students", "*.js", true));

            bundles.Add(new ScriptBundle("~/bundles/teachers").IncludeDirectory(
                        "~/Scripts/Teachers", "*.js", true));

            bundles.Add(new ScriptBundle("~/bundles/reports").IncludeDirectory(
                        "~/Scripts/Reports", "*.js", true));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").IncludeDirectory(
                      "~/Scripts/Bootstrap", "*.js", true));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/site.css",
                      "~/Content/slimtable.css", 
                      "~/Content/fSelect.css"));

            bundles.Add(new StyleBundle("~/Content/bootstrap").IncludeDirectory(
                      "~/Content/Bootstrap","*.css",true));

            bundles.Add(new StyleBundle("~/Content/datatables").IncludeDirectory(
                      "~/Content/DataTables", "*.css", true));
        }
    }
}
