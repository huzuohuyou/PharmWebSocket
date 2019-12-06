using System;

namespace Pharmacy_Reporter.Entity
{
    /// <summary>
    /// 报道状态登记实体
    /// </summary>
    [Serializable]
    public class SignInEntity
    {
        /// <summary>
        /// 患者编号
        /// </summary>
        public string PTNO { get; set; }
        /// <summary>
        /// 报道日期
        /// </summary>
        public string CHECKDATE { get; set; }
        /// <summary>
        /// 报道时间
        /// </summary>
        public string CHECKTIME { get; set; }
        /// <summary>
        /// ?
        /// </summary>
        public string FLAG { get; set; }
        /// <summary>
        /// 取窗口号
        /// </summary>
        public string TuyakNo { get; set; }
        /// <summary>
        /// 取药窗口
        /// </summary>
        public string CHUANGKOU { get; set; }
        /// <summary>
        /// 是否取药
        /// </summary>
        public string IS_DONE { get; set; }
        /// <summary>
        /// 开单日期
        /// </summary>
        public string BDATE { get; set; }
        /// <summary>
        /// 处方单号
        /// </summary>
        public string PRESNO { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CREATE_USER_ID { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public string CREATE_TIME { get; set; }
        /// <summary>
        /// 最后更新人
        /// </summary>
        public string UPDATE_USER_ID { get; set; }
        /// <summary>
        /// 最后更新日期
        /// </summary>
        public string UPDATE_TIME { get; set; }
        /// <summary>
        /// 数据库编号
        /// </summary>
        public string DBCODE { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public string TIMESPAN { get; set; }

    }
}
