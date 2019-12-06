using App.FunctionLibrary;
using Framework.Entity;
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
    public class PharmacyReporterService : ServiceBase
    {
        /// <summary>
        /// 获取已分配的窗口
        /// </summary>
        private string GetInPatientWindowNo(PharmacyReporterManager orm, string Ptno)
        {
            var windowsSet = new HashSet<string>();
            var checkInfo = orm.GetInPatientCheckedInfo(Ptno);
            var orders = orm.GetInpatientCheckedOrder(checkInfo);

            for (int i = 0; i < orders.Length; i++)
            {
                if (orm.InPatientHadCheckedIn2(Ptno, orders[i].ORDERNO))
                {
                    var inUseedWindowCode = orm.InPatientGetCurrentWindow(Ptno, orders[i].ORDERNO);
                    windowsSet.Add($@"{orm.GetWindowName(inUseedWindowCode)}");
                }
            }

            if (windowsSet.Count == 0)
                return string.Empty;
            else
                throw new Exception($@"请到 {windowsSet.Join(",").Trim()} 取药！");
        }

        /// <summary>
        /// 获取窗口号
        /// </summary>
        private string GetOutPatientWindowNo(PharmacyReporterManager orm, string Ptno)
        {
            var outPreNos = orm.GetOutPresNO(Ptno);
            for (int i = 0; i < outPreNos.Length; i++)
            {
                if (!orm.IsOrderCharged(outPreNos[i].Presno))
                    throw new Exception("您的处方未缴费 ！");
            }
            
            var windowsSet = new HashSet<string>();
            var prenos = orm.GetPresNOwithWindow(Ptno);

            for (int i = 0; i < prenos.Length; i++)
            {
                //只有未签到的处方单才会更新状态
                if (orm.HadCheckedInWindow(Ptno, prenos[i].Presno))
                {
                    var useedWindows = orm.GetCurrentWindow(Ptno, prenos[i].Presno);
                    for (int j = 0; j < useedWindows.Length; j++)
                    {
                        windowsSet.Add($@"{ orm.GetWindowName(useedWindows[j].Code)}");
                    }
                }
            }

            if (windowsSet.Count == 0)
                return string.Empty;
            else
                throw new RepeatException($@"请到 {windowsSet.Join(",").Trim()} 取药！");
        }

        /// <summary>
        /// 记录发药分配窗口记录
        /// </summary>
        public int LogPharm(TWORD_JBINFO_180104 entity)
        {
            if (base.RoutingRequired)
                return base.CallProxy<int>(entity);
            else
                using (var db = new DataBaser(true))
                {
                    var orm = base.Create<PharmacyReporterManager>(db);
                    return orm.LogPharm(entity);
                }
        }

        /// <summary>
        /// 获取窗口排队状态
        /// </summary>
        /// <param name="dm">ture是否包含毒麻发药窗口，false不包含毒麻发药窗口</param>
        /// <param name="working">ture表示仅开放发药窗口，false表示所有发药窗口</param>
        public WindowJobs[] GetWorkingWindowsLineUpNums(bool dm, bool working)
        {
            if (base.RoutingRequired)
                return base.CallProxy<WindowJobs[]>(dm, working);
            else
                using (var db = new DataBaser(true))
                {
                    var orm = base.Create<PharmacyReporterManager>(db);
                    return orm.GetWorkingWindowsLineUpNums(dm, working);
                }
        }

        /// <summary>
        /// 窗口工作量
        /// </summary>
        public WindowJobs[] GetWindowJobs()
        {
            if (base.RoutingRequired)
                return base.CallProxy<WindowJobs[]>();
            else
                using (var db = new DataBaser(true))
                {
                    var orm = base.Create<PharmacyReporterManager>(db);
                    return orm.GetWindowJobs();
                }
        }

        static readonly Random random = new Random();

        /// <summary>
        /// 获取最少的排队窗口
        /// </summary>
        /// <param name="dm">ture是否包含毒麻发药窗口，false不包含毒麻发药窗口</param>
        public PharmacyWindow GetLeastLineUpNumsWindow(bool dm)
        {
            if (base.RoutingRequired)
                return base.CallProxy<PharmacyWindow>(dm);

            var windowJobs = GetWorkingWindowsLineUpNums(dm, true);
            if (windowJobs.Length == 0)
                return null;

            var min = windowJobs.Min(r => r.COUNT).COUNT;
            var same = windowJobs.FindAll(x => x.COUNT == min);
            var idx = same.Length == 1 ? 0 : random.Next(0, same.Length);

            return new PharmacyWindow { Code = same[idx].CODE };
        }

        /// <summary>
        /// 打印签到条
        /// </summary>
        public SigninStrip PrintReportStrip(string Ptno)
        {
            if (Ptno.IsEmpty())
                throw new Exception("患者编号为空 ...");

            if (base.RoutingRequired)
                return base.CallProxy<SigninStrip>(Ptno);

            using (var db = new DataBaser(true))
            {
                db.BeginTrans();

                var print = false;
                var inPrint = false;
                var useedWindowCode = string.Empty;
                var inUseedWindowCode = string.Empty;
                var windowsSet = new HashSet<string>();
                var prenoSet = new HashSet<string>();
                var inOrderSet = new HashSet<string>();
                var orm = base.Create<PharmacyReporterManager>(db);
                if (!orm.ExistPatient(Ptno))
                    throw new Exception("未找到患者信息 ...");

                //获取患者姓名
                var sname = orm.GetPatientName(Ptno);

                //活动窗口的工作量
                var workingWins = orm.GetWorkingWindow();
                if (workingWins.Length <= 0)
                    throw new Exception("发药窗口未开放 ！");

                //取药患者序号
                var index = orm.GetTuyakNo();
                //获取处方单
                var prenos = orm.GetPresNO(Ptno);
                var checkInfo = orm.GetInPatientCheckInfo(Ptno);
                var orders = orm.GetInpatientOrder(checkInfo);
                if (orders.Length <= 0
                    && prenos.Length <= 0
                    && GetInPatientWindowNo(orm, Ptno) == string.Empty
                    && GetOutPatientWindowNo(orm, Ptno) == string.Empty)
                    throw new Exception("未发现待发药处方 !");

                var dm = false;

                //更新医嘱状态,取药状态
                for (int i = 0; i < prenos.Length; i++)
                {
                    if (orm.HasDMOrder(prenos[i].Presno))
                    {
                        dm = true;
                    }

                    //只有未签到的处方单才会更新状态
                    if (!orm.HadCheckedIn(Ptno, prenos[i].Presno))
                    {
                        prenoSet.Add(prenos[i].Presno);
                        print = true;
                    }
                    // else
                    //{
                    //    orm.GetCurrentWindow(Ptno, prenos[i].Presno);
                    //    useedWindowCode.ad(orm.GetCurrentWindow(Ptno, prenos[i].Presno);
                    //    windowsSet.Add($@"{ orm.GetWindowName(useedWindowCode)}");
                    //}
                }

                //确定取药窗口,使用取余编制序号算法按人平均分配队伍
                var currentWin = GetLeastLineUpNumsWindow(dm);// workingWins[index % workingWins.Length];

                //同一患者同一窗口取药机制
                if (print)
                {
                    var list = prenoSet.ToList();
                    for (int i = 0; i < list.Count; i++)
                    {
                        orm.OrderMedicationAppointment(Ptno, list[i]);
                        orm.PrslipMedicationAppointment(Ptno, list[i], index.ToString()
                            , useedWindowCode.IsEmpty() ? currentWin.Code : useedWindowCode);
                    }
                }

                //日间手术操作
                for (int i = 0; i < orders.Length; i++)
                {
                    if (!orm.InPatientHadCheckedIn(Ptno, orders[i].ORDERNO))
                    {
                        inOrderSet.Add(orders[i].ORDERNO);
                        inPrint = true;
                    }
                }

                //var ok = workingWins.Count(r => r.Code.Equals("14") || r.Code.Equals("15")) > 0;
                //var isWeekend = DateTime.Now.DayOfWeek.Equals(DayOfWeek.Sunday) || DateTime.Now.DayOfWeek.Equals(DayOfWeek.Saturday);

                //日间开放10 11号窗口
                //var ok = (inUseedWindowCode.IsEmpty() ? currentWin.Code : inUseedWindowCode).Equals("14")
                //    || (inUseedWindowCode.IsEmpty() ? currentWin.Code : inUseedWindowCode).Equals("15");
                //if (!ok && orders.Length > 0)
                //    throw new Exception("日间手术室指定窗口10、11未开放");

                if (inPrint)
                {
                    //string[] InWincodes = { "14", "15" };
                    var temp = workingWins.ToList();//.FindAll(r => InWincodes.Contains(r.Code));
                    currentWin.Code = temp[index % temp.Count].Code;

                    //需要签到更新签到表和医嘱表
                    orm.InsertIprslip(checkInfo);
                    orm.UpdateIOrderZG(checkInfo);

                    //签到
                    var list = inOrderSet.ToList();
                    for (int i = 0; i < orders.Length; i++)
                    {
                        orders[i].CHUANGKOU = inUseedWindowCode.IsEmpty() ? currentWin.Code : inUseedWindowCode;
                        if (!orm.ExixtOrder(list[i]))
                            orm.InPatientSignIn(orders[i], checkInfo);
                    }
                }

                db.CommitTrans();

                #region 窗口分配记录
                orm.LogPharm(new TWORD_JBINFO_180104
                {
                    JBUSE = Ptno,
                    PUMCODE = currentWin.Code,
                    PLACECODE = Environ.SlowTime.ToString("yyyy-MM-dd"),
                    MINQTY = "药房报道机日志"
                });
                #endregion

                if (!print && !inPrint)
                    throw new Exception($@"请到 {windowsSet.Join(",").Trim()} 取药！");
                else
                    return new SigninStrip
                    {
                        PTNO = Ptno,
                        PTNAME = sname,
                        CHECK_DATE = Environ.SlowTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        WINDOW_NAME = orm.GetWindowName(currentWin.Code)
                    };
            }
        }
    }
}
