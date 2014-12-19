using System.Web;
using System.Web.Optimization;

namespace dotRDb.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/lib").Include(
                        "~/bower_components/angular/angular.js",
                        "~/bower_components/lodash/dist/lodash.min.js",
                        "~/bower_components/angular-route/angular-route.min.js",
                        "~/bower_components/ui-router/release/angular-ui-router.js",
                        "~/bower_components/angular-route/angular-route.js",
                        "~/bower_components/jquery/dist/jquery.min.js"
                        ));
            bundles.Add(new ScriptBundle("~/bundles/ui-bootstrap").Include(
                        "~/bower_components/ui-bootstrap/src/ui-bootstrap-0.12.0.js",
                        "~/bower_components/ui-bootstrap/src/ui-bootstrap-tpls-0.12.0.min.js",
                        "~/bower_components/ui-bootstrap/src/accordion/accordion.js",
                        "~/bower_components/ui-bootstrap/src/buttons/buttons.js",
                        "~/bower_components/ui-bootstrap/src/collapse/collapse.js",
                        "~/bower_components/ui-bootstrap/src/modal/modal.js",
                        "~/bower_components/ui-bootstrap/src/pagination/pagination.js",
                        "~/bower_components/ui-bootstrap/src/progressbar/progressbar.js",
                        "~/bower_components/ui-bootstrap/src/tabs/tabs.js",
                        "~/bower_components/ui-bootstrap/src/tooltip/tooltip.js",
                        "~/bower_components/ui-bootstrap/src/typeahead/typeahead.js"
                        ));
            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                        "~/Scripts/app/app.js",
                        "~/Scripts/app/genericService.js",
                        "~/Scripts/app/resource/resourceServices.js",
                        "~/Scripts/app/resource/resourceController.js",
                        "~/Scripts/app/datum/dataController.js",
                        "~/Scripts/app/datum/dataServices.js",
                        "~/Scripts/app/graph/node/nodeController.js",
                        "~/Scripts/app/graph/node/nodeServices.js",
                        "~/Scripts/app/graph/edge/edgeController.js",
                        "~/Scripts/app/graph/edge/edgeServices.js",
                        "~/Scripts/app/graph/graphServices.js"
                        ));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            BundleTable.EnableOptimizations = false;
        }
    }
}