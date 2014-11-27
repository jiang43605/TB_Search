using Chengf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chengf_BaoBeiAnalyze
{
    /// <summary>
    /// 宝贝的交易记录，对于监测宝贝的实际交易很重要
    /// </summary>
    public class BaoBei_TradeRecord
    {
        /// <summary>
        /// 买家
        /// </summary>
        public List<string> BuyerName
        {
            set;
            get;
        }
        /// <summary>
        /// 拍下价格
        /// </summary>
        public List<double> BuyPrice
        {
            set;
            get;
        }
        /// <summary>
        /// 拍下的数量
        /// </summary>
        public List<int> BuyNum
        {
            set;
            get;
        }
        /// <summary>
        /// 拍下时间
        /// </summary>
        public List<DateTime> BuyTime
        {
            set;
            get;
        }
        /// <summary>
        /// 得到指定某天的成交记录
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static BaoBei_TradeRecord GetTradeRecord(string uri, DateTime datetime)
        {
            BaoBei_TradeRecord traceRecord = new BaoBei_TradeRecord();
            Chengf.Cf_HttpWeb newhttpweb = new Chengf.Cf_HttpWeb();
            int endpoint = 0;
            string html = newhttpweb.PostOrGet(uri, Chengf.HttpMethod.GET)[1];
            string tradrecordh_html = Cf_String.ExtractStringNoQH(html, "data-api=\"", "\"")[0];
            tradrecordh_html += "&callback=Hub.data.records_reload";
            string recordhtml = "";
            string strreplace = "bid_page=1";//为下面循环进行数据暂存
            BaoBei_TradeRecord endrecordlist = new BaoBei_TradeRecord();//存储末尾的交易记录
            if (new Cf_HttpWeb().PostOrGet(tradrecordh_html, HttpMethod.GET)[1].IndexOf(datetime.ToString("yyyy-MM-dd")) == -1)
                return null;
            for (int i = 1; ; i++)
            {
                try
                {
                    string pagenum = "bid_page=" + i.ToString();
                    tradrecordh_html = tradrecordh_html.Replace(strreplace, pagenum);
                    strreplace = pagenum;
                    string recordhtml_copy = (string)newhttpweb.PostOrGet(tradrecordh_html, HttpMethod.GET)[1];
                    if (!IsDateEnd(HtmlToTradeRecord(recordhtml_copy).BuyTime, datetime, out endpoint) || recordhtml_copy.IndexOf("暂时还没有买家购买此宝贝") != -1 || i >= 100)
                    {
                        if (recordhtml_copy.IndexOf("暂时还没有买家购买此宝贝") != -1) { endrecordlist = null; break; }
                        endrecordlist = HtmlToTradeRecord(recordhtml_copy);
                        break;
                    }
                    recordhtml += recordhtml_copy;
                    Thread.Sleep(500);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            BaoBei_TradeRecord totalrecord = new BaoBei_TradeRecord();
            totalrecord = HtmlToTradeRecord(recordhtml);
            //将末尾记录和全段记录合并
            if (endrecordlist != null)
            {
                for (int i = 0; i < endpoint; i++)
                {
                    totalrecord.BuyerName.Add(endrecordlist.BuyerName[i]);
                    totalrecord.BuyNum.Add(endrecordlist.BuyNum[i]);
                    totalrecord.BuyPrice.Add(endrecordlist.BuyPrice[i]);
                    totalrecord.BuyTime.Add(endrecordlist.BuyTime[i]);
                }
            }

            return totalrecord;
        }
        /// <summary>
        /// 通过URI解析特定网页的内容，返回一个解析的对象
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        private static BaoBei_TradeRecord UriToTradeRecord(string uri)
        {
            BaoBei_TradeRecord tradrecord = new BaoBei_TradeRecord();
            Chengf.Cf_HttpWeb newhttpweb = new Chengf.Cf_HttpWeb();
            string record_html = newhttpweb.PostOrGet(uri, Chengf.HttpMethod.GET)[1];
            return HtmlToTradeRecord(record_html);
        }
        /// <summary>
        /// 解析特定网页的内容，返回一个解析的对象
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private static BaoBei_TradeRecord HtmlToTradeRecord(string html)
        {
            BaoBei_TradeRecord tradrecord = new BaoBei_TradeRecord();
            #region 得到买家的名字
            List<string> buyname_list = Cf_String.ExtractStringNoQH(html, "tb-sellnick\\\">", "</span> <");
            for (int i = 0; i < buyname_list.Count; i++)
            {
                buyname_list[i] = Cf_String.DeleteSpecificString(buyname_list[i], "<", ">");
            }
            #endregion
            tradrecord.BuyerName = buyname_list;
            #region 得到购买的价格
            List<string> buyprice_strlist = Cf_String.ExtractStringNoQH(html, "tb-rmb-num\\\">", "</em>");
            List<double> buyprice_doblist = new List<double>();
            foreach (var item in buyprice_strlist)
            {
                buyprice_doblist.Add(double.Parse(item));
            }
            #endregion
            tradrecord.BuyPrice = buyprice_doblist;
            #region 得到购买的数量
            List<string> buynum_strlist = Cf_String.ExtractStringNoQH(html, "<td class=\\\"tb-amount\\\">", "</td>");
            List<int> buynum_intlist = new List<int>();
            foreach (var item in buynum_strlist)
            {
                buynum_intlist.Add(int.Parse(item));
            }
            #endregion
            tradrecord.BuyNum = buynum_intlist;
            #region 得到购买的时间
            List<string> buytime_strtime = Cf_String.ExtractStringNoQH(html, "<td class=\\\"tb-start\\\">", "</td>");
            List<DateTime> buytime_datertime = new List<DateTime>();
            foreach (var item in buytime_strtime)
            {
                buytime_datertime.Add(DateTime.Parse(item));
            }
            #endregion
            tradrecord.BuyTime = buytime_datertime;
            return tradrecord;
        }
        /// <summary>
        /// 判断时间点是否吻合
        /// </summary>
        /// <param name="datetime"></param>
        /// <param name="datebool"></param>
        /// <param name="newdatetime"></param>
        /// <returns></returns>
        private static bool IsDateEnd(List<DateTime> datetime, DateTime datebool, out int newdatetime)
        {
            bool returnbool = true;
            newdatetime = 0;
            for (int i = 0; i < datetime.Count; i++)
            {
                if (datetime[i].ToShortDateString() != datebool.ToShortDateString())
                {
                    newdatetime = i;
                    return false;
                }
            }
            return returnbool;
        }
        /// <summary>
        /// 传递一个BaoBei_TradeRecord得出总的销售额
        /// </summary>
        /// <param name="traderecord"></param>
        /// <returns></returns>
        public static double TotalPrice(BaoBei_TradeRecord traderecord)
        {
            double totalprice = 0;
            for (int i = 0; i < traderecord.BuyNum.Count; i++)
            {
                totalprice += traderecord.BuyNum[i] * traderecord.BuyPrice[i];
            }
            totalprice = double.Parse(totalprice.ToString("0.00"));
            return totalprice;
        }
    }
}
