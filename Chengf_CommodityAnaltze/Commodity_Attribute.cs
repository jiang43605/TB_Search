using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chengf_CommodityAnaltze
{
    /// <summary>
    /// 商品类
    /// </summary>
   public class Commodity_Attribute
    {
       /// <summary>
       /// 搜索商品关键词名称
       /// </summary>
       public string Commodity_name
       {
           set;
           get;
       }
       /// <summary>
       /// 搜索的商品有几页
       /// </summary>
       public string Commodity_PageQuantity
       {
           set;
           get;
       }
       /// <summary>
       /// 搜索的商品有多少个
       /// </summary>
       public string Commodity_Quantity
       {
           set;
           get;
       }
       /// <summary>
       /// 搜索的商品预计有多少个
       /// </summary>
       public string Commodity_PreQuantity
       {
           set;
           get;
       }
    }
}
