using Chengf_BaoBeiAnalyze;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chengf_BaoBeiMonitor
{
    public static class MonitorDataManage
    {
        /// <summary>
        /// 原始数据
        /// </summary>
        public static List<BaoBei_Attribute> OriginalAttribute
        {
            set;
            get;
        }
        /// <summary>
        /// 数据处理，需要赋值originalattribute
        /// </summary>
        /// <param name="newattribute">更新后的数据</param>
        /// <param name="oldattribute">前一次数据</param>
        /// <param name="datasavelist">辅助计算数据</param>
        /// <returns></returns>
        public static List<MonitorDataAttribute> DataManage(List<BaoBei_Attribute> newattribute, List<BaoBei_Attribute> oldattribute, ref DataTemporarySave[] datasavelist)
        {
            List<MonitorDataAttribute> newmda = new List<MonitorDataAttribute>();
            for (int i = 0; i < oldattribute.Count; i++)
            {
                MonitorDataAttribute mda = new MonitorDataAttribute();
                #region 进行数据比对
                mda.QHNum = (newattribute[i].Baobei_trading.Quantity - oldattribute[i].Baobei_trading.Quantity);//宝贝销量的瞬间改变值     

                #region 总销量的判别，如果增长为负，则继续使用上次的销量
                //mda.Totalnum = (newattribute[i].Baobei_trading.Quantity - OriginalAttribute[i].Baobei_trading.Quantity);//距今的总销量
                //if (mda.Totalnum < datasavelist[i].TotalnumSave)
                //{
                //    mda.Totalnum = datasavelist[i].TotalnumSave;
                //}
                //else
                //{
                //    datasavelist[i].TotalnumSave = mda.Totalnum;
                //} 
                mda.Totalnum = newattribute[i].Baobei_TimeOfTrade;
                #endregion

                mda.NowTime = newattribute[i].Baobei_Lastupdatatime.ToString("HH:mm:ss");//当前时间
                mda.Dataid = newattribute[i].Baobei_manager;//宝贝的掌柜名
                mda.QHNum_time = tool_time(newattribute[i], oldattribute[i]);//销售改变所用时间
                #region 对价格变化的监测
                if (newattribute[i].Baobei_price != oldattribute[i].Baobei_price)
                {
                    datasavelist[i].PriceSave += "/" + newattribute[i].Baobei_price.ToString();
                    mda.Price_State = datasavelist[i].PriceSave;
                }
                else
                {
                    mda.Price_State = datasavelist[i].PriceSave;
                } 
                #endregion

                #region 判断总时间，销量发生变化才计数，否则不变
                if (mda.QHNum == 0)
                {
                    mda.Totalnum_time = datasavelist[i].Totalnum_time_Save;
                }
                else
                {
                    mda.Totalnum_time = newattribute[i].Baobei_Lastupdatatime.ToShortTimeString();
                    datasavelist[i].Totalnum_time_Save = mda.Totalnum_time;
                }
                #endregion

                #region 宝贝的状态，即上下架
                if (newattribute[i].Baobei_State) mda.Baobei_State = "上架中";
                else mda.Baobei_State = "已下架"; 
                #endregion

                #region 判断价格和销量直接的关系得到估计的进账，使用了外部传递的数据暂存。
                //if (oldattribute[i].Baobei_price != newattribute[i].Baobei_price)
                //{
                //    datasavelist[i].ProfitSave += (oldattribute[i].Baobei_trading.Quantity - datasavelist[i].VolumeSave) * oldattribute[i].Baobei_price;
                //    datasavelist[i].VolumeSave = oldattribute[i].Baobei_trading.Quantity;
                //    mda.MaybeEarnMoney = datasavelist[i].ProfitSave + (newattribute[i].Baobei_price * mda.QHNum);
                //}
                //else
                //{
                //    mda.MaybeEarnMoney = datasavelist[i].ProfitSave + (newattribute[i].Baobei_trading.Quantity - datasavelist[i].VolumeSave) * newattribute[i].Baobei_price;
                //}
                mda.MaybeEarnMoney = newattribute[i].Baobei_TimeOfPrice;
                #endregion

                #region 购买的人数
                //if (mda.MaybeEarnMoney < 0) mda.MaybeEarnMoney = 0;
                //mda.PurchPeopleNum = (newattribute[i].Baobei_trading.PaySuccessItems - OriginalAttribute[i].Baobei_trading.PaySuccessItems) + (newattribute[i].Baobei_trading.ConfirmGoodsItems - OriginalAttribute[i].Baobei_trading.ConfirmGoodsItems);
                mda.PurchPeopleNum = newattribute[i].Baobei_TimeOfPeopleNum;
                #endregion
                #endregion
                newmda.Add(mda);
            }
            return newmda;
        }
        public static List<MonitorDataAttribute> DataManage(List<BaoBei_Attribute> newattribute, List<BaoBei_Attribute> oldattribute, List<BaoBei_Attribute> originalattribute)
        {
            List<MonitorDataAttribute> newmda = new List<MonitorDataAttribute>();
            for (int i = 0; i < oldattribute.Count; i++)
            {
                MonitorDataAttribute mda = new MonitorDataAttribute();
                #region 进行数据比对
                mda.QHNum = (newattribute[i].Baobei_trading.Quantity - oldattribute[i].Baobei_trading.Quantity);
                mda.Totalnum = (newattribute[i].Baobei_trading.Quantity - originalattribute[i].Baobei_trading.Quantity);
                mda.NowTime = newattribute[i].Baobei_Lastupdatatime.ToString("HH:mm:ss");
                mda.Dataid = newattribute[i].Baobei_manager;
                mda.QHNum_time = tool_time(newattribute[i], oldattribute[i]);
                mda.Totalnum_time = tool_time(newattribute[i], originalattribute[i]);
                if (newattribute[i].Baobei_State) mda.Baobei_State = "上架中";
                else mda.Baobei_State = "已下架";
                mda.MaybeEarnMoney = newattribute[i].Baobei_price * mda.Totalnum;
                if (mda.MaybeEarnMoney < 0) mda.MaybeEarnMoney = 0;
                mda.PurchPeopleNum = (newattribute[i].Baobei_trading.PaySuccessItems - originalattribute[i].Baobei_trading.PaySuccessItems) + (newattribute[i].Baobei_trading.ConfirmGoodsItems - originalattribute[i].Baobei_trading.ConfirmGoodsItems);
                #endregion
                newmda.Add(mda);
            }
            return newmda;
        }
        /// <summary>
        /// 对时间格式化
        /// </summary>
        /// <param name="newattribute"></param>
        /// <param name="oldattribute"></param>
        /// <returns></returns>
        private static string tool_time(BaoBei_Attribute newattribute, BaoBei_Attribute oldattribute)
        {
            int intm = int.Parse(newattribute.Baobei_Lastupdatatime.Subtract(oldattribute.Baobei_Lastupdatatime).TotalSeconds.ToString("0"));
            if (intm / 60 == 0) return intm.ToString() + "s";
            else if (intm / 60 != 0 && intm / 3600 == 0) return (intm / 60).ToString() + ":" + (intm % 60).ToString() + "";
            else return (intm / 3600).ToString() + ":" + ((intm % 3600) / 60).ToString() + ":" + ((intm % 3600) % 60).ToString() + "";
        }
    }
}
