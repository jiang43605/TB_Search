using Chengf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Chengf_BaoBeiAnalyze
{
    /// <summary>
    /// 宝贝的交易记录，对于监测宝贝的实际交易很重要
    /// </summary>
    public class BaoBei_TradeRecord
    {
        /// <summary>
        /// 初始化该类
        /// </summary>
        public BaoBei_TradeRecord()
        {

        }
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
        public static BaoBei_TradeRecord GetTradeRecord(string uri, DateTime datetime, CookieContainer cookiecontainertool, double defalutPrice = 0)
        {

            BaoBei_TradeRecord traceRecord = new BaoBei_TradeRecord();
            Chengf.Cf_HttpWeb newhttpweb = new Chengf.Cf_HttpWeb();
            int endpoint = 0;
            var html = newhttpweb.CPostOrGet(uri, Chengf.HttpMethod.GET).HtmlValue;

            string tradrecordh_html = "https://" +
                                      Regex.Match(html, @"(?<=rateCounterApi(\s)*:(\s)*'//)(?<item>.*)(?=')").Groups[
                                          "item"].Value
                                          .Replace("itemId", "auctionNumId")
                                          .Replace("detailCount.do", "feedRateList.htm");

            var userNumId = Regex.Match(html, @"(?<=sellerId(\s)*: ')(?<id>\d*)(?=')").Groups["id"].Value;

            //https://rate.taobao.com/feedRateList.htm?auctionNumId=534169093991&userNumId=702657768&currentPageNum=1&pageSize=20000000&orderType=sort_weight&callback=jsonp_tbcrate_reviews_list
            tradrecordh_html += $"&userNumId={userNumId}&pageSize=20&orderType=feedbackdate&callback=jsonp_tbcrate_reviews_list";
            //string recordhtml = "";
            string strreplace = "&currentPageNum=1";//为下面循环进行数据暂存
            tradrecordh_html += strreplace;
            newhttpweb.Referer = uri;
            string timeEquals = newhttpweb.PostOrGet(tradrecordh_html, HttpMethod.GET, cookiecontainertool).HtmlValue;
            if (timeEquals.IndexOf(datetime.ToLongDateString(), StringComparison.Ordinal) == -1 || timeEquals.Contains("sec.taobao"))
                return null;

            var totalrecord = new BaoBei_TradeRecord
            {
                BuyTime = new List<DateTime>(),
                BuyPrice = new List<double>(),
                BuyNum = new List<int>(),
                BuyerName = new List<string>()
            };

            for (int i = 1; ; i++)
            {
                try
                {
                    string pagenum = "&currentPageNum=" + i;
                    tradrecordh_html = tradrecordh_html.Replace(strreplace, pagenum);
                    strreplace = pagenum;

                    newhttpweb.Referer = uri;
                    string recordhtml_copy = newhttpweb.PostOrGet(tradrecordh_html, HttpMethod.GET, cookiecontainertool).HtmlValue;

                    var tr = ResolveHtml(recordhtml_copy.Trim('\r', '\n', ' '), defalutPrice);
                    if (tr.BuyTime.Any(o => o.ToShortDateString() != datetime.ToShortDateString()))
                    {
                        totalrecord += FilterInCorrectDate(tr, datetime);
                        return totalrecord;
                    }

                    totalrecord += tr;

                    Thread.Sleep(500);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
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
            string record_html = newhttpweb.PostOrGet(uri, Chengf.HttpMethod.GET).HtmlValue;
            return HtmlToTradeRecord(record_html);
        }

        /// <summary>
        /// 去除不匹配的时间
        /// </summary>
        /// <param name="tr"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static BaoBei_TradeRecord FilterInCorrectDate(BaoBei_TradeRecord tr, DateTime dt)
        {
            while (tr.BuyTime.Any(o => o.ToShortDateString() != dt.ToShortDateString()))
            {
                var index = tr.BuyTime.FindIndex(o => o.ToShortDateString() != dt.ToShortDateString());
                tr.BuyTime.RemoveAt(index);
                tr.BuyNum.RemoveAt(index);
                tr.BuyPrice.RemoveAt(index);
                tr.BuyerName.RemoveAt(index);
            }

            return tr;
        }

        /// <summary>
        /// 解析html使用json解析，需要设置一个默认的价格值
        /// </summary>
        /// <param name="html"></param>
        /// <param name="defalutPrice"></param>
        /// <returns></returns>
        private static BaoBei_TradeRecord ResolveHtml(string html, double defalutPrice)
        {
            html = html.Replace("jsonp_tbcrate_reviews_list(", string.Empty).TrimEnd(')');

            var json = JToken.Parse(html);
            var tr = new BaoBei_TradeRecord
            {
                BuyTime = new List<DateTime>(),
                BuyPrice = new List<double>(),
                BuyNum = new List<int>(),
                BuyerName = new List<string>()
            };

            foreach (var comment in json["comments"])
            {
                tr.BuyTime.Add(string.IsNullOrWhiteSpace(comment["date"].Value<string>()) ? DateTime.MinValue : comment["date"].Value<DateTime>());
                tr.BuyNum.Add(comment["buyAmount"].Value<int>());
                tr.BuyPrice.Add(string.IsNullOrWhiteSpace(comment["bidPriceMoney"].ToString()) ? defalutPrice : comment["bidPriceMoney"]["amount"].Value<double>());
                tr.BuyerName.Add(comment["user"]["nick"].Value<string>());
            }

            return tr;
        }

        /// <summary>
        /// 解析特定网页的内容，返回一个解析的对象
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        [Obsolete("仅用于阅读记录")]
        private static BaoBei_TradeRecord HtmlToTradeRecord(string html)
        {
            BaoBei_TradeRecord tradrecord = new BaoBei_TradeRecord();
            #region 得到买家的名字
            List<string> buyname_list = Cf_String.ExtractStringNoQH(html, "\"nick\":\"", "\"");
            buyname_list = buyname_list.Select(o => Regex.Unescape(o)).ToList();
            #endregion
            tradrecord.BuyerName = buyname_list;
            #region 得到购买的价格
            List<string> buyprice_strlist = Cf_String.ExtractStringNoQH(html, "\"amount\":", ",");
            List<double> buyprice_doblist = new List<double>();
            foreach (var item in buyprice_strlist)
            {
                buyprice_doblist.Add(double.Parse(item));
            }
            #endregion
            tradrecord.BuyPrice = buyprice_doblist;
            #region 得到购买的数量
            List<string> buynum_strlist = Cf_String.ExtractStringNoQH(html, "\"buyAmount\":", "}");
            List<int> buynum_intlist = new List<int>();
            foreach (var item in buynum_strlist)
            {
                buynum_intlist.Add(int.Parse(item));
            }
            #endregion
            tradrecord.BuyNum = buynum_intlist;
            #region 得到购买的时间
            List<string> buytime_strtime = Cf_String.ExtractStringNoQH(html, "\"date\":\"", "\"");
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

        public static BaoBei_TradeRecord operator +(BaoBei_TradeRecord b1, BaoBei_TradeRecord b2)
        {
            b1.BuyNum.AddRange(b2.BuyNum);
            b1.BuyPrice.AddRange(b2.BuyPrice);
            b1.BuyTime.AddRange(b2.BuyTime);
            b1.BuyerName.AddRange(b2.BuyerName);

            return b1;
        }
    }
}
