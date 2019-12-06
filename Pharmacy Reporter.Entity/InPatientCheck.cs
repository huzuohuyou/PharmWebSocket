using System;

namespace Pharmacy_Reporter.Entity
{
    /// <summary>
    /// 日间手术签到表
    /// </summary>
    [Serializable]
    public class InPatientCheck
    {
        /// <summary>
        /// 医嘱区分类型
        /// </summary>
        public string ORDERGUBUN { get; set; }
        /// <summary>
        /// 患者编号
        /// </summary>
        public string PTNO { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Sname { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public string Age { get; set; }
        /// <summary>
        /// 病区
        /// </summary>
        public string WARDCODE { get; set; }
        /// <summary>
        /// 房间号
        /// </summary>
        public string Roomcode { get; set; }
        /// <summary>
        /// 创号
        /// </summary>
        public string Bedno { get; set; }
        /// <summary>
        /// 病区
        /// </summary>
        public string WARDNAME { get; set; }
        /// <summary>
        /// 患者区分
        /// </summary>
        public string BI1 { get; set; }
        /// <summary>
        /// 患者区分
        /// </summary>
        public string BI { get; set; }
        /// <summary>
        /// 科室
        /// </summary>
        public string DEPTNAME { get; set; }
        /// <summary>
        /// 科室编码
        /// </summary>
        public string DEPTCODE { get; set; }
        /// <summary>
        /// 医生姓名
        /// </summary>
        public string DRNAME { get; set; }
        /// <summary>
        /// 医生编码
        /// </summary>
        public string DRCODE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AMSET1 { get; set; }
        /// <summary>
        /// 收费状态
        /// </summary>
        public string JUPSU { get; set; }
        /// <summary>
        /// 入院日期
        /// </summary>
        public string INDATE { get; set; }

    }
}
