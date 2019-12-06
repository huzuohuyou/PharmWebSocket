using Elight.Entity;
using Elight.Infrastructure;
using Elight.Web.Controllers;
using Elight.Web.Filters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TouchScreen_uery.Entity;
using TouchScreenQuery.Service;

namespace Elight.Web.Areas.TouchScreen.Controllers
{
    /// <summary>
    /// 专家介绍
    /// </summary>
    public class ExpertIntroductionController : BaseController
    {
        // GET: TouchScreen/ExpertIntroduction
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetDepts(int pageIndex, int pageSize, string keyWord)
        {

            var service = new DeptService();
            var pageData = service.GetList(pageIndex, pageSize, keyWord);
            var count = service.GetCout(keyWord);
            var result = new LayPadding<DeptEntity>()
            {
                result = true,
                msg = "success",
                list = pageData,
                count = count
            };
            return Content(result.ToJson());
        }

        /// <summary>
        /// 妇儿
        /// </summary>
        /// <returns></returns>
        public ActionResult ExpertList()
        {
            return View();
        }

        
        /// <summary>
        /// 获取专家
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        [HttpPost, AuthorizeChecked]
        public ActionResult GetExperts(int pageIndex, int pageSize, string deptType,string deptName)
        {
            if (deptType == ""|| deptName=="")
            {
                return Content("".ToJson()); ;
            }
            var service = new ExpertIntroductionService();
            var pageData = service.GetList(pageIndex, pageSize, deptType, deptName);
            var count = service.GetCout(deptType, deptName);
            var result = new LayPadding<ExpertEntity> ()
            {
                result = true,
                msg = "success",
                list = pageData,
                count = count
            };
            return Content(result.ToJson());
        }



        /// <summary>
        /// 获取专家详细信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        [HttpPost, AuthorizeChecked]
        public ActionResult GetForm( string idnumber,string name)
        {
            if (idnumber == ""|| name == "")
            {
                return Content("".ToJson()); ;
            }
            var entity =new ExpertIntroductionService().GetDetail(idnumber, name);
            return Content(entity.ToJson());
        }


        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }

    }
}