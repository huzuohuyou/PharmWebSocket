using System;

namespace Pharmacy_Reporter.Entity
{
    /// <summary>
    /// 工作量实体
    /// </summary>
    [Serializable]
    public class WindowJobs
    {
        /// <summary>
        /// 发药窗口字符串表达式
        /// </summary>
        public string Name => $@"发药窗口 {int.Parse(CODE.Trim()) - 4} ";

        /// <summary>
        /// 窗口编号
        /// </summary>
        public string CODE { get; set; }

        /// <summary>
        /// 工作量
        /// </summary>
        public int COUNT { get; set; }
    }
}
