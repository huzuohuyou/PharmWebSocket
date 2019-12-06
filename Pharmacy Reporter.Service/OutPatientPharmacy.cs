using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharmacy_Reporter.Entity;
using Pharmacy_Reporter.ORM;

namespace Pharmacy_Reporter.Service
{

    public class OutPatientPharmacy : ICanSignIn
    {
        public SigninStrip SignIn(PharmacyReporterManager orm)
        {
            //获取处方单
            var prenos = orm.GetPresNO(Ptno);
            if (prenos.Length <= 0)
                throw new Exception("今日无处方、预约状态医嘱!");
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
                        , useedWindowCode.IsEmpty() ? currentWin.Code : useedWindowCode);
                }
            }
            db.CommitTrans();

            if (!print)
                throw new Exception($@"请到{windowsSet.Join(",")}取药！");
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
