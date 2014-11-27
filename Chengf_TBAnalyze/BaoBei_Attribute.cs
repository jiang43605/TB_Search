using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chengf_BaoBeiAnalyze
{
    /// <summary>
    /// 激活下宝贝类
    /// </summary>
    public class BaoBei_Attribute
    {
        string baobei_name;
        double baobei_price;
        string baobei_manager;
        Trading_Quantity baobei_trading;
        BaoBei_comment baobei_comment;
        string baobei_link;
        bool baobei_State;
        DateTime baobei_Lastupdatatime;
        string rkingname;
        int baobei_TimeOfTrade;
        double baobei_TimeOfPrice;
        int baobei_TimeOfPeopleNum;
        /// <summary>
        /// 指定时间购买的人次
        /// </summary>
        public int Baobei_TimeOfPeopleNum
        {
            get { return baobei_TimeOfPeopleNum; }
            set { baobei_TimeOfPeopleNum = value; }
        }
        /// <summary>
        /// 指定时间总的销售额
        /// </summary>
        public double Baobei_TimeOfPrice
        {
            get { return baobei_TimeOfPrice; }
            set { baobei_TimeOfPrice = value; }
        }
        /// <summary>
        /// 指定时间卖出的宝贝数量
        /// </summary>
        public int Baobei_TimeOfTrade
        {
            get { return baobei_TimeOfTrade; }
            set { baobei_TimeOfTrade = value; }
        }
        /// <summary>
        /// 宝贝排名
        /// </summary>
        public string Baobei_Rkingname
        {
            get { return rkingname; }
            set { rkingname = value; }
        }
        /// <summary>
        /// 宝贝最后一次更新的时间
        /// </summary>
        public DateTime Baobei_Lastupdatatime
        {
            get { return baobei_Lastupdatatime; }
            set { baobei_Lastupdatatime = value; }
        }
        /// <summary>
        /// 宝贝目前的状态，是否出于上架中，false为下架状态，true为上架状态
        /// </summary>
        public bool Baobei_State
        {
            get { return baobei_State; }
            set { baobei_State = value; }
        }
        /// <summary>
        /// 宝贝的连接地址
        /// </summary>
        public string Baobei_link
        {
            get { return baobei_link; }
            set { baobei_link = value; }
        }
        /// <summary>
        /// 宝贝的名字
        /// </summary>
        public string Baobei_name
        {
            get { return baobei_name; }
            set { baobei_name = value; }
        }
        /// <summary>
        /// 宝贝掌柜名
        /// </summary>
        public string Baobei_manager
        {
            get { return baobei_manager; }
            set { baobei_manager = value; }
        }
        /// <summary>
        /// 宝贝价格
        /// </summary>
        public double Baobei_price
        {
            get { return baobei_price; }
            set { baobei_price = value; }
        }
        /// <summary>
        /// 宝贝交易量
        /// </summary>
        public Trading_Quantity Baobei_trading
        {
            get { return baobei_trading; }
            set { baobei_trading = value; }
        }
        /// <summary>
        /// 宝贝评论数量
        /// </summary>
        //public int Baobei_commentcount
        //{
        //    get { return baobei_commentcount; }
        //    set { baobei_commentcount = value; }
        //}
        /// <summary>
        /// 宝贝的评论
        /// </summary>
        public BaoBei_comment Baobei_comment
        {
            get { return baobei_comment; }
            set { baobei_comment = value; }
        }

    }
}
