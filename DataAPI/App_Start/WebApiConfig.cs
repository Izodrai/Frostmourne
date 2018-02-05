using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace DataAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configuration et services de l'Web API 

            // Itinéraires de l'Web API 
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "ActionApi",
                routeTemplate: "api/{controller}/{action}/{xtb_id}/{xtb_log}/{arg1}/{arg2}/{arg3}/",
                defaults: new { xtb_id = RouteParameter.Optional, xtb_log = RouteParameter.Optional, arg1 = RouteParameter.Optional, arg2 = RouteParameter.Optional, arg3 = RouteParameter.Optional }
            );
        }
    }
}
