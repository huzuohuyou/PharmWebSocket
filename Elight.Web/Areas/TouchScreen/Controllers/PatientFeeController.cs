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
    /// 患者费用查询
    /// </summary>
    public class PatientFeeController : BaseController
    {
        // GET: TouchScreen/PatientFee
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 住院查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult InPatientQuery()
        {
            return View();
        }

        /// <summary>
        /// 门诊查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult OutPatientQuery()
        {
            return View();
        }

        /// <summary>
        /// 门诊查询2
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult OutPatientQuery2()
        {
            return View();
        }


        /// <summary>
        /// 门诊费用分组
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        [HttpPost, AuthorizeChecked]
        public ActionResult GetOutSumFee(int pageIndex, int pageSize, string keyWord)
        {
            if (keyWord == "_")
            {
                return Content("".ToJson()); ;
            }
            var service = new PatientFeeService();
            var pageData = service.GetList(pageIndex, pageSize, keyWord);
            var count = service.GetCout(keyWord);
            var result = new LayPadding<FeeEntity>()
            {
                result = true,
                msg = "success",
                list = pageData,
                count = count
            };
            return Content(result.ToJson());
        }

        /// <summary>
        /// 门诊费用分组--查全部费用，去掉日期条件
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        [HttpPost, AuthorizeChecked]
        public ActionResult GetOutSumFeeAll( string keyWord)
        {
            if (keyWord == "_")
            {
                return Content("".ToJson()); ;
            }
            var service = new PatientFeeService();
            var pageData = service.GetListAll(1, 10000, keyWord);
            var count = service.GetCoutAll(keyWord);
            var result = new LayPadding<FeeEntity>()
            {
                result = true,
                msg = "success",
                list = pageData,
                count = count
            };
            return Content(result.ToJson());
        }

        /// <summary>
        /// 门诊费用详情
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        [HttpPost, AuthorizeChecked]
        public ActionResult GetOutFeeDetail(int pageIndex, int pageSize, string pno, string date, string seqno, string bi, string drcode)
        {

            if (pno == "" || date == "" || seqno == "" || bi == "" || drcode == "")
            {
                return Content("".ToJson()); ;
            }
            var param = new FeeParam {
                Pno=pno,
                Date=date,
                SeqNO=seqno,
                Bi=bi,
                DrCode=drcode
            };
            var keyWord = $@"{pno}_{date}_{seqno}_{bi}_{drcode}";
            var service = new PatientFeeService();
            var pageData = service.PinYin(pageIndex, pageSize, keyWord);
            var count = service.GetPinYinCout(keyWord);
            var result = new LayPadding<FeeEntity>()
            {
                result = true,
                msg = "success",
                list = pageData,
                count = count
            };
            return Content(result.ToJson());
        }

        //GetInFeeDetail
        /// <summary>
        /// 住院费用详情
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        [HttpPost, AuthorizeChecked]
        public ActionResult GetInFeeDetail(int pageIndex, int pageSize, string pno, string date,string admseqno)
        {

            if (pno == "" || date == "" || admseqno == "")
            {
                return Content("".ToJson()); ;
            }
            
            var keyWord = $@"{pno}_{date}_{admseqno}";
            var service = new PatientFeeService();
            var pageData = service.GetInList(pageIndex, pageSize, keyWord);
            //var count = pageData.Count;
            var count = service.GetInFeeListCout(keyWord);
            var result = new LayPadding<InFeeEntity>()
            {
                result = true,
                msg = "success",
                list = pageData,
                count = count
            };
            return Content(result.ToJson());
        }

        /// <summary>
        /// 住院费用汇总记录
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        [HttpPost, AuthorizeChecked]
        public ActionResult GetInFeeSummary(string keyWord)
        {
            if (keyWord == "_")
            {
                return Content("".ToJson()); ;
            }
            var service = new PatientFeeService();
            var pageData = service.GetInFeeListAll(1, 10000, keyWord);
            var count = service.GetInfeeCoutAll(keyWord);
            var result = new LayPadding<InFeeEntity>()
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