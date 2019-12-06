using System;

namespace Pharmacy_Reporter.Entity
{
    /// <summary>
    /// 处方单实体
    /// </summary>
    [Serializable]
    public class Prescription
    {
        /// <summary>
        /// 患者编号
        /// </summary>
        public string Ptno { get; set; }
        /// <summary>
        /// 医嘱日期
        /// </summary>
        public string Bdate { get; set; }
        /// <summary>
        /// 处方单号
        /// </summary>
        public string Presno { get; set; }
    }
}
