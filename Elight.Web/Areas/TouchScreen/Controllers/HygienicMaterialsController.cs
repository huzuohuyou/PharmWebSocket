using Elight.Web.Controllers;
using System.Web.Mvc;
using Elight.Entity;
using Elight.Infrastructure; 
using Elight.Web.Filters; 
using TouchScreen_uery.Entity;
using TouchScreenQuery.Service;

namespace Elight.Web.Areas.TouchScreen.Controllers
{
    public class HygienicMaterialsController : BaseController
    {

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 卫生材料
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HygienicMaterials()
        {
            return View();
        }
        
        

        [HttpPost, AuthorizeChecked]
        public ActionResult Index(int pageIndex, int pageSize, string keyWord)
        {
            if (keyWord == "")
            {
                return Content("".ToJson()); ;
            }
            var service = new HygienicMaterialsService();
            var pageData = service.GetList(pageIndex, pageSize, keyWord);
            var count = service.GetCout(keyWord);
            var result = new LayPadding<HygienicMaterialsEntity>()
            {
                result = true,
                msg = "success",
                list = pageData,
                count = count
            };
            return Content(result.ToJson());
        }

        [HttpPost, AuthorizeChecked]
        public ActionResult GetList(int pageIndex, int pageSize, string keyWord)
        {
            if (keyWord == "")
            {
                return Content("".ToJson()); ;
            }
            var service = new HygienicMaterialsService();
            var pageData = service.GetList(pageIndex, pageSize, keyWord);
            var count = service.GetCout(keyWord);
            var result = new LayPadding<HygienicMaterialsEntity>()
            {
                result = true,
                msg = "success",
                list = pageData,
                count = count
            };
            return Content(result.ToJson());
        }

        /// <summary>
        ///拼音查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        [HttpPost, AuthorizeChecked]
        public ActionResult PinYin(int pageIndex, int pageSize, string keyWord)
        {
            keyWord = keyWord.ToUpper();
            if (keyWord == "")
            {
                return Content("".ToJson()); ;
            }
            var service = new HygienicMaterialsService();
            var pageData = service.PinYin(pageIndex, pageSize, keyWord);
            var count = service.GetPinYinCout(keyWord);
            var result = new LayPadding<HygienicMaterialsEntity>()
            {
                result = true,
                msg = "success",
                list = pageData,
                count = count
            };
            return Content(result.ToJson());
        }

    }
}