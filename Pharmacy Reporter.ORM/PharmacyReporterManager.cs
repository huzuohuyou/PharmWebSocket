using App.FunctionLibrary;
using Framework.ORM;
using Pharmacy_Reporter.Entity;
using System;

namespace Pharmacy_Reporter.ORM
{
    /// <summary>
    /// 门诊报道机数据库交互
    /// </summary>
    public class PharmacyReporterManager : ORMbase
    {
        /// <summary>
        /// 获取发药编号
        /// </summary>
        /// <returns></returns>
        public int GetTuyakNo()
        {
            var sql = $@"
SELECT      TW_HSP_PMPA.SEQ_TUYAK.NEXTVAL TUYAK 
FROM        DUAL";

            var wb = new WhereBuilder();
            var No = DBaser.QueryValue<int>(sql, wb);
            if (No < 1)
                throw new Exception("错误编号");

            return No;
        }

        /// <summary>
        /// 是否存在毒麻药品
        /// </summary>
        public bool HasDMOrder(string PRESNO)
        {
            var sql = $@"
SELECT      COUNT(*) C FROM (
SELECT      (CASE WHEN P.BUN IN ('E0','E1') AND P.GBHIGH IS NULL THEN 'K'  
            WHEN P.BUN IN ('E2') AND P.GBHIGH IS NULL THEN 'Z'  
            WHEN P.BUN IN ('F0','F1') THEN 'C' 
            WHEN P.GBHIGH ='N' AND SLIPNO NOT IN ('1132','2132') THEN 'D' 
            WHEN P.GBHIGH ='N' AND SLIPNO IN ('1132','2132') THEN 'D2' ELSE  '0' END) DM FROM TWOCS_PRSLIP P
            WHERE PRESNO='{PRESNO}' )K WHERE K.DM ='D' ";
            var wb = new WhereBuilder();
            var r = DBaser.QueryValue<int>(sql, wb);
            return r > 0;
        }

        /// <summary>
        /// true 收费 false未收费
        /// </summary>
        /// <param name="presNo">门诊处方号</param>
        public bool IsOrderCharged(string presNo)
        {
            var sql = $@"
SELECT      SUNAP
FROM        TWOCS_OORDER";
            var wb = new WhereBuilder();
            wb.And("PRESNO", presNo);
            var charged = DBaser.QueryValue<int>(sql, wb);
            return charged == 1;
        }

        /// <summary>
        /// 是否存在患者信息
        /// </summary>
        public bool ExistPatient(string ptno)
        {
            var sql = $@"
SELECT      COUNT(1) COUNT
FROM        TWBAS_PATIENT";
            var wb = new WhereBuilder();
            wb.And("ptno", ptno);
            var COUNT = DBaser.QueryValue<int>(sql, wb);
            return COUNT > 0;
        }
        /// <summary>
        /// 获取用户名
        /// </summary>
        public string GetPatientName(string ptno)
        {
            var sql = $@"
SELECT      SNAME 
FROM        TWBAS_PATIENT";

            var wb = new WhereBuilder();
            wb.And("ptno", ptno);

            return DBaser.QueryValue<string>(sql, wb);
        }

        /// <summary>
        /// 更新医嘱表状态为取药预约
        /// </summary>
        /// <param name="ptno">患者编号</param>
        /// <param name="presno">处方号</param>
        public int OrderMedicationAppointment(string ptno, string presno)
        {
            if (ptno.IsEmpty())
                throw new Exception("患者编号为空");
            if (presno.IsEmpty())
                throw new Exception(".");
            var wb = new WhereBuilder();
            wb.And("PTNO", ptno, true);
            wb.And("PRESNO", presno, true);
            wb.And($@"BDATE=  TRUNC(SYSDATE)", AnalyzableKind.KeepOriginal);
            //wb.And("JUPSU", "0", true);
            DBaser.Update("TWOCS_OORDER");
            DBaser["JUPSU"] = "1";
            var count = DBaser.AcceptChanges(wb);
            //if (count <= 0)
            //    throw new Exception($"更新医嘱状态失败", new Exception($"患者编号:{ptno} 处方号:{presno}"));
            return count;
        }

        /// <summary>
        /// 已签到未取药 true
        /// </summary>
        public bool HadCheckedIn(string ptno, string presno)
        {
            var sql = $@"
SELECT      COUNT(*) COUNT 
FROM        TWOCS_PRSLIP
where BDATE > TRUNC(SYSDATE-3)
and PTNO= '{ptno}'
and PRESNO='{presno}'
and CHECKDATE IS NOT NULL
and CHECKTIME IS NOT NULL";

            var wb = new WhereBuilder();
            //wb.And("PTNO", ptno, true);
            //wb.And("PRESNO", presno, true);
            //wb.And("FLAG =1", AnalyzableKind.KeepOriginal);
            //wb.And("GBOUT IS NULL", AnalyzableKind.KeepOriginal);
            //wb.And("STATUS =0", AnalyzableKind.KeepOriginal);
            //wb.And("BDATE > TRUNC(SYSDATE-3)", AnalyzableKind.KeepOriginal);
            //wb.And("CHECKDATE IS NOT NULL", AnalyzableKind.KeepOriginal);
            //wb.And("CHECKTIME IS NOT NULL", AnalyzableKind.KeepOriginal);
            var count = DBaser.QueryValue<string>(sql, wb);

            return int.TryParse(count, out int result) && result > 0;
        }

        /// <summary>
        /// 已签到未取药 true
        /// </summary>
        public bool HadCheckedInWindow(string ptno, string presno)
        {
            var sql = $@"
SELECT      COUNT(*) COUNT 
FROM        TWOCS_PRSLIP";
            var wb = new WhereBuilder();
            wb.And("PTNO", ptno, true);
            wb.And("PRESNO", presno, true);
            wb.And("BDATE=  TRUNC(SYSDATE)", AnalyzableKind.KeepOriginal);
            wb.And("CHECKDATE IS NOT NULL", AnalyzableKind.KeepOriginal);
            wb.And("CHECKTIME IS NOT NULL", AnalyzableKind.KeepOriginal);

            wb.And("(GBOUT IS NULL OR GBOUT =0) ", AnalyzableKind.KeepOriginal);
            var count = DBaser.QueryValue<string>(sql, wb);

            return int.TryParse(count, out int result) && result > 0;
        }

        /// <summary>
        /// 日间手术 已签到未取药 true
        /// </summary>
        public bool InPatientHadCheckedIn(string ptno, string orderNo)
        {
            var sql = $@"
SELECT      COUNT(*) COUNT 
FROM        TWOCS_PRSLIP_CHECK";
            var wb = new WhereBuilder();
            wb.And("PTNO", ptno, true);
            wb.And("ORDERNO", orderNo, true);
            wb.And("FLAG =1", AnalyzableKind.KeepOriginal);
            //wb.And("GBOUT IS NULL", AnalyzableKind.KeepOriginal);
            wb.And("STATUS =0", AnalyzableKind.KeepOriginal);
            wb.And($@"BDATE <= TO_DATE('{Environ.SlowTime.ToString("yyyy - MM - dd")}','YYYY-MM-DD') 
    AND BDATE >= TO_DATE('{Environ.SlowTime.AddDays(-2).ToString("yyyy-MM-dd")}', 'YYYY-MM-DD') ", AnalyzableKind.KeepOriginal);
            wb.And("CHECKDATE IS NOT NULL", AnalyzableKind.KeepOriginal);
            wb.And("CHECKTIME IS NOT NULL", AnalyzableKind.KeepOriginal);
            var count = DBaser.QueryValue<string>(sql, wb);
            int.TryParse(count, out int result);
            return result > 0;
        }

        /// <summary>
        /// 日间手术室签到
        /// </summary>
        /// <param name="ptno">患者编号</param>
        /// <param name="orderNo">医嘱编号</param>
        public bool InPatientHadCheckedIn2(string ptno, string orderNo)
        {
            var sql = $@"
SELECT      COUNT(*) COUNT 
FROM        TWOCS_PRSLIP_CHECK";
            var wb = new WhereBuilder();
            wb.And("PTNO", ptno, true);
            wb.And("ORDERNO", orderNo, true);
            wb.And("FLAG =1", AnalyzableKind.KeepOriginal);
            //wb.And("GBOUT IS NULL", AnalyzableKind.KeepOriginal);
            //wb.And("STATUS =0", AnalyzableKind.KeepOriginal);
            wb.And($@"BDATE <= TO_DATE('{Environ.SlowTime.ToString("yyyy-MM-dd")}','YYYY-MM-DD') 
    AND BDATE >= TO_DATE('{Environ.SlowTime.AddDays(-2).ToString("yyyy-MM-dd")}', 'YYYY-MM-DD') ", AnalyzableKind.KeepOriginal);
            wb.And("CHECKDATE IS NOT NULL", AnalyzableKind.KeepOriginal);
            wb.And("CHECKTIME IS NOT NULL", AnalyzableKind.KeepOriginal);
            wb.And("OUTDATE IS  NULL", AnalyzableKind.KeepOriginal);
            wb.And("OUTTIME IS  NULL", AnalyzableKind.KeepOriginal);
            var count = DBaser.QueryValue<string>(sql, wb);

            return int.TryParse(count, out int result) && result > 0;
        }

        /// <summary>
        /// 已签到未发药 已分配窗口
        /// </summary>
        public PharmacyWindow[] GetCurrentWindow(string ptno, string presno)
        {
            var sql = $@"
SELECT      DISTINCT(CHUANGKOU) CODE
FROM        TWOCS_PRSLIP
{{0}}
GROUP BY    CHUANGKOU";
            var wb = new WhereBuilder();
            wb.And("PTNO", ptno, true);
            wb.And("PRESNO", presno, true);
            wb.And("BDATE=  TRUNC(SYSDATE)", AnalyzableKind.KeepOriginal);
            wb.And("CHECKDATE IS NOT NULL", AnalyzableKind.KeepOriginal);
            wb.And("CHECKTIME IS NOT NULL", AnalyzableKind.KeepOriginal);
            wb.And("(GBOUT IS NULL OR GBOUT =0) ", AnalyzableKind.KeepOriginal);

            return DBaser.Query<PharmacyWindow>(sql, wb);
        }

        /// <summary>
        /// 日间手术室已签到未发药 已分配窗口
        /// </summary>
        public string InPatientGetCurrentWindow(string ptno, string orderNo)
        {
            var sql = $@"
SELECT      DISTINCT(CHUANGKOU) CODE
FROM        TWOCS_PRSLIP_CHECK
{{0}}
GROUP BY    CHUANGKOU";

            var wb = new WhereBuilder();
            wb.And("PTNO", ptno, true);
            wb.And("ORDERNO", orderNo, true);
            wb.And("FLAG =1", AnalyzableKind.KeepOriginal);
            //wb.And("GBOUT IS NULL", AnalyzableKind.KeepOriginal);
            //wb.And("STATUS =0", AnalyzableKind.KeepOriginal);
            wb.And($@"BDATE <= TO_DATE('{Environ.SlowTime.ToString("yyyy-MM-dd")}','YYYY-MM-DD') 
    AND BDATE >= TO_DATE('{Environ.SlowTime.AddDays(-2).ToString("yyyy-MM-dd")}', 'YYYY-MM-DD') ", AnalyzableKind.KeepOriginal);
            wb.And("CHECKDATE IS not NULL", AnalyzableKind.KeepOriginal);
            wb.And("CHECKTIME IS not NULL", AnalyzableKind.KeepOriginal);
            wb.And("OUTDATE IS  NULL", AnalyzableKind.KeepOriginal);
            wb.And("OUTTIME IS  NULL", AnalyzableKind.KeepOriginal);
            var windows = DBaser.Query<PharmacyWindow>(sql, wb);
            if (windows.Length > 1)
                return string.Empty;// throw new Exception("一个处方单被分配到多个窗口！");
            if (windows.Length == 0)
                return string.Empty;//throw new Exception("未找到窗口记录！");

            return windows.First()?.Code;
        }

        /// <summary>
        /// 更新发药记录表预约状态
        /// </summary>
        /// <param name="ptno">患者编号</param>
        /// <param name="presno">处方号</param>
        /// <param name="tuyakNo">发药序号</param>
        /// <param name="window">窗口号</param>
        public int PrslipMedicationAppointment(string ptno, string presno, string tuyakNo, string window)
        {
            if (ptno.IsEmpty())
                throw new Exception("患者编号为空");
            if (presno.IsEmpty())
                throw new Exception("处方号为空");
            if (tuyakNo.IsEmpty())
                throw new Exception("发药序号为空");
            if (window.IsEmpty())
                throw new Exception("窗口为空");

            var wb = new WhereBuilder();
            //wb.And("PTNO", ptno, true);
            //wb.And("PRESNO", presno, true);
            //wb.And("STATUS", "0", true);
            //wb.And("CHECKDATE IS NULL",AnalyzableKind.KeepOriginal);
            //wb.And("CHECKTIME IS NULL", AnalyzableKind.KeepOriginal);

            //DBaser.Update("TWOCS_PRSLIP");
            //DBaser["CHECKDATE"] = Environ.SlowTime.Date;
            //DBaser["CHECKTIME"] = Environ.SlowTime;
            //DBaser["FLAG"] = "1";
            //DBaser["TuyakNo"] = tuyakNo;
            //DBaser["CHUANGKOU"] = window;
            //var count = DBaser.AcceptChanges(wb);
            var sql = $@"
Update      TWOCS_PRSLIP 
Set         TUYAKNO = '{tuyakNo}',
	        CHUANGKOU = '{window}',
	        CHECKDATE = to_date('{Environ.SlowTime.ToString("yyyy-MM-dd")}','yyyy-mm-dd hh24:mi:ss'),
	        FLAG = 1,
	        CHECKTIME = to_date('{Environ.SlowTime.ToString("yyyy-MM-dd HH:mm:ss")}','yyyy-mm-dd hh24:mi:ss')
Where       PTNO = '{ptno}' 
And         PRESNO = '{presno}' 
--And         STATUS = '0' 
--And         CHECKDATE IS NULL 
--And         CHECKTIME IS NULL";

            var count = DBaser.Execute(sql, wb);
            //if (count <= 0)
            //    throw new Exception($"更新医嘱状态失败", new Exception($"患者编号:{ptno} 处方号:{presno}"));
            return count;
        }

        /// <summary>
        /// 获取患者的  处方、预约 处方单状态单
        /// </summary>
        public Prescription[] GetPresNO(string ptno)
        {
            var sql = $@"
SELECT PRESNO,PTNO, TUYAKNO, DEPTCODE, DRCODE,DRNAME, USERID, GBPRINT, SNAME, 
          SEX,  BIRTHDATE, CODE, CODENAME, DEPTNAMEK,STATUS, 
          JUSO, JUMIN ,BDATE,CHUANGKOU,ORDERGUBUN,A  FROM ( 
 SELECT P.PRESNO,P.PTNO, p.TUYAKNO, p.DEPTCODE, p.DRCODE, p.USERID, p.GBPRINT, A.SNAME, 
        TW_HSP_PMPA.TWBAS_COMMON.S_CodeName('药房','窗口',P.CHUANGKOU) CHUANGKOU, 
        A.SEX, A.BIRTHDATE, P.BI CODE, P.STATUS, 
        TW_HSP_PMPA.TWBAS_GETNAME.DEPTNAME(P.DEPTCODE) DEPTNAMEK,
        TW_HSP_PMPA.TWBAS_GETNAME.DRNAME(P.DRCODE) DRNAME,
        TW_HSP_PMPA.TWBAS_COMMON.S_CodeName('BASIC','BI',P.BI) CODENAME ,
        A.JUSO, A.JUMIN, P.BDATE,ORDERGUBUN, 
        (CASE WHEN P.BUN IN ('E0','E1') AND P.GBHIGH IS NULL THEN 'K'  
        WHEN P.BUN IN ('E2') AND P.GBHIGH IS NULL THEN 'Z'  
        WHEN P.BUN IN ('F0','F1') THEN 'C' 
        WHEN P.GBHIGH ='N' AND SLIPNO NOT IN ('1132','2132') THEN 'D' 
        WHEN P.GBHIGH ='N' AND SLIPNO IN ('1132','2132') THEN 'D2' ELSE  '0' END) A 
   FROM TW_HSP_OCS.TWOCS_PRSLIP    P, 
        TW_HSP_PMPA.TWBAS_PATIENT  A 
  WHERE P.PTNO     = A.PTNO 
    AND  P.PTNO = '{ptno}' 
    AND P.CHUANGKOU= '00'     
    AND (P.GBOUT    = '0' OR P.GBOUT IS NULL)                                                   
    AND P.ENTDATE    >= TO_DATE('{Environ.SlowTime.AddDays(-2).ToString("yyyy-MM-dd")}','YYYY-MM-DD')                              
    AND P.ENTDATE    <= TO_DATE('{Environ.SlowTime.ToString("yyyy-MM-dd")}','YYYY-MM-DD')                         
    AND P.Gbio     = 'O'                                                                        
    AND P.STATUS    = '0'                                                                       
    AND ((P.DEPTCODE ='ER' AND P.BUN IN ('F0','F1')) OR P.DEPTCODE <> 'ER')                     
  ) 
 GROUP BY PRESNO,PTNO, TUYAKNO, DEPTCODE,DRNAME, DRCODE, USERID, GBPRINT, SNAME, 
          SEX,  BIRTHDATE, CODE, CODENAME, DEPTNAMEK,STATUS, 
          JUSO, JUMIN ,BDATE,CHUANGKOU,ORDERGUBUN,A 
 ORDER BY GBPRINT,TUYAKNO DESC  ";
            var wb = new WhereBuilder();
            var PresNOs = DBaser.Query<Prescription>(sql, wb);

            return PresNOs;
        }

        /// <summary>
        /// 获取门诊今日处方
        /// </summary>
        public Prescription[] GetOutPresNO(string ptno)
        {
            var sql = $@"
SELECT      PRESNO 
FROM        TWOCS_OORDER 
WHERE       PTNO = '{ptno}' 
AND         BDATE    = TO_DATE('{Environ.SlowTime.ToString("yyyy-MM-dd")}','YYYY-MM-DD')                          
";
            var wb = new WhereBuilder();
            var PresNOs = DBaser.Query<Prescription>(sql, wb);
            return PresNOs;
        }

        /// <summary>
        /// 获取患者的  处方、预约 处方单状态单
        /// </summary>
        public Prescription[] GetPresNOwithWindow(string ptno)
        {
            var sql = $@"
SELECT PRESNO,PTNO, TUYAKNO, DEPTCODE, DRCODE,DRNAME, USERID, GBPRINT, SNAME, 
          SEX,  BIRTHDATE, CODE, CODENAME, DEPTNAMEK,STATUS, 
          JUSO, JUMIN ,BDATE,CHUANGKOU,ORDERGUBUN,A  FROM ( 
 SELECT P.PRESNO,P.PTNO, p.TUYAKNO, p.DEPTCODE, p.DRCODE, p.USERID, p.GBPRINT, A.SNAME, 
        TW_HSP_PMPA.TWBAS_COMMON.S_CodeName('药房','窗口',P.CHUANGKOU) CHUANGKOU, 
        A.SEX, A.BIRTHDATE, P.BI CODE, P.STATUS, 
        TW_HSP_PMPA.TWBAS_GETNAME.DEPTNAME(P.DEPTCODE) DEPTNAMEK,
        TW_HSP_PMPA.TWBAS_GETNAME.DRNAME(P.DRCODE) DRNAME,
        TW_HSP_PMPA.TWBAS_COMMON.S_CodeName('BASIC','BI',P.BI) CODENAME ,
        A.JUSO, A.JUMIN, P.BDATE,ORDERGUBUN, 
        (CASE WHEN P.BUN IN ('E0','E1') AND P.GBHIGH IS NULL THEN 'K'  
        WHEN P.BUN IN ('E2') AND P.GBHIGH IS NULL THEN 'Z'  
        WHEN P.BUN IN ('F0','F1') THEN 'C' 
        WHEN P.GBHIGH ='N' AND SLIPNO NOT IN ('1132','2132') THEN 'D' 
        WHEN P.GBHIGH ='N' AND SLIPNO IN ('1132','2132') THEN 'D2' ELSE  '0' END) A 
   FROM TW_HSP_OCS.TWOCS_PRSLIP    P, 
        TW_HSP_PMPA.TWBAS_PATIENT  A 
  WHERE P.PTNO     = A.PTNO 
    AND  P.PTNO = '{ptno}' 
    AND P.CHUANGKOU != '00'     
    AND (P.GBOUT    = '0' OR P.GBOUT IS NULL)                                                   
    AND P.ENTDATE    >= TO_DATE('{Environ.SlowTime.AddDays(-2).ToString("yyyy-MM-dd")}','YYYY-MM-DD')                              
    AND P.ENTDATE    <= TO_DATE('{Environ.SlowTime.ToString("yyyy-MM-dd")}','YYYY-MM-DD')                         
    AND P.Gbio     = 'O'                                                                        
    AND P.STATUS    = '0'                                                                       
    AND ((P.DEPTCODE ='ER' AND P.BUN IN ('F0','F1')) OR P.DEPTCODE <> 'ER')                     
  ) 
 GROUP BY PRESNO,PTNO, TUYAKNO, DEPTCODE,DRNAME, DRCODE, USERID, GBPRINT, SNAME, 
          SEX,  BIRTHDATE, CODE, CODENAME, DEPTNAMEK,STATUS, 
          JUSO, JUMIN ,BDATE,CHUANGKOU,ORDERGUBUN,A 
 ORDER BY GBPRINT,TUYAKNO DESC  ";
            var wb = new WhereBuilder();
            var PresNOs = DBaser.Query<Prescription>(sql, wb);

            return PresNOs;
        }

        /// <summary>
        /// 获取患者的  处方、预约 处方单状态单
        /// </summary>
        public Prescription[] GetInPresNO(string ptno)
        {
            var sql = $@"
SELECT      DISTINCT(PRESNO) PRESNO,BDATE,PTNO 
FROM        TWOCS_IORDER 
{{0}}
GROUP BY    PRESNO,BDATE,PTNO";
            var wb = new WhereBuilder();
            wb.And("PTNO", ptno, true);
            wb.And("JUPSU in ('0','1')", AnalyzableKind.KeepOriginal);
            wb.And("BDATE=  TRUNC(SYSDATE)", AnalyzableKind.KeepOriginal);
            var PresNOs = DBaser.Query<Prescription>(sql, wb);
            return PresNOs;
        }

        /// <summary>
        /// 获取全部窗口
        /// </summary>
        public PharmacyWindow[] GetAllWindow()
        {
            var sql = $@"
SELECT      * 
FROM        TW_HSP_PMPA.TWBAS_BASECODE 
{{0}}
Order       By Code ";
            var wb = new WhereBuilder();
            wb.And("BUSINESS", "药房");
            wb.And("BUN", "窗口");
            wb.And("GBUSE", "0");
            wb.And("NFLAG1", "1");
            var result = DBaser.Query<PharmacyWindow>(sql, wb);
            return result;
        }

        /// <summary>
        /// 获取工作窗口的取药数量
        /// </summary>
        public PharmacyWindow[] GetWorkingWindow()
        {
            var sql = $@"
SELECT      CHUANGKUO CODE      
FROM        TWOCS_PRCAOZUO  P, (
SELECT      CODE 
FROM        TWBAS_BASECODE    
WHERE       BUSINESS='药房'      
AND         BUN='窗口'      
AND         NFLAG1='1') B     
WHERE       JOBDATE = TRUNC(SYSDATE)   
AND         GBEND='0'   
AND         P.CHUANGKUO=B.CODE  
GROUP BY    CHUANGKUO";
            var wb = new WhereBuilder();
            var result = DBaser.Query<PharmacyWindow>(sql, wb);
            return result;
        }

        /// <summary>
        /// 获取窗口名称
        /// </summary>
        public string GetWindowName(string code)
        {
            if (code.IsEmpty())
                throw new Exception("窗口编号为空！");

            var sql = $@"
SELECT      CODENAME 
FROM        TWBAS_BASECODE    
";
            var wb = new WhereBuilder();
            wb.And("BUSINESS", "药房");
            wb.And("BUN", "窗口");
            wb.And("GBUSE", "0");
            wb.And("NFLAG1", "1");
            wb.And("CODE", code);
            var result = DBaser.QueryValue<string>(sql, wb);
            if (result.IsEmpty())
            {
                throw new Exception($"无法获取到窗口名称{code}");
            }
            return result;
        }

        /// <summary>
        /// 获取签到病人信息
        /// </summary>
        /// <param name="ptno"></param>
        /// <returns></returns>
        public InPatientCheck GetInPatientCheckInfo(string ptno)
        {
            if (ptno.IsEmpty())
                throw new Exception("患者编号为空！");
            var sql = $@"
SELECT /*+ rule */ 
  DECODE(O.ORDERGUBUN,'B','B','H','H','L') ORDERGUBUN, 
  O.Ptno , M.Sname, M.Sex, M.Age, O.WardCode, M.Roomcode, M.Bedno, 
  TW_HSP_PMPA.TWBAS_GETNAME.WARDNAME(O.WARDCODE) WARDNAME , 
  TW_HSP_PMPA.TWBAS_COMMON.S_CodeName('BASIC','BI',O.BI) BI1 ,O.BI,
  TW_HSP_PMPA.TWBAS_GETNAME.DEPTNAME(O.DEPTCODE) DEPTNAME,O.DEPTCODE, 
  TW_HSP_PMPA.TWBAS_GETNAME.DRNAME(O.DRCODE) DRNAME,O.DRCODE,
  TW_HSP_PMPA.TWBAS_COMMON.S_CodeName('IPD','在院状态',M.AMSET1) AMSET1 ,
  O.JUPSU, M.INDATE 
   FROM TW_HSP_OCS.TWOCS_IORDER_ZG O,
        TW_HSP_PMPA.VIEW_MASTER M ,
        TW_HSP_PMPA.TWBAS_NSUGA N
  WHERE
  o.ptno='{ptno}'
  and O.BDATE <= TO_DATE('{Environ.SlowTime.ToString("yyyy-MM-dd")}','YYYY-MM-DD') 
    AND O.BDATE >= TO_DATE('{Environ.SlowTime.AddDays(-2).ToString("yyyy-MM-dd")}','YYYY-MM-DD') 
    AND O.GBPICKUP = '1'
    AND SUBSTR(O.SENDDEPT1,1,2) = '70' 
    AND O.GBAUTO NOT IN ('T','Z') 
    AND O.STATUS = '0'
    AND O.JUPSU = '0' 
    AND O.ORDERGUBUN = 'H' 
        AND N.SUNEXT = O.SUCODE 
    AND O.SUNSUNAP ='3' 
    AND M.AMSET4 <> '2' 
    AND M.AMSET1 = '0' 
    AND (O.STOPCHKID IS NULL OR TRIM(O.STOPCHKID) = '' ) 
    AND O.STOPCHKDATE IS NULL 
    AND O.PTNO     = M.PTNO
    AND O.BI       = M.BI
    AND O.INDATE   = M.INDATE
    AND O.ADMSEQNO = M.ADMSEQNO
  GROUP BY
  DECODE(O.ORDERGUBUN,'B','B','H','H','L'), 
  O.Ptno , M.Sname, M.Sex, M.Age, O.Bi, O.WardCode, M.Roomcode, M.Bedno, 
  O.DEPTCODE, O.DRCODE, M.AMSET1, O.JUPSU, M.INDATE
";
            var wb = new WhereBuilder();
            var result = DBaser.Query<InPatientCheck>(sql, wb);
            if (result.Length != 1)
                return null;
            return result[0];
        }

        /// <summary>
        /// 获取 已经 签到病人信息
        /// </summary>
        public InPatientCheck GetInPatientCheckedInfo(string ptno)
        {
            if (ptno.IsEmpty())
                throw new Exception("患者编号为空！");
            var sql = $@"
SELECT /*+ rule */ 
  DECODE(O.ORDERGUBUN,'B','B','H','H','L') ORDERGUBUN, 
  O.Ptno , M.Sname, M.Sex, M.Age, O.WardCode, M.Roomcode, M.Bedno, 
  TW_HSP_PMPA.TWBAS_GETNAME.WARDNAME(O.WARDCODE) WARDNAME , 
  TW_HSP_PMPA.TWBAS_COMMON.S_CodeName('BASIC','BI',O.BI) BI1 ,O.BI,
  TW_HSP_PMPA.TWBAS_GETNAME.DEPTNAME(O.DEPTCODE) DEPTNAME,O.DEPTCODE, 
  TW_HSP_PMPA.TWBAS_GETNAME.DRNAME(O.DRCODE) DRNAME,O.DRCODE,
  TW_HSP_PMPA.TWBAS_COMMON.S_CodeName('IPD','在院状态',M.AMSET1) AMSET1 ,
  O.JUPSU, M.INDATE 
   FROM TW_HSP_OCS.TWOCS_IORDER_ZG O,
        TW_HSP_PMPA.VIEW_MASTER M ,
        TW_HSP_PMPA.TWBAS_NSUGA N
  WHERE
  o.ptno='{ptno}'
  and O.BDATE <= TO_DATE('{Environ.SlowTime.ToString("yyyy-MM-dd")}','YYYY-MM-DD') 
    AND O.BDATE >= TO_DATE('{Environ.SlowTime.AddDays(-2).ToString("yyyy-MM-dd")}','YYYY-MM-DD') 
    AND O.GBPICKUP = '1'
    AND SUBSTR(O.SENDDEPT1,1,2) = '70' 
    AND O.GBAUTO NOT IN ('T','Z') 
    AND O.STATUS = '0'
    AND O.JUPSU = '1' 
    AND O.ORDERGUBUN = 'H' 
        AND N.SUNEXT = O.SUCODE 
    AND O.SUNSUNAP ='3' 
    AND M.AMSET4 <> '2' 
    AND M.AMSET1 = '0' 
    AND (O.STOPCHKID IS NULL OR TRIM(O.STOPCHKID) = '' ) 
    AND O.STOPCHKDATE IS NULL 
    AND O.PTNO     = M.PTNO
    AND O.BI       = M.BI
    AND O.INDATE   = M.INDATE
    AND O.ADMSEQNO = M.ADMSEQNO
  GROUP BY
  DECODE(O.ORDERGUBUN,'B','B','H','H','L'), 
  O.Ptno , M.Sname, M.Sex, M.Age, O.Bi, O.WardCode, M.Roomcode, M.Bedno, 
  O.DEPTCODE, O.DRCODE, M.AMSET1, O.JUPSU, M.INDATE
";
            var wb = new WhereBuilder();
            var result = DBaser.Query<InPatientCheck>(sql, wb);
            if (result.Length != 1)
                return null;
            return result[0];
        }

        /// <summary>
        /// 住院发药记录登记
        /// </summary>
        public int InsertIprslip(InPatientCheck inPatient)
        {
            if (inPatient == null)
                return 0;

            var sql = $@" 
INSERT  INTO    TWOCS_IPRSLIP( 
PTNO, BI, INDATE, ADMSEQNO, WARDCODE, ROOMCODE, BEDNO, DEPTCODE, DRCODE,  
ORDERGUBUN, ORDERCODE, BOHUMCODE, BDATE, SUCODE, BUN, SLIPNO, PLUSCODE, PLUSNAME,  
CONTENTS, BCONTENTS, REALQTY, DIV, NAL, QTY, JSQTY,  
GRP, POWDER, TFLAG, SELF, ER, REMARK, PRN, STATUS, BASEUNIT, LABELUNIT,  
 BASEAMT, AMT1, AMT2,  
ORDERNO, PKORDERNO, SEQNO,  
INSERTID, INSERTBUSE, INSERTDATE, INSERTTIME,   
PUMCODE,CHUANGKOU 
 ) 
SELECT  I.PTNO, I.BI, I.INDATE, I.ADMSEQNO, I.WARDCODE, M.ROOMCODE, M.BEDNO, I.DEPTCODE, I.DRCODE,  
I.ORDERGUBUN, I.ORDERCODE, C.BOHUMCODE, I.BDATE, I.SUCODE, I.BUN, I.SLIPNO, I.PLUSCODE, I.PLUSNAME,  
I.CONTENTS, I.BCONTENTS, I.REALQTY, I.DIV, I.NAL, I.QTY, I.JSQTY,  
I.GRP, I.POWDER, I.TFLAG, I.SELF, I.ER, I.REMARK, I.PRN, I.STATUS, C.BASEUNIT, C.LABELUNIT,  
T.BOAMT BASEAMT,T.BOAMT * I.JSQTY AMT1, '' AMT2,
I.ORDERNO, I.PKORDERNO, ROWNUM,  
'001757', '20010107', TRUNC(SYSDATE), TO_CHAR(SYSDATE,'HH24:MI:SS'),   
T.PUMCODE ,'MZ'
FROM    TW_HSP_OCS.TWOCS_IORDER_ZG I,
        TW_HSP_OCS.TWOCS_ORDERCODE C,
        TW_HSP_PMPA.TWBAS_TSUGA T, 
        TW_HSP_PMPA.TWBAS_NSUGA N,
        TW_HSP_PMPA.VIEW_MASTER M 
WHERE   I.PTNO        = '{inPatient.PTNO}'
AND     I.BDATE       <= TO_DATE('{Environ.SlowTime.ToString("yyyy-MM-dd")}','YYYY-MM-DD')
AND     I.BDATE       >= TO_DATE('{Environ.SlowTime.AddDays(-2).ToString("yyyy-MM-dd")}','YYYY-MM-DD')
AND     I.GBPICKUP = '1'
AND     SUBSTR(I.SENDDEPT1,1,2) = '70' 
AND     I.GBAUTO NOT IN ('T','Z') 
AND     I.STATUS = '0'
AND     I.ORDERGUBUN = 'H'
--AND     I.WARDCODE = '{inPatient.WARDCODE}' 
--AND     I.DEPTCODE = '{inPatient.DEPTCODE}' 
--AND     I.DRCODE   = '{inPatient.DRCODE}' 
AND     I.JUPSU = '0' 
AND     N.SUNEXT = I.SUCODE 
AND     (I.STOPCHKID IS NULL OR TRIM(I.STOPCHKID) = '')
AND     I.STOPCHKDATE IS NULL 
AND     I.SUNSUNAP ='3' 
AND     I.ORDERCODE = C.ORDERCODE 
AND     I.SUCODE = T.SUCODE 
AND     I.PTNO = M.PTNO ";

            var count = DBaser.Execute(sql, new WhereBuilder());
            return count;
        }

        /// <summary>
        /// 获取日间手术病人医嘱
        /// </summary>
        public TWOCS_PRSLIP_CHECK[] GetInpatientOrder(InPatientCheck inPatient)
        {
            if (inPatient == null)
                return new TWOCS_PRSLIP_CHECK[] { };

            var sql = $@"
SELECT  I.SENDDEPT1,I.PTNO, I.BI, I.INDATE, I.ADMSEQNO, I.WARDCODE, M.ROOMCODE, M.BEDNO, I.DEPTCODE, I.DRCODE,  
I.ORDERGUBUN, I.ORDERCODE, C.BOHUMCODE, I.BDATE, I.SUCODE, I.BUN, I.SLIPNO, I.PLUSCODE DOSCODE, I.PLUSNAME,  
I.CONTENTS, I.BCONTENTS, I.REALQTY, I.DIV, I.NAL, I.QTY, I.JSQTY,  
I.GRP, I.POWDER, I.TFLAG, I.SELF, I.ER, I.REMARK, I.PRN, I.STATUS, C.BASEUNIT, C.LABELUNIT,  
T.BOAMT BASEAMT,T.BOAMT * I.JSQTY AMT1, '' AMT2,
I.ORDERNO, I.PKORDERNO, ROWNUM,  
'001757', '20010107', TRUNC(SYSDATE), TO_CHAR(SYSDATE,'HH24:MI:SS'),   
T.PUMCODE ,'MZ'
FROM    TW_HSP_OCS.TWOCS_IORDER_ZG I,
        TW_HSP_OCS.TWOCS_ORDERCODE C,
        TW_HSP_PMPA.TWBAS_TSUGA T, 
        TW_HSP_PMPA.TWBAS_NSUGA N,
        TW_HSP_PMPA.VIEW_MASTER M 
WHERE   I.PTNO        = '{inPatient.PTNO}'
AND     I.BDATE       <= TO_DATE('{Environ.SlowTime.ToString("yyyy-MM-dd")}','YYYY-MM-DD')
AND     I.BDATE       >= TO_DATE('{Environ.SlowTime.AddDays(-2).ToString("yyyy-MM-dd")}','YYYY-MM-DD')
AND     I.GBPICKUP = '1'
AND     SUBSTR(I.SENDDEPT1,1,1) = '7' 
AND     I.GBAUTO NOT IN ('T','Z') 
AND     I.STATUS = '0'
AND     I.ORDERGUBUN = 'H'
AND     I.WARDCODE = '{inPatient.WARDCODE}' 
AND     I.DEPTCODE = '{inPatient.DEPTCODE}' 
AND     I.DRCODE   = '{inPatient.DRCODE}' 
AND     I.JUPSU = '0' 
AND     N.SUNEXT = I.SUCODE 
AND     (I.STOPCHKID IS NULL OR TRIM(I.STOPCHKID) = '')
AND     I.STOPCHKDATE IS NULL 
AND     I.SUNSUNAP ='3' 
AND     I.ORDERCODE = C.ORDERCODE 
AND     I.SUCODE = T.SUCODE 
AND     I.PTNO = M.PTNO
";
            var wb = new WhereBuilder();
            var result = DBaser.Query<TWOCS_PRSLIP_CHECK>(sql, wb);
            return result;
        }

        /// <summary>
        /// 获取日间手术病人医嘱
        /// </summary>
        public TWOCS_PRSLIP_CHECK[] GetInpatientCheckedOrder(InPatientCheck inPatient)
        {
            if (inPatient == null)
                return new TWOCS_PRSLIP_CHECK[] { };
            var sql = $@"
SELECT  I.SENDDEPT1,I.PTNO, I.BI, I.INDATE, I.ADMSEQNO, I.WARDCODE, M.ROOMCODE, M.BEDNO, I.DEPTCODE, I.DRCODE,  
I.ORDERGUBUN, I.ORDERCODE, C.BOHUMCODE, I.BDATE, I.SUCODE, I.BUN, I.SLIPNO, I.PLUSCODE DOSCODE, I.PLUSNAME,  
I.CONTENTS, I.BCONTENTS, I.REALQTY, I.DIV, I.NAL, I.QTY, I.JSQTY,  
I.GRP, I.POWDER, I.TFLAG, I.SELF, I.ER, I.REMARK, I.PRN, I.STATUS, C.BASEUNIT, C.LABELUNIT,  
T.BOAMT BASEAMT,T.BOAMT * I.JSQTY AMT1, '' AMT2,
I.ORDERNO, I.PKORDERNO, ROWNUM,  
'001757', '20010107', TRUNC(SYSDATE), TO_CHAR(SYSDATE,'HH24:MI:SS'),   
T.PUMCODE ,'MZ'
FROM    TW_HSP_OCS.TWOCS_IORDER_ZG I,
        TW_HSP_OCS.TWOCS_ORDERCODE C,
        TW_HSP_PMPA.TWBAS_TSUGA T, 
        TW_HSP_PMPA.TWBAS_NSUGA N,
        TW_HSP_PMPA.VIEW_MASTER M 
WHERE   I.PTNO        = '{inPatient.PTNO}'
AND     I.BDATE       <= TO_DATE('{Environ.SlowTime.ToString("yyyy-MM-dd")}','YYYY-MM-DD')
AND     I.BDATE       >= TO_DATE('{Environ.SlowTime.AddDays(-2).ToString("yyyy-MM-dd")}','YYYY-MM-DD')
AND     I.GBPICKUP = '1'
AND     SUBSTR(I.SENDDEPT1,1,1) = '7' 
AND     I.GBAUTO NOT IN ('T','Z') 
AND     I.STATUS = '0'
AND     I.ORDERGUBUN = 'H'
AND     I.WARDCODE = '{inPatient.WARDCODE}' 
AND     I.DEPTCODE = '{inPatient.DEPTCODE}' 
AND     I.DRCODE   = '{inPatient.DRCODE}' 
AND     I.JUPSU = '1' 
AND     N.SUNEXT = I.SUCODE 
AND     (I.STOPCHKID IS NULL OR TRIM(I.STOPCHKID) = '')
AND     I.STOPCHKDATE IS NULL 
AND     I.SUNSUNAP ='3' 
AND     I.ORDERCODE = C.ORDERCODE 
AND     I.SUCODE = T.SUCODE 
AND     I.PTNO = M.PTNO
";
            var wb = new WhereBuilder();
            var result = DBaser.Query<TWOCS_PRSLIP_CHECK>(sql, wb);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public int UpdateIOrderZG(InPatientCheck inPatient)
        {
            if (inPatient == null)
                return 0;

            var sql = $@"
UPDATE  TW_HSP_OCS.TWOCS_IORDER_ZG I 
SET     I.JUPSU    = '1' 
WHERE   I.ROWID IN (
SELECT  I.ROWID 
FROM    TW_HSP_OCS.TWOCS_IORDER_ZG I, 
        TW_HSP_PMPA.TWBAS_NSUGA N
WHERE   I.PTNO        = '{inPatient.PTNO}'
AND     I.BDATE       <= TO_DATE('{Environ.SlowTime.ToString("yyyy-MM-dd")}','YYYY-MM-DD')
AND     I.BDATE       >= TO_DATE('{Environ.SlowTime.AddDays(-2).ToString("yyyy-MM-dd")}','YYYY-MM-DD')
AND     I.GBPICKUP = '1'
AND     SUBSTR(I.SENDDEPT1,1,1) = '7' 
AND     I.GBAUTO NOT IN ('T','Z') 
AND     I.STATUS = '0'
AND     I.SUNSUNAP = '3' 
AND     I.ORDERGUBUN = 'H'
AND     I.WARDCODE = '{inPatient.WARDCODE}' 
AND     I.DEPTCODE = '{inPatient.DEPTCODE}' 
AND     I.DRCODE   = '{inPatient.DRCODE}'  
AND     I.JUPSU = '0'
AND     N.SUNEXT = I.SUCODE
AND     (I.STOPCHKID IS NULL OR TRIM(I.STOPCHKID) = '')
AND     I.STOPCHKDATE IS NULL 
            )
";
            var count = DBaser.Execute(sql, new WhereBuilder());
            return count;
        }

        /// <summary>
        /// 存在登记医嘱True;
        /// </summary>
        public bool ExixtOrder(string orderNO)
        {
            var sql = $@"
SELECT      Count(*) Count 
FROM        TWOCS_PRSLIP_CHECK    
";
            var wb = new WhereBuilder();
            wb.And("ORDERNO", orderNO);

            var Count = DBaser.QueryValue<int>(sql, wb);
            return Count > 0;
        }

        /// <summary>
        /// 存在登记医嘱True;
        /// </summary>
        public WindowJobs[] GetWindowJobs()
        {
            var sql = $@"
SELECT      PUMCODE CODE, 
            COUNT(*)COUNT  
FROM        TWORD_JBINFO_180104 
WHERE       MINQTY='药房报道机日志'
and         placecode=to_char(sysdate,'yyyy-MM-dd')
GROUP BY    PUMCODE    
";
            var windowJobs = DBaser.Query<WindowJobs>(sql);
            return windowJobs;
        }

        /// <summary>
        /// 排队人最少的窗口
        /// </summary>
        /// <param name="dm">是否取毒麻药品</param>
        /// <param name="working"></param>
        public WindowJobs[] GetWorkingWindowsLineUpNums(bool dm, bool working = true)
        {
            var sql = $@"
SELECT      AA.CHUANGKUO CODE,AA.CHUANGKUO CHUANGKOU ,BB.COUNT FROM (
SELECT      CHUANGKUO       
FROM        TWOCS_PRCAOZUO  P, (
SELECT      CODE 
FROM        TWBAS_BASECODE    
WHERE       BUSINESS='药房'      
AND         BUN='窗口'      
AND         NFLAG1='1') B     
WHERE       JOBDATE = TRUNC(SYSDATE)   
AND         GBEND='0'   
AND         P.CHUANGKUO=B.CODE  
{(dm ? "AND  CHUANGKUO IN ('14','15')" : "")}
GROUP BY    CHUANGKUO
) AA
LEFT JOIN 
(
SELECT      COUNT(*) COUNT ,CHUANGKOU 
FROM (
SELECT      * 
FROM ( 
SELECT      P.PTNO,  p.GBPRINT, p.GBOUT, 
            A.SNAME,A.SEX, A.BIRTHDATE,P.BI CODE, 
            TW_HSP_PMPA.TWBAS_GETNAME.PTBINAME(P.PTNO) CODENAME, 
            P.CHUANGKOU,
            --TW_HSP_PMPA.TWBAS_COMMON.S_CodeName('药房','窗口',P.CHUANGKOU) CHUANGKOU,  
            P.STATUS, A.JUSO, A.JUMIN  ,P.BDATE,P.TUYAKNO,TO_CHAR(P.CHECKTIME,'YYYY-MM-DD HH24:MI') CHECKTIME 
FROM        TW_HSP_OCS.TWOCS_PRSLIP    P, 
            TW_HSP_PMPA.TWBAS_PATIENT  A 
WHERE       P.PTNO     = A.PTNO 
AND ( P.GBOUT = '0' OR P.GBOUT IS NULL )
AND P.STATUS ='0'
AND P.FLAG = '2'
--AND  P.Gbio     = 'O'
AND  P.ENTDATE = TO_DATE( to_char(sysdate,'yyyy-MM-dd'),'YYYY-MM-DD')
) GROUP BY PTNO,  GBPRINT, GBOUT,TUYAKNO,CHECKTIME, 
           SNAME,SEX, BIRTHDATE, CODE,CODENAME, 
           STATUS, JUSO, JUMIN  ,BDATE,CHUANGKOU   
 -- ORDER BY TUYAKNO,GBPRINT 
)
GROUP BY CHUANGKOU
) BB 
ON AA.CHUANGKUO=BB.CHUANGKOU

";
            var jobs = DBaser.Query<WindowJobs>(sql);
            if (dm && jobs.Length == 0)
                return GetWorkingWindowsLineUpNums(false);
            //throw new Exception("毒麻取药窗口10 、11 未开放！");
            else if (jobs.Length == 0)
                throw new Exception("取药窗口未开放！");
            return jobs;
        }

        /// <summary>
        /// 报道登记
        /// </summary>
        public int InPatientSignIn(TWOCS_PRSLIP_CHECK orderInfo, InPatientCheck ptInfo)
        {
            DBaser.Insert("TWOCS_PRSLIP_CHECK");
            DBaser["GBIO"] = orderInfo.GBIO;
            DBaser["PTNO"] = ptInfo.PTNO;
            DBaser["BI"] = orderInfo.BI;
            DBaser["DEPTCODE"] = ptInfo.DEPTCODE;
            DBaser["DRCODE"] = ptInfo.DRCODE;
            DBaser["WARDCODE"] = ptInfo.WARDCODE;
            DBaser["ROOMCODE"] = orderInfo.ROOMCODE;
            DBaser["ORDERGUBUN"] = orderInfo.ORDERGUBUN;
            DBaser["ORDERCODE"] = orderInfo.ORDERCODE;
            DBaser["SENDDEPT1"] = orderInfo.SENDDEPT1;
            DBaser["BDATE"] = orderInfo.BDATE;
            DBaser["SUCODE"] = orderInfo.SUCODE;
            DBaser["BUN"] = orderInfo.BUN;
            DBaser["SLIPNO"] = orderInfo.SLIPNO;
            DBaser["QTY"] = orderInfo.QTY;
            DBaser["NAL"] = orderInfo.NAL;
            DBaser["DIV"] = orderInfo.DIV;
            DBaser["REALQTY"] = orderInfo.REALQTY;
            DBaser["DOSCODE"] = orderInfo.DOSCODE;
            DBaser["GRP"] = orderInfo.GRP;
            DBaser["REMARK"] = orderInfo.REMARK;
            //DBaser["STATUS"] = orderInfo.STATUS;
            DBaser["ORDERNO"] = orderInfo.ORDERNO;
            DBaser["CHUANGKOU"] = orderInfo.CHUANGKOU;
            DBaser["PUMCODE"] = orderInfo.PUMCODE;
            //DBaser["PRESNO"] = orderInfo.PRESNO;
            DBaser["CHECKDATE"] = Environ.SlowTime.Date;// orderInfo.CHECKDATE;
            DBaser["FLAG"] = orderInfo.FLAG;
            DBaser["CHECKTIME"] = Environ.SlowTime.ToString("HH:mm");// orderInfo.CHECKTIME;
            //DBaser["OUTUSER"] = orderInfo.OUTUSER;
            //DBaser["OUTDATE"] = orderInfo.OUTDATE;
            //DBaser["OUTTIME"] = orderInfo.OUTTIME;
            //DBaser["REUSER"] = orderInfo.REUSER;
            //DBaser["REDATE"] = orderInfo.REDATE;
            //DBaser["RETIME"] = orderInfo.RETIME;
            //DBaser["HOSPITALIZEDUID"] = orderInfo.HOSPITALIZEDUID;
            return DBaser.AcceptChanges();
        }

        /// <summary>
        /// 记录发药分配窗口记录
        /// </summary>
        public int LogPharm(TWORD_JBINFO_180104 entity)
        {
            try
            {
                DBaser.Insert("TWORD_JBINFO_180104");
                DBaser["JBUSE"] = entity.JBUSE;
                DBaser["PUMCODE"] = entity.PUMCODE;
                DBaser["PLACECODE"] = entity.PLACECODE;
                DBaser["MINQTY"] = entity.MINQTY;
                return DBaser.AcceptChanges();
            }
            catch
            {
                return 0;
            }
        }
    }
}
