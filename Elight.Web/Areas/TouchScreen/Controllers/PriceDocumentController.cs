using Elight.Web.Controllers;
using System.Web.Mvc;
using Elight.Entity;
using Elight.Infrastructure; 
using Elight.Web.Filters; 
using TouchScreen_uery.Entity;
using TouchScreenQuery.Service;

namespace Elight.Web.Areas.TouchScreen.Controllers
{
    public class PriceDocumentController : BaseController
    {

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 物价文件
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PriceDocument()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }
        /// <summary>
        /// 文件内容
        /// </summary> 
        /// <param name="shuoming">价格说明</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Detail(string WJNR)
        {
            ViewBag.WJNR = WJNR;

            return View();
        }

        [HttpPost, AuthorizeChecked]
        public ActionResult Index(int pageIndex, int pageSize, string keyWord)
        {
            if (keyWord == ""||keyWord!="2016")
            {
                return Content("".ToJson()); ;
            }
            var service = new PriceDocumentService();
            var pageData = service.GetList(pageIndex, pageSize, keyWord);
            var count = service.GetCout(keyWord);
            var result = new LayPadding<PriceDocumentEntity>()
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
            var service = new PriceDocumentService();
            var pageData = service.GetList(pageIndex, pageSize, keyWord);
            var count = service.GetCout(keyWord);
            var result = new LayPadding<PriceDocumentEntity>()
            {
                result = true,
                msg = "success",
                list = pageData,
                count = count
            };
            return Content(result.ToJson());
        }

        /// <summary>
        /// 获取文件内容
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        [HttpPost, AuthorizeChecked]
        public ActionResult GetForm(string WJBM, string WJNO, string WJMC)
        {
            if (WJBM == "" || WJNO == "" || WJMC == "")
            {
                //return Content("".ToJson());
            }
            var entity = new PriceDocumentService().GetDetail(WJBM, WJNO, WJMC);
            return Content(entity.ToJson());
        }

    }
}