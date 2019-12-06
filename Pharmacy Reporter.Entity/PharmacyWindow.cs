using System;

namespace Pharmacy_Reporter.Entity
{
    /// <summary>
    /// 药房窗口
    /// </summary>
    [Serializable]
    public class PharmacyWindow
    {
        /// <summary>
        /// 窗口编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 窗口名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 取药工作量
        /// </summary>
        public int Num { get; set; }
    }
}
