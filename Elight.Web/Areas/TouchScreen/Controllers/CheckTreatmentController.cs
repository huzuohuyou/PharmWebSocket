using Elight.Entity;
using Elight.Infrastructure;
using Elight.Web.Controllers;
using Elight.Web.Filters;
using System.Web.Mvc;

namespace Elight.Web.Areas.TouchScreen.Controllers
{
    /// <summary>
    /// 检查治疗
    /// </summary>
    public class CheckTreatmentController :  BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 检查治疗
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CheckTreatment()
        {
            return View();
        }

        /// <summary>
        /// 病理检验
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PathologicalExamination()
        {
            return View();
        }

        /// <summary>
        /// 手术
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Surgery()
        {
            return View();
        }

        /// <summary>
        /// 拼音查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PinyinQuery()
        {
            return View();
        }

        /// <summary>
        /// 收费详情
        /// </summary>
        /// <param name="name">项目名称</param>
        /// <param name="yiju">收费依据</param>
        /// <param name="neihan">项目内涵</param>
        /// <param name="except">除外内容</param>
        /// <param name="shuoming">价格说明</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Detail(string BCODE)
        {
            if (BCODE == ""|| BCODE == null)
            {
                ViewBag.SUNAMEK = "查无数据";
                ViewBag.REMARK = "";
                ViewBag.CHARGEBASIS = "";
                ViewBag.EXCEPT = "";
                ViewBag.PriceStructure = "";

            }
            else
            {
                var service = new CheckTreatmentService();
                var Data = service.Get(BCODE);

                ViewBag.SUNAMEK = Data.SUNAMEK;
                ViewBag.REMARK = Data.REMARK;
                ViewBag.CHARGEBASIS = Data.CHARGEBASIS;
                ViewBag.CONTAINS = Data.CONTAINS;
                ViewBag.EXCEPT = Data.EXCEPT;
                ViewBag.PriceStructure = Data.PriceStructure;

            }

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
        public ActionResult Index(int pageIndex, int pageSize, string keyWord)
        {
            if (keyWord=="")
            {
                return Content("".ToJson()); ;
            }
            var service = new CheckTreatmentService();
            var pageData = service.GetList(pageIndex, pageSize, keyWord);
            var count = service.GetCout(keyWord);
            var result = new LayPadding<ItemEntity>()
            {
                result = true,
                msg = "success",
                list = pageData,
                count = count
            };
            return Content(result.ToJson());
        }

        /// <summary>
        /// 病理检验
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        [HttpPost, AuthorizeChecked]
        public ActionResult GetList(int pageIndex, int pageSize, string keyWord)
        {
            if (keyWord == "")
            {
                return Content("".ToJson()); ;
            }
            var service = new PathologicalExaminationService();
            var pageData = service.GetList(pageIndex, pageSize, keyWord);
            var count = service.GetCout(keyWord);
            var result = new LayPadding<ItemEntity>()
            {
                result = true,
                msg = "success",
                list = pageData,
                count = count
            };
            return Content(result.ToJson());
        }

        /// <summary>
        /// 手术
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        [HttpPost, AuthorizeChecked]
        public ActionResult GetSurgerys(int pageIndex, int pageSize, string keyWord)
        {
            if (keyWord == "")
            {
                return Content("".ToJson()); ;
            }
            var service = new SurgeryService();
            var pageData = service.GetList(pageIndex, pageSize, keyWord);
            var count = service.GetCout(keyWord);
            var result = new LayPadding<ItemEntity>()
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
            var service = new CheckTreatmentService();
            var pageData = service.PinYin(pageIndex, pageSize, keyWord);
            var count = service.GetPinYinCout(keyWord);
            var result = new LayPadding<ItemEntity>()
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