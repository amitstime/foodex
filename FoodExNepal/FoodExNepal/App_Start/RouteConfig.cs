using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FoodExNepal
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            /*routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
            name: "TestRoute",
            url: "Admin/{controller}/{action}/{id}",
            defaults: new { controller = "Banner", action = "Index", id = UrlParameter.Optional }
        );*/
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}