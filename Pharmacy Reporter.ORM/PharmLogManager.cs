using Framework.ORM;
using Pharmacy_Reporter.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharmacy_Reporter.ORM
{
    /// <summary>
    /// 日志记录表
    /// </summary>
    public class PharmLogManager : ORMbase
    {
        /// <summary>
        /// 记录发药分配窗口记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int LogPharm(TWORD_JBINFO_180104 entity)
        {
            DBaser.Insert("TWORD_JBINFO_180104");
            DBaser["JBUSE"] = entity.JBUSE;
            DBaser["PUMCODE"] = entity.PUMCODE;
            DBaser["PLACECODE"] = entity.PLACECODE;
            DBaser["MINQTY"] = entity.MINQTY;


            return DBaser.AcceptChanges();
        }
    }
}
