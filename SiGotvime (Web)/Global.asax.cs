using SiGotvime__Web_.Controllers;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Http;

namespace SiGotvime__Web_
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {            
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            UnityConfig.RegisterComponents();            
        }

        //protected void Application_BeginRequest(object sender, EventArgs e)
        //{

        //    // Implement HTTP compression
        //    HttpApplication app = (HttpApplication)sender;


        //    // Retrieve accepted encodings
        //    string encodings = app.Request.Headers.Get("Accept-Encoding");
        //    if (encodings != null)
        //    {
        //        // Check the browser accepts deflate or gzip (deflate takes preference)
        //        encodings = encodings.ToLower();
        //        if (encodings.Contains("deflate"))
        //        {
        //            app.Response.Filter = new DeflateStream(app.Response.Filter, CompressionMode.Compress);
        //            app.Response.AppendHeader("Content-Encoding", "deflate");
        //        }
        //        else if (encodings.Contains("gzip"))
        //        {
        //            app.Response.Filter = new GZipStream(app.Response.Filter, CompressionMode.Compress);
        //            app.Response.AppendHeader("Content-Encoding", "gzip");
        //        }
        //    }
        //}


        //public void Application_Error(Object sender, EventArgs e)
        //{
        //    Exception exception = Server.GetLastError();
        //    Server.ClearError();

        //    var routeData = new RouteData();
        //    routeData.Values.Add("controller", "Error");
        //    routeData.Values.Add("action", "Index");
        //    routeData.Values.Add("exception", exception);

        //    if (exception.GetType() == typeof(HttpException))
        //    {
        //        routeData.Values.Add("statusCode", ((HttpException)exception).GetHttpCode());
        //    }
        //    else
        //    {
        //        routeData.Values.Add("statusCode", 500);
        //    }

        //    Response.TrySkipIisCustomErrors = true;
        //    IController controller = new ErrorController();
        //    controller.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
        //    Response.End();
        //}
    }
}
