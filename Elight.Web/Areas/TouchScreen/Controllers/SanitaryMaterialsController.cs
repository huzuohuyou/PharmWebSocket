using Elight.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Elight.Web.Areas.TouchScreen.Controllers
{
    /// <summary>
    /// 卫生材料
    /// </summary>
    public class SanitaryMaterialsController : BaseController
    {
        // GET: TouchScreen/SanitaryMaterials
        public ActionResult Index()
        {
            return View();
        }

       
        
    }
}