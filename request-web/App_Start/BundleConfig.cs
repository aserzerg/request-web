using System.Web;
using System.Web.Optimization;

namespace request_web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css")
                .Include("~/Content/themes/base/jquery-ui.css", new CssRewriteUrlTransform())
                .Include("~/Content/themes/base/accordion.css", new CssRewriteUrlTransform())
                .Include("~/Content/themes/base/autocomplete.css", new CssRewriteUrlTransform())
                .Include("~/Content/themes/base/button.css", new CssRewriteUrlTransform())
                .Include("~/Content/themes/base/datepicker.css", new CssRewriteUrlTransform())
                .Include("~/Content/themes/base/dialog.css", new CssRewriteUrlTransform())
                .Include("~/Content/themes/base/draggable.css", new CssRewriteUrlTransform())
                .Include("~/Content/themes/base/menu.css", new CssRewriteUrlTransform())
                .Include("~/Content/themes/base/progressbar.css", new CssRewriteUrlTransform())
                .Include("~/Content/themes/base/resizable.css", new CssRewriteUrlTransform())
                .Include("~/Content/themes/base/selectable.css", new CssRewriteUrlTransform())
                .Include("~/Content/themes/base/selectmenu.css", new CssRewriteUrlTransform())
                .Include("~/Content/themes/base/sortable.css", new CssRewriteUrlTransform())
                .Include("~/Content/themes/base/slider.css", new CssRewriteUrlTransform())
                .Include("~/Content/themes/base/spinner.css", new CssRewriteUrlTransform())
                .Include("~/Content/themes/base/tabs.css", new CssRewriteUrlTransform())
                .Include("~/Content/themes/base/theme.css", new CssRewriteUrlTransform())
                .Include("~/Content/themes/base/tooltip.css", new CssRewriteUrlTransform())
                .Include("~/Content/DataTables/css/jquery.datatables.css", new CssRewriteUrlTransform())
                .Include("~/Content/DataTables/css/datatables.bootrstrap.css", new CssRewriteUrlTransform())
                .Include("~/Content/bootstrap.css", new CssRewriteUrlTransform())
                .Include("~/Content/site.css", new CssRewriteUrlTransform()));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/localization/messages_ru.js",
                "~/Scripts/jquery.validate.js",
                "~/Scripts/jquery.validate.unobtrusive.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryajax").Include(
                "~/Scripts/jquery.unobtrusive-ajax.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui")
                .Include("~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                "~/Scripts/DataTables/jquery.datatables.js",
                "~/Scripts/DataTables/datatables.bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/inputmasks").Include(
                "~/Scripts/jquery.maskedinput.js",
                "~/Scripts/inputmasks.js"));

            bundles.Add(new ScriptBundle("~/bundles/components")
                .IncludeDirectory("~/Scripts/Components", "*.js"));
        }
    }
}
