using Elight.Infrastructure;
using Elight.Web.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TouchScreen_uery.Entity;

namespace Elight.Web.Areas.TouchScreen.Controllers
{
    /// <summary>
    /// 满意度
    /// </summary>
    public class SatisfactionController : BaseController
    {
        // GET: TouchScreen/Satisfaction
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Questionnaire()
        {
            return View();
        }

        [HttpPost]
        public void GetForm()
        {

            Response.Redirect("http://localhost:17924/Home/Index");
        }

        public void SubmitBaseInfo(SatisfactionEntity entity)
        {
            LogHelper.Info(JsonConvert.SerializeObject(entity));
        }

    }
}