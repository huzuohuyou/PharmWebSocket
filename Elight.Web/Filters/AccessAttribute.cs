using Exceptionless;
using System.Web.Mvc;

namespace Elight.Web.Filters
{
    /// <summary>
    /// 访问记录
    /// </summary>
    public class AccessAttribute : ActionFilterAttribute
    {
        public AccessAttribute() {

        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //ExceptionlessClient.Default.Configuration.ApiKey = "sDgUq6b8thKMOMXkCXPqDvpGw1zIbnESankL8OBS";
            //ExceptionlessClient.Default.Configuration.ServerUrl = "http://localhost:8088";
            //filterContext.ActionDescriptor
            ExceptionlessClient.Default.CreateLog(filterContext.Controller.ToString(), filterContext.ActionDescriptor.ActionName).Submit();
        }
    }
}