using System.Web;
using System.Web.Optimization;

namespace SiGotvime__Web_
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {


            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/style.min.css",
                        "~/Content/animate.min.css",
                        "~/Content/Site.css",
                        "~/Content/loaders/loaders.min.css",
                        "~/Content/backTop.min.css",
                        "~/Content/remodal.min.css",
                        "~/Content/remodal-default-theme.min.css",
                        "~/Content/jquery.mCustomScrollbar.min.css",
                        "~/Content/angucomplete-alt.min.css"
                        //"~/Content/Icons/categoryIcons.sprite.min.css"
                      )
                      .Include("~/Content/bootstrap.min.css", new CssRewriteUrlTransform())
                      );


            bundles.Add(new ScriptBundle("~/Bundles/js").Include(
                    "~/Scripts/moment.min.js",
                    "~/Scripts/MyScripts/GoogleAnalytics.js",
                    "~/Scripts/jquery-1.11.1.min.js",
                    "~/Scripts/jquery.unobtrusive-ajax.min.js",
                    "~/Scripts/jquery.uniform.min.js",
                    "~/Scripts/jquery.validate.min.js",
                    "~/Scripts/validation/languages/jquery.validationEngine-en.min.js",
                    "~/Scripts/validation/jquery.validationEngine.min.js",
                    "~/Scripts/wow.min.js",
                    "~/Scripts/jquery.slicknav.min.js",
                    "~/Scripts/scripts.min.js",
                    "~/Scripts/jquery.backTop.min.js",
                    "~/Scripts/jquery.mCustomScrollbar.concat.min.js",
                    "~/Scripts/bootstrap.min.js",
                    "~/Scripts/MyScripts/BlockUI.min.js",
                    "~/Scripts/MyScripts/app.min.js",

                    "~/Scripts/angular.min.js",
                    "~/Scripts/AngularApp/angucomplete-alt.min.js",
                    "~/Scripts/AngularApp/app.js",
                    "~/Scripts/Facebook/FBLogin.js",
                    "~/Scripts/Twitter/Twitter.js"
                )               
                );

#if !DEBUG
            BundleTable.EnableOptimizations = true;
#endif

        }
    }
}
