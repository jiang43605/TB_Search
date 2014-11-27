using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chengf_BaoBeiAnalyze
{
   public class Trading_Quantity
    {
        int quantity;
        int confirmGoods;
        int paySuccess;
        int paySuccessItems;
        int confirmGoodsItems;
        int refundCount;
        /// <summary>
        /// 总的交易数量
        /// </summary>
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }
        /// <summary>
        /// 总的交易成功的商品数量
        /// </summary>
        public int ConfirmGoods
        {
            get { return confirmGoods; }
            set { confirmGoods = value; }
        }
        /// <summary>
        /// 确认收货的商品（30天内）
        /// </summary>
        public int PaySuccess
        {
            get { return paySuccess; }
            set { paySuccess = value; }
        }
        /// <summary>
        /// 正在交易中的商品（30天内）
        /// </summary>
        public int PaySuccessItems
        {
            get { return paySuccessItems; }
            set { paySuccessItems = value; }
        }
        /// <summary>
        /// 交易成功的商品（30天内）
        /// </summary>
        public int ConfirmGoodsItems
        {
            get { return confirmGoodsItems; }
            set { confirmGoodsItems = value; }
        }
        /// <summary>
        /// 纠纷退款（30天内）
        /// </summary>
        public int RefundCount
        {
            get { return refundCount; }
            set { refundCount = value; }
        }
    }
}
