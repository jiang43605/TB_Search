using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chengf_BaoBeiMonitor
{
    public class MonitorDataAttribute
    {
        int num;
        string nowtime;
        int totalnum;
        string dataid;
        string qHNum_time;
        string totalnum_time;
        string baobei_State;
        double maybeEarnMoney;
        int purchPeopleNum;
        string price_State;
        /// <summary>
        /// 宝贝的价格状态
        /// </summary>
        public string Price_State
        {
            get { return price_State; }
            set { price_State = value; }
        }
        /// <summary>
        /// 截至目前购买的人数
        /// </summary>
        public int PurchPeopleNum
        {
            get { return purchPeopleNum; }
            set { purchPeopleNum = value; }
        }
        /// <summary>
        /// 估计已挣取的Money
        /// </summary>
        public double MaybeEarnMoney
        {
            get { return maybeEarnMoney; }
            set { maybeEarnMoney = value; }
        }
        /// <summary>
        /// 宝贝上下架属性
        /// </summary>
        public string Baobei_State
        {
            get { return baobei_State; }
            set { baobei_State = value; }
        }
        /// <summary>
        /// 从元数据到目前最新数据经历的时间
        /// </summary>
        public string Totalnum_time
        {
            get { return totalnum_time; }
            set { totalnum_time = value; }
        }
        /// <summary>
        /// 数据发生改变所用的时间
        /// </summary>
        public string QHNum_time
        {
            get { return qHNum_time; }
            set { qHNum_time = value; }
        }
        /// <summary>
        /// 该数据的标识ID
        /// </summary>
        public string Dataid
        {
            get { return dataid; }
            set { dataid = value; }
        }
        /// <summary>
        /// 对于元数据而言的新增数目
        /// </summary>
        public int Totalnum
        {
            get { return totalnum; }
            set { totalnum = value; }
        }
        /// <summary>
        /// 当前时间
        /// </summary>
        public string NowTime
        {
            get { return nowtime; }
            set { nowtime = value; }
        }
        /// <summary>
        /// 前后增加或少于
        /// </summary>
        public int QHNum
        {
            get { return num; }
            set { num = value; }
        }
        /// <summary>
        /// 将MonitorDataAttribute格式化传出来
        /// </summary>
        /// <param name="mda"></param>
        /// <returns></returns>
        public static string FormatAttribute(MonitorDataAttribute mda)
        {
            string dataattr = "";
            if (mda.Dataid.Length != 12)
            {
                dataattr += mda.Dataid + "：";
                for (int i = 0; i < 12 - mda.Dataid.Length - 1; i++)
                {
                    dataattr += "-";
                }
            }
            if (mda.Totalnum.ToString().Length != 5)
            {
                dataattr += mda.Totalnum.ToString();
                for (int i = 0; i < 5 - mda.Totalnum.ToString().Length; i++)
                {
                    dataattr += "-";
                }
            }
            if (mda.QHNum.ToString().Length != 4)
            {
                dataattr += mda.QHNum.ToString();
                for (int i = 0; i < 4 - mda.QHNum.ToString().Length; i++)
                {
                    dataattr += "-";
                }
            }
            dataattr += "【" + mda.NowTime + "】";
            return dataattr;
        }
    }
}
