using Elight.Infrastructure;
using Elight.Web.Controllers;
using Pharmacy_Reporter.Entity;
using Pharmacy_Reporter.Service;
using System.Web.Mvc;

namespace Elight.Web.Areas.Pharm.Controllers
{
    public class CheckController  : BaseController
    {
        // GET: Pharm/Check
        public ActionResult Index()
        {
            return View();
        }

        

        [HttpPost]
        public ActionResult GetWindows(string ptno)
        {
           
            SigninStrip item = null;
            try
            {
                item = new PharmacyReporterService().PrintReportStrip(ptno);
            }
            catch (global::System.Exception ex)
            {
                item = new SigninStrip
                {
                    WINDOW_NAME = ex.Message
                };
            }
            return Content(item.ToJson());

        }


    }
}