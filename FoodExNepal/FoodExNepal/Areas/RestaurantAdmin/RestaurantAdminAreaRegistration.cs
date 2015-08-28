using System.Web.Mvc;

namespace FoodExNepal.Areas.RestaurantAdmin
{
    public class RestaurantAdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "RestaurantAdmin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "RestaurantAdmin_default",
                "RestaurantAdmin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
