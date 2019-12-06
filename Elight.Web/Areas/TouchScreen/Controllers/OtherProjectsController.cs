using Elight.Entity;
using Elight.Infrastructure;
using Elight.Web.Controllers;
using Elight.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TouchScreen_uery.Entity;
using TouchScreenQuery.Service;

namespace Elight.Web.Areas.TouchScreen.Controllers
{
    /// <summary>
    /// 其他项目
    /// </summary>
    public class OtherProjectsController : BaseController
    {

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 其他项目
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult OtherProjects()
        { 
            return View();
        }

        /// <summary>
        /// 其他项目
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult OtherProjectsShowInformation()
        { 
            return View();
        }

        /// <summary>
        /// 详细信息
        /// </summary>
        /// <param name="name">项目名称</param>
        /// <param name="yiju">收费依据</param>
        /// <param name="neihan">项目内涵</param>
        /// <param name="except">除外内容</param>
        /// <param name="shuoming">价格说明</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Detail(string SUNAMEK, string CHARGEBASIS, string EXCEPT, string CONTAINS, string PriceStructure)
        {
            ViewBag.SUNAMEK = SUNAMEK;
            ViewBag.CHARGEBASIS = CHARGEBASIS;
            ViewBag.EXCEPT = EXCEPT;
            ViewBag.CONTAINS = CONTAINS;
            ViewBag.PriceStructure = PriceStructure;

            return View();
        }

        [HttpPost, AuthorizeChecked]
        public ActionResult Index(int pageIndex, int pageSize, string keyWord)
        {
            if (keyWord == "")
            {
                return Content("".ToJson()); ;
            }
            var service = new OtherProjectsService();
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

        [HttpPost, AuthorizeChecked]
        public ActionResult GetList(int pageIndex, int pageSize, string keyWord)
        {
            if (keyWord == "")
            {
                return Content("".ToJson()); ;
            }
            var service = new OtherProjectsService();
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
    }
}