using System;

namespace Pharmacy_Reporter.Entity
{
    /// <summary>
    /// 药房报道表
    /// </summary>
    [Serializable]
    public class TWORD_JBINFO_180104
    {
        /// <summary>
        /// PATIENT_NO
        /// </summary>
        public string JBUSE { get; set; }
        /// <summary>
        /// 窗口
        /// </summary>
        public string PUMCODE { get; set; }
        /// <summary>
        /// 报道时间
        /// </summary>
        public string PLACECODE { get; set; }
        /// <summary>
        /// 处方号
        /// </summary>
        public string MINQTY { get; set; }
        /// <summary />
        public string MAXQTY { get; set; }
        /// <summary />
        public string BASEJQTY { get; set; }
        /// <summary />
        public string OPENFLAG { get; set; }
    }
}
