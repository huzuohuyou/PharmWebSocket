using System;

namespace Pharmacy_Reporter.Entity
{
    /// <summary>
    /// 报到条
    /// </summary>
    [Serializable]
    public class SigninStrip
    {
        /// <summary>
        /// 病人编号
        /// </summary>
        public string PTNO { get; set; }

        ///<summary>
        /// 病人名称
        /// </summary>
        public string PTNAME { get; set; }

        /// <summary>
        /// 窗口名
        /// </summary>
        public string WINDOW_NAME { get; set; }

        /// <summary>
        /// 报道时间
        /// </summary>
        public string CHECK_DATE { get; set; }

        /// <summary>
        /// 打印模板字符串表达式
        /// </summary>
        public override string ToString()
        {
            return $@"PTNO:{PTNO} PTNAME:{PTNAME} WINDOW_NAME:{WINDOW_NAME} CHECK_DATE:{CHECK_DATE}";
        }
    }
}
