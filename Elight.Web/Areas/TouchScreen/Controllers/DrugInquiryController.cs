using Elight.Entity;
using Elight.Infrastructure;
using Elight.Web.Controllers;
using Elight.Web.Filters;
using System.Web.Mvc;

namespace Elight.Web.Areas.TouchScreen.Controllers
{
    public class DrugInquiryController : BaseController
    {
        // GET: TouchScreen/DrugInquiry
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 西药
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult WesternMedicine()
        {
            return View();
        }

        /// <summary>
        /// 中成药
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ChinesePatentMedicine()
        {
            return View();
        }

        /// <summary>
        /// 中草药
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ChineseHerbalMedicine()
        {
            return View();
        }

        
        /// <summary>
        /// 检查治疗
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
            var service = new DrugInquiryService();
            var pageData = service.PinYin(pageIndex, pageSize, keyWord);
            var count = service.GetPinYinCout(keyWord);
            var result = new LayPadding<DrugEntity>()
            {
                result = true,
                msg = "success",
                list = pageData,
                count = count
            };
            return Content(result.ToJson());
        }
        /// <summary>
        /// 检查治疗
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        [HttpPost, AuthorizeChecked]
        public ActionResult GetWesternMedicine(int pageIndex, int pageSize, string keyWord)
        {
            if (keyWord == "")
            {
                return Content("".ToJson()); ;
            }
            var service = new DrugInquiryService();
            var pageData = service.GetList(pageIndex, pageSize, keyWord);
            var count = service.GetCout(keyWord);
            var result = new LayPadding<DrugEntity>()
            {
                result = true,
                msg = "success",
                list = pageData,
                count = count
            };
            return Content(result.ToJson());
        }

        /// <summary>
        /// 检查治疗
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        [HttpPost, AuthorizeChecked]
        public ActionResult GetChinesePatentMedicine(int pageIndex, int pageSize, string keyWord)
        {
            if (keyWord == "")
            {
                return Content("".ToJson()); ;
            }
            var service = new DrugInquiryService();
            var pageData = service.GetList(pageIndex, pageSize, keyWord);
            var count = service.GetCout(keyWord);
            var result = new LayPadding<DrugEntity>()
            {
                result = true,
                msg = "success",
                list = pageData,
                count = count
            };
            return Content(result.ToJson());
        }

        /// <summary>
        /// 检查治疗
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        [HttpPost, AuthorizeChecked]
        public ActionResult GetChineseHerbalMedicine(int pageIndex, int pageSize, string keyWord)
        {
            if (keyWord == "")
            {
                return Content("".ToJson()); ;
            }
            var service = new DrugInquiryService();
            var pageData = service.GetList(pageIndex, pageSize, keyWord);
            var count = service.GetCout(keyWord);
            var result = new LayPadding<DrugEntity>()
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