using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chengf_BaoBeiMonitor
{
    /// <summary>
    /// 用于数据的辅助，例如价格的临时保存
    /// </summary>
   public class DataTemporarySave
    {
      
       public DataTemporarySave()
       {
           ProfitSave = 0;
           VolumeSave = 0;
           Totalnum_time_Save = "0秒";
           UID_uri = null;
           PriceSave = null;
           TotalnumSave = 0;
       }
       /// <summary>
       /// 总销量保存
       /// </summary>
       public int TotalnumSave
       {
           set;
           get;
       }
       /// <summary>
       /// 价格保存
       /// </summary>
       public string PriceSave
       {
           set;
           get;
       }
       /// <summary>
       /// 数据暂存对象的标识符
       /// </summary>
       public string UID_uri
       {
           set;
           get;
       }
       /// <summary>
       /// 保存获得的利润
       /// </summary>
       public double ProfitSave
       {
           set;
           get;
       }
       /// <summary>
       /// 销量保存
       /// </summary>
       public double VolumeSave
       {
           set;
           get;
       }
       /// <summary>
       /// 保存总的间距时间
       /// </summary>
       public string Totalnum_time_Save
       {
           set;
           get;
       }
    }
}
