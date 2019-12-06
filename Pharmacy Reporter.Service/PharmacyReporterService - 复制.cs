using App.FunctionLibrary;
using Framework.Service;
using Pharmacy_Reporter.Entity;
using Pharmacy_Reporter.ORM;
using System;
using System.Collections.Generic;

namespace Pharmacy_Reporter.Service
{
    /// <summary>
    /// 门诊报道机服务
    /// </summary>
    public class PharmacyReporterService: ServiceBase
    {
       
        /// <summary>
        /// 打印签到条
        /// </summary>
        public SigninStrip PrintReportStrip(string Ptno)
        {
            
            if (Ptno.IsEmpty())
                throw new Exception("患者ID号为空！");
            
            using (var db = new DataBaser())
            {
                db.BeginTrans();
                var print = false;
                var inPrint = false;
                var useedWindowCode = string.Empty;
                var inUseedWindowCode = string.Empty;
                var windowsSet = new HashSet<string>() ;
                var prenoSet = new HashSet<string>();
                var inOrderSet = new HashSet<string>();
                var orm = base.Create<PharmacyReporterManager>(db);
                if (!orm.ExistPatient(Ptno))
                    throw new Exception("未找到患者信息！");
                //获取患者姓名
                var sname = orm.GetPatientName(Ptno);
                //活动窗口的工作量
                var workingWins = orm.GetWorkingWindow();
                if (workingWins.Length<=0)
                    throw new Exception("药房窗口未开放,请与药房联系确认后重新签到！");
                //取药患者序号
                var index = orm.GetTuyakNo();
                //获取处方单
                var prenos = orm.GetPresNO(Ptno);
                var checkInfo = orm.GetInPatientCheckInfo(Ptno);
                var orders = orm.GetInpatientOrder(checkInfo);
                var checkedOrders= orm.GetInpatientCheckedOrder(checkInfo);
                //if (prenos.Length<=0&& orders.Length<=0&& !orm.HasInpatientCheckedOrder(checkInfo))
                //    throw new Exception("今日无处方、预约状态医嘱!");
                //确定取药窗口,使用取余编制序号算法按人平均分配队伍
                var currentWin = workingWins[index % workingWins.Length];
                //更新医嘱状态,取药状态
                for (int i = 0; i < prenos.Length; i++)
                {
                    //只有未签到的处方单才会更新状态
                    if (!orm.HadCheckedIn(Ptno, prenos[i].Presno))
                    {
                        prenoSet.Add(prenos[i].Presno);
                        print = true;
                    }
                    else
                    {
                        useedWindowCode = orm.GetCurrentWindow(Ptno, prenos[i].Presno);
                        windowsSet.Add($@"{ orm.GetWindowName(useedWindowCode)}");
                    }
                }
                //同一患者同一窗口取药机制
                if (print)
                {
                    var list = prenoSet.ToList();
                    for (int i = 0; i < list.Count; i++)
                    {
                        orm.OrderMedicationAppointment(Ptno, list[i]);
                        orm.PrslipMedicationAppointment(Ptno, list[i], index.ToString()
                            , useedWindowCode.IsEmpty()? currentWin.Code: useedWindowCode);
                    }
                }
                //日间手术操作
                if (orders.Length>0)
                {
                    if (orm.HasInpatientNotCheckedOrder(checkInfo))
                    {
                        orm.InsertIprslip(checkInfo);
                        orm.UpdateIOrderZG(checkInfo);
                    }
                }
                for (int i = 0; i < checkedOrders.Length; i++)
                {
                    if (orm.InPatientHadCheckedIn(Ptno, checkedOrders[i].ORDERNO))
                    {
                        inOrderSet.Add(checkedOrders[i].ORDERNO);
                        inPrint = true;
                    }
                    else
                    {
                        inUseedWindowCode = orm.InPatientGetCurrentWindow(Ptno, checkedOrders[i].ORDERNO);
                        if (inUseedWindowCode!="")
                        {
                            windowsSet.Add($@"{ orm.GetWindowName(inUseedWindowCode)}");
                        }
                        
                    }
                }


                if (inPrint)
                {
                    var list = inOrderSet.ToList();
                    for (int i = 0; i < orders.Length; i++)
                    {
                        orders[i].CHUANGKOU = inUseedWindowCode.IsEmpty() ? currentWin.Code : inUseedWindowCode;
                        if (orm.ExixtOrder(list[i]))
                            orm.InPatientSignIn(orders[i], checkInfo);
                    }

                }

                db.CommitTrans();

                if (!print&& !inPrint)
                    throw new Exception($@"请到{windowsSet.Join(",")}取药！");
                else
                    return new SigninStrip
                    {
                        PTNO = Ptno,
                        PTNAME = sname,
                        CHECK_DATE = Environ.SlowTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        WINDOW_NAME = orm.GetWindowName (currentWin.Code)
                    };
                
            }
            
        }
    }
}
