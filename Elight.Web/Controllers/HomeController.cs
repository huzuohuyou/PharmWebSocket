using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Elight.Entity;
using Elight.Web.Filters;
using Elight.IService;
using Elight.Infrastructure;
using System.Text;

namespace Elight.Web.Controllers
{
    [LoginChecked]
    public class HomeController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IUserLogOnService _userLogOnService;
        private readonly IUserRoleRelationService _userRoleRelationService;
        private readonly ILogService _logService;
        private readonly IPermissionService _permissionService;

        public HomeController(IUserService userService, IUserLogOnService userLogOnService,
            IUserRoleRelationService userRoleRelationService, ILogService logService, IPermissionService permissionService)
        {
            this._userService = userService;
            this._userLogOnService = userLogOnService;
            this._userRoleRelationService = userRoleRelationService;
            this._logService = logService;
            this._permissionService = permissionService;
        }

        /// <summary>
        /// 后台首页视图。
        /// </summary>
        /// <returns></returns>
        [HttpGet,LoginChecked(false),Access]
        public ActionResult Index()
        {
            //if (OperatorProvider.Instance.Current != null)
            //{
            ViewBag.SoftwareName = Configs.GetValue("SoftwareName");
            //ViewBag.Account = OperatorProvider.Instance.Current.Account;
            //ViewBag.Avatar = OperatorProvider.Instance.Current.Avatar;
            return View();
            //}
            //else
            //{
            //    return Redirect("/Account/Login");
            //}
        }

        [HttpGet, LoginChecked(false)]
        public ActionResult Refresh()
        {
            return Redirect("Index");
            StringBuilder script = new StringBuilder();
            script.Append("<script> top.window.location.href='/Home/Index'</script>");
            return new ContentResult() { Content = script.ToString() };
        }





        /// <summary>
        /// 默认显示视图。
        /// </summary>
        /// <returns></returns>
        [HttpGet, LoginChecked(false)]
        public ActionResult Default()
        {
            return View();
        }

        /// <summary>
        /// 获取左侧菜单。
        /// </summary>
        /// <returns></returns>
        [HttpPost, LoginChecked(false)]
        public ActionResult GetLeftMenu()
        {
            //string userId = OperatorProvider.Instance.Current.UserId;

            List<LayNavbar> listNavbar = new List<LayNavbar>();
            //var listModules = _permissionService.GetList(userId);
            var listModules = new List<Sys_Permission> {
                new Sys_Permission{
                    ParentId="0",
                    Layer=0,
                    Name="物价查询",
                    Icon="fa fa-balance-scale",
                    IsEdit=true,
                    IsEnable=true,
                    IsPublic=true,
                    Url="",
                    Id="wujiachaxun",
                    Type=0,
                },
                new Sys_Permission{
                    ParentId="wujiachaxun",
                    Layer=1,
                    Name="物价文件",
                    Icon="fa fa-file-text-o",
                    IsEdit=true,
                    IsEnable=true,
                    IsPublic=true,
                    Url="/TouchScreen/PriceDocument/Index",
                    Id="wujiawenjianchaxun",
                    Type=0,
                },
                new Sys_Permission{
                    ParentId="wujiachaxun",
                    Layer=1,
                    Name="检查治疗",
                    Icon="fa fa-stethoscope",
                    IsEdit=true,
                    IsEnable=true,
                    IsPublic=true,
                    Url="/TouchScreen/CheckTreatment/Index",
                    Id="jianchazhiliaochaxun",
                    Type=0,
                },
                new Sys_Permission{
                    ParentId="wujiachaxun",
                    Layer=1,
                    Name="药品",
                    Icon="fa fa-medkit",
                    IsEdit=true,
                    IsEnable=true,
                    IsPublic=true,
                    Url="/TouchScreen/DrugInquiry/Index",
                    Id="yaopinchaxun",
                    Type=0,
                },
                new Sys_Permission{
                    ParentId="wujiachaxun",
                    Layer=1,
                    Name="卫生材料",
                    Icon="fa fa-shopping-basket",
                    IsEdit=true,
                    IsEnable=true,
                    IsPublic=true,
                    Url="/TouchScreen/HygienicMaterials/Index",
                    Id="weishengcailiaochaxun",
                    Type=0,
                },
                new Sys_Permission{
                    ParentId="wujiachaxun",
                    Layer=1,
                    Name="其他项目",
                    Icon="fa fa-star",
                    IsEdit=true,
                    IsEnable=true,
                    IsPublic=true,
                    Url="/TouchScreen/OtherProjects/Index",
                    Id="qitaxiangmuchaxun",
                    Type=0,
                },
                new Sys_Permission{
                    ParentId="wujiachaxun",
                    Layer=1,
                    Name="患者费用",
                    Icon="fa fa-credit-card",
                    IsEdit=true,
                    IsEnable=true,
                    IsPublic=true,
                    Url="/TouchScreen/PatientFee/Index",
                    Id="huanzhefeiyongchaxun",
                    Type=0,
                },


                new Sys_Permission{
                    ParentId="0",
                    Layer=0,
                    Name="专家介绍",
                    Icon="fa fa-group",
                    IsEdit=true,
                    IsEnable=true,
                    IsPublic=true,
                    Url="",
                    Id="zhuanjiajieshao",
                    Type=0,
                },

                new Sys_Permission{
                    ParentId="zhuanjiajieshao",
                    Layer=1,
                    Name="妇儿",
                    Icon="fa fa-venus",
                    IsEdit=true,
                    IsEnable=true,
                    IsPublic=true,
                    Url="/TouchScreen/ExpertIntroduction/ExpertList?keyWord=妇儿",
                    Id="fuke",
                    Type=0,
                },
                new Sys_Permission{
                    ParentId="zhuanjiajieshao",
                    Layer=1,
                    Name="内科系统",
                    Icon="fa fa-plus-square",
                    IsEdit=true,
                    IsEnable=true,
                    IsPublic=true,
                    Url="/TouchScreen/ExpertIntroduction/ExpertList?keyWord=内科系统",
                    Id="neikexitong",
                    Type=0,
                },
                new Sys_Permission{
                    ParentId="zhuanjiajieshao",
                    Layer=1,
                    Name="外科系统",
                    Icon="fa fa-plus-circle",
                    IsEdit=true,
                    IsEnable=true,
                    IsPublic=true,
                    Url="/TouchScreen/ExpertIntroduction/ExpertList?keyWord=外科系统",
                    Id="waikexitong",
                    Type=0,
                },
                new Sys_Permission{
                    ParentId="zhuanjiajieshao",
                    Layer=1,
                    Name="五官科",
                    Icon="fa fa-meh-o",
                    IsEdit=true,
                    IsEnable=true,
                    IsPublic=true,
                    Url="/TouchScreen/ExpertIntroduction/ExpertList?keyWord=五官科",
                    Id="wuguanke",
                    Type=0,
                },
                new Sys_Permission{
                    ParentId="zhuanjiajieshao",
                    Layer=1,
                    Name="医技科室",
                    Icon="fa fa-heartbeat",
                    IsEdit=true,
                    IsEnable=true,
                    IsPublic=true,
                    Url="/TouchScreen/ExpertIntroduction/ExpertList?keyWord=医技科室",
                    Id="yijikeshi",
                    Type=0,
                },
                new Sys_Permission{
                    ParentId="zhuanjiajieshao",
                    Layer=1,
                    Name="其他科室",
                    Icon="fa fa-ambulance",
                    IsEdit=true,
                    IsEnable=true,
                    IsPublic=true,
                    Url="/TouchScreen/ExpertIntroduction/ExpertList?keyWord=其他科室",
                    Id="qitakeshi",
                    Type=0,
                },


                new Sys_Permission{
                    ParentId="0",
                    Layer=0,
                    Name="满意度调查",
                    Icon="fa fa-user",
                    IsEdit=true,
                    IsEnable=true,
                    IsPublic=true,
                    Url="",
                    Id="manyidudiaocha",
                    Type=0,
                },
                new Sys_Permission{
                    ParentId="manyidudiaocha",
                    Layer=1,
                    Name="填写问卷",
                    Icon="fa fa-user",
                    IsEdit=true,
                    IsEnable=true,
                    IsPublic=true,
                    Url="/TouchScreen/Satisfaction/Index",
                    Id="tianxiewenjuan",
                    Type=0,
                },
                //new Sys_Permission{
                //    ParentId="manyidudiaocha",
                //    Layer=1,
                //    Name="填写满意度",
                //    Icon="fa fa-user",
                //    IsEdit=true,
                //    IsEnable=true,
                //    IsPublic=true,
                //    Url="/TouchScreen/Satisfaction/Questionnaire",
                //    Id="tianxiewenjuan",
                //    Type=0,
                //},

                new Sys_Permission{
                    ParentId="0",
                    Layer=0,
                    Name="使用说明",
                    Icon="fa fa-leanpub",
                    IsEdit=true,
                    IsEnable=true,
                    IsPublic=true,
                    Url="",
                    Id="shiyongshuoming",
                    Type=0,
                },

                new Sys_Permission{
                    ParentId="shiyongshuoming",
                    Layer=1,
                    Name="文件与收费查询",
                    Icon="fa fa-leanpub",
                    IsEdit=true,
                    IsEnable=true,
                    IsPublic=true,
                    Url="/TouchScreen/Readme/Index",
                    Id="使用说明",
                    Type=0,
                },
            };
            //listModules = listModules.Union(my).ToList();
            foreach (var item in listModules.Where(c => c.Type == ModuleType.Menu && c.Layer == 0).ToList())
            {
                LayNavbar navbarEntity = new LayNavbar();
                var listChildNav = listModules.Where(c => c.Type == ModuleType.Menu && c.Layer == 1 && c.ParentId == item.Id)
                    .Select(c => new LayChildNavbar() { href = c.Url, icon = c.Icon, title = c.Name }).ToList();
                navbarEntity.icon = item.Icon;
                navbarEntity.spread = false;
                navbarEntity.title = item.Name;
                navbarEntity.children = listChildNav;
                listNavbar.Add(navbarEntity);
            }
            return Content(listNavbar.ToJson());
        }

        /// <summary>
        /// 获取登录用户权限。
        /// </summary>
        /// <returns></returns>
        [HttpPost, LoginChecked(false)]
        public ActionResult GetPermission()
        {
            //var userId = OperatorProvider.Instance.Current.UserId;
            //var modules = _permissionService.GetList(userId);
            //return Content(modules.ToJson());
            return Content("".ToJson());
        }
    }
}