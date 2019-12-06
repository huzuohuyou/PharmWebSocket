using System.Web.Mvc;

namespace Elight.Web.Areas.TouchScreen
{
    public class TouchScreenAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "TouchScreen";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "TouchScreen_default",
                "TouchScreen/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}