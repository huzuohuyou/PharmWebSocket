using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharmacy_Reporter.Entity
{
    /// <summary>
    /// 日间手术发药报道实体
    /// </summary>
    [Serializable]
    public class TWOCS_PRSLIP_CHECK
    {
        /// <summary>
        /// //门诊/住院                NOT NULL,
        /// </summary>
        public string GBIO { get { return "I"; } }
        /// <summary>
        /// // 患者编码              NOT NULL,
        /// </summary>
        public string PTNO { get; set; }
        /// <summary>
        /// // 患者身份,
        /// </summary>
        public string BI { get; set; }
        /// <summary>
        /// // 科室,
        /// </summary>
        public string DEPTCODE { get; set; }
        /// <summary>
        /// // 负责医生,
        /// </summary>
        public string DRCODE { get; set; }
        /// <summary>
        /// // 病区,
        /// </summary>
        public string WARDCODE { get; set; }
        /// <summary>
        /// // 房间号,
        /// </summary>
        public string ROOMCODE { get; set; }
        /// <summary>
        /// // 医嘱类别,
        /// </summary>
        public string ORDERGUBUN { get; set; }
        /// <summary>
        /// //医嘱编码,
        /// </summary>
        public string ORDERCODE { get; set; }
        /// <summary>
        /// // 传送部门,
        /// </summary>
        public string SENDDEPT1 { get; set; }
        /// <summary>
        /// // 医嘱日期,
        /// </summary>
        public DateTime BDATE { get; set; }
        /// <summary>
        /// // 价格编码,
        /// </summary>
        public string SUCODE { get; set; }
        /// <summary>
        /// // 分类,
        /// </summary>
        public string BUN { get; set; }
        /// <summary>
        /// // 医嘱分类,
        /// </summary>
        public string SLIPNO { get; set; }
        /// <summary>
        /// // 数量,
        /// </summary>
        public string QTY { get; set; }
        /// <summary>
        /// // 天数,
        /// </summary>
        public string NAL { get; set; }
        /// <summary>
        /// // 用量,
        /// </summary>
        public string DIV { get; set; }
        /// <summary>
        /// // 实际用量,
        /// </summary>
        public string REALQTY { get; set; }
        /// <summary>
        /// // 用法,
        /// </summary>
        public string DOSCODE { get; set; }
        /// <summary>
        /// // 分组,
        /// </summary>
        public string GRP { get; set; }
        /// <summary>
        /// // 备注,
        /// </summary>
        public string REMARK { get; set; }
        /// <summary>
        /// // 状态,       0未发药，1发药，取消发药
        /// </summary>
        public string STATUS { get { return "0"; } }
        /// <summary>
        /// // 医嘱号,
        /// </summary>
        public string ORDERNO { get; set; }
        /// <summary>
        /// // 窗口,
        /// </summary>
        public string CHUANGKOU { get; set; }
        /// <summary>
        /// // xy码,
        /// </summary>
        public string PUMCODE { get; set; }
        /// <summary>
        /// // 处方号,
        /// </summary>
        public string PRESNO { get; set; }
        /// <summary>
        /// // 签到日期,
        /// </summary>
        public DateTime CHECKDATE { get; set; }
        /// <summary>
        /// // 状态             0未签到，1签到，2取消签到
        /// </summary>
        public string FLAG { get { return "1"; } }
        /// <summary>
        /// // 签到时间,    
        /// </summary>
        public string CHECKTIME { get; set; }
        /// <summary>
        /// // 发药人(6 BYTE),
        /// </summary>
        public string OUTUSER { get; set; }
        /// <summary>
        /// // 发药日期,
        /// </summary>
        public string OUTDATE { get; set; }
        /// <summary>
        /// // 发药时间,
        /// </summary>
        public string OUTTIME { get; set; }
        /// <summary>
        /// // 退药人(6 BYTE),
        /// </summary>
        public string REUSER { get; set; }
        /// <summary>
        /// // 退药日期,
        /// </summary>
        public string REDATE { get; set; }
        /// <summary>
        /// // 退药时间,
        /// </summary>
        public string RETIME { get; set; }
        /// <summary>
        /// // 患者主索引(16 BYTE),
        /// </summary>
        public string HOSPITALIZEDUID { get; set; }





    }
}
