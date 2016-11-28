using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chengf;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace Chengf_BaoBeiAnalyze
{
    public class BaoBei_analyze
    {
        Cf_HttpWeb myhttpweb = new Cf_HttpWeb();
        string getstring;

        /// <summary>
        /// 分析传过来的网页，返回该宝贝对象
        /// </summary>
        /// <param name="uri">网页地址</param>
        /// <param name="timeout">响应时间</param>
        /// <param name="objCookieContainer">总的Cookie传递集合</param>
        /// <param name="serchDateTime"></param>
        /// <returns></returns>
        public BaoBei_Attribute Analyze(string uri, int timeout, CookieContainer objCookieContainer,DateTime serchDateTime)
        {
            BaoBei_Attribute Baobei_Attribute = new BaoBei_Attribute();
            myhttpweb.Timeout = timeout;
            myhttpweb.AllowAutoRedirect = true;
            getstring = myhttpweb.PostOrGet(uri, HttpMethod.GET).HtmlValue;

            myhttpweb.Timeout = timeout;
            myhttpweb.EncodingSet = Regex.Match(getstring, "(?<=<meta.*charset=\"?)(?<charset>\\w+)(?=\"?)").Groups["charset"].Value;
            getstring = myhttpweb.PostOrGet(uri, HttpMethod.GET).HtmlValue;
            if (getstring.IndexOf("天猫Tmall.com", StringComparison.Ordinal) == -1
                && !getstring.Contains("301 Moved Permanently")
                && !getstring.Contains("302 Found"))
            {
                Baobei_Attribute.Baobei_link = uri;
                Baobei_Attribute.Baobei_name = Cf_String.ExtractStringNoQH(getstring, "<title>", "</title>")[0];//宝贝的名字
                Baobei_Attribute.Baobei_manager = Cf_String.ExtractStringNoQH(getstring, "sellerNick       : '", "'")[0];//宝贝掌柜的名字

                Baobei_Attribute.Baobei_price = Price(uri, timeout);//宝贝的实际价格
                Baobei_Attribute.Baobei_trading = Trading(uri, timeout);//宝贝交易量

                BaoBei_TradeRecord TradeRecord = BaoBei_TradeRecord.GetTradeRecord(uri, serchDateTime, objCookieContainer, Baobei_Attribute.Baobei_price);
                if (TradeRecord == null)
                {
                    Baobei_Attribute.Baobei_TimeOfTrade = 0;
                    Baobei_Attribute.Baobei_TimeOfPrice = 0;
                    Baobei_Attribute.Baobei_TimeOfPeopleNum = 0;
                }
                else
                {
                    Baobei_Attribute.Baobei_TimeOfTrade = TradeRecord.BuyNum.Sum();
                    Baobei_Attribute.Baobei_TimeOfPrice = BaoBei_TradeRecord.TotalPrice(TradeRecord);
                    Baobei_Attribute.Baobei_TimeOfPeopleNum = TradeRecord.BuyerName.Count;
                }
                //Baobei_Attribute.Baobei_commentcount = int.Parse(Cf_String.ExtractStringNoQH(getstring, "<em class=\"J_ReviewsCount\">", "</em>")[0]);//宝贝评论数量
                Baobei_Attribute.Baobei_comment = null;//Comment(uri, timeout);//获得评论类，已被关闭，非常耗时
                if (getstring.IndexOf("此宝贝已下架") != -1) Baobei_Attribute.Baobei_State = false;
                else Baobei_Attribute.Baobei_State = true;
                Baobei_Attribute.Baobei_Lastupdatatime = DateTime.Now;
                return Baobei_Attribute;
            }
            else
                return null;
        }
        /// <summary>
        /// 获取宝贝的实际价格
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        double Price(string uri, int timeout)
        {
            string gouri =
                $"https://detailskip.taobao.com/service/getData/1/p1/item/detail/sib.htm?itemId={Regex.Match(uri, @"(?<=id=)(?<id>\d+)(?=&)").Groups["id"].Value}&modules=price,xmpPromotion&callback=onSibRequestSuccess";

            myhttpweb.Timeout = timeout;
            myhttpweb.Referer = uri.Replace("#detail", string.Empty);
            string html_price = myhttpweb.PostOrGet(gouri, HttpMethod.GET).HtmlValue;

            var price = Regex.Match(html_price, "(?<=\"def\":.*\"price\":\")(?<price>\\d+.?\\d*)(?=\")").Groups["price"].Value;

            if (string.IsNullOrWhiteSpace(price))
            {
                var p = html_price.ExtractStringNoQH("\"price\":\"", "\"")
                    .First()
                    .Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

                return p.Select(o => double.Parse(o.Trim())).Average();
            }

            return double.Parse(price);


            //if (html_price.IndexOf("price:\"") == -1)
            //{
            //    return double.Parse(Cf_String.ExtractStringNoQH(getstring, "price:", ",")[0]);
            //}
            //else
            //{
            //    return double.Parse(Cf_String.ExtractStringNoQH(html_price, "price:\"", "\"")[0]);
            //}

        }//求实际价格的函数
         /// <summary>
         /// 获取宝贝的交易数量
         /// </summary>
         /// <param name="uri"></param>
         /// <returns></returns>
        Trading_Quantity Trading(string uri, int timeout)
        {
            string gouri =
                $"https://detailskip.taobao.com/service/getData/1/p1/item/detail/sib.htm?itemId={Regex.Match(uri, @"(?<=id=)(?<id>\d+)(?=&)").Groups["id"].Value}&modules=sellerDetail,soldQuantity&callback=onSibRequestSuccess";

            myhttpweb.Timeout = timeout;
            myhttpweb.Referer = uri.Replace("#detail", string.Empty);
            string html_price = myhttpweb.PostOrGet(gouri, HttpMethod.GET).HtmlValue;

            var soldTotalCount = Regex.Match(html_price, "(?<=soldTotalCount\":)(?<soldTotalCount>\\d+)").Groups["soldTotalCount"].Value;
            var confirmGoodsCount = Regex.Match(html_price, "(?<=confirmGoodsCount\":)(?<confirmGoodsCount>\\d+)").Groups["confirmGoodsCount"].Value;
            return new Trading_Quantity()
            {
                Quantity = int.Parse(string.IsNullOrWhiteSpace(soldTotalCount) ? "-1" : soldTotalCount),
                ConfirmGoodsItems = int.Parse(string.IsNullOrWhiteSpace(confirmGoodsCount) ? "-1" : confirmGoodsCount),
                PaySuccess = int.Parse(string.IsNullOrWhiteSpace(confirmGoodsCount) ? "-1" : confirmGoodsCount)
            };
            //Trading_Quantity newtrading = new Trading_Quantity();
            ////string getstring = myhttpweb.PostOrGet(uri, HttpMethod.GET)[1];
            //string gouri = "https:" + Cf_String.ExtractStringNoQH(getstring, "apiItemInfo\":\"", "\"")[0];
            //gouri += "&ref=";
            //myhttpweb.Timeout = timeout;
            //myhttpweb.Referer = uri;
            //string html_trading = myhttpweb.PostOrGet(gouri, HttpMethod.GET).HtmlValue;
            //if (html_trading.IndexOf("quanity:") != -1)
            //    newtrading.Quantity = int.Parse(Cf_String.ExtractStringNoQH(html_trading, "  	,quanity:", ",")[0].Trim());
            //else newtrading.Quantity = 0;
            //if (html_trading.IndexOf("confirmGoods:") != -1)
            //    newtrading.ConfirmGoods = int.Parse(Cf_String.ExtractStringNoQH(html_trading, "confirmGoods:", "}")[0].Trim());
            //else newtrading.ConfirmGoods = 0;
            //if (html_trading.IndexOf("paySuccess:") != -1)
            //    newtrading.PaySuccess = int.Parse(Cf_String.ExtractStringNoQH(html_trading, "paySuccess:", ",")[0].Trim());
            //else newtrading.PaySuccess = 0;
            //if (html_trading.IndexOf("paySuccessItems:") != -1)
            //    newtrading.PaySuccessItems = int.Parse(Cf_String.ExtractStringNoQH(html_trading, "paySuccessItems:", ",")[0].Trim());
            //else newtrading.PaySuccessItems = 0;
            //if (html_trading.IndexOf("confirmGoodsItems:") != -1)
            //    newtrading.ConfirmGoodsItems = int.Parse(Cf_String.ExtractStringNoQH(html_trading, "confirmGoodsItems:", ",")[0].Trim());
            //else newtrading.ConfirmGoodsItems = 0;
            //if (html_trading.IndexOf("refundCount:") != -1)
            //    newtrading.RefundCount = int.Parse(Cf_String.ExtractStringNoQH(html_trading, "refundCount:", "}")[0].Trim());
            //else newtrading.RefundCount = 0;
            //return newtrading;
        }//获取宝贝交易数量的对象
        /// <summary>
        /// 获取所有评论的字典，非常耗时
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        BaoBei_comment Comment(string uri, int timeout)
        {
            BaoBei_comment commentcl = new BaoBei_comment();
            commentcl.Comment = new List<string>();
            commentcl.Comment_name = new List<string>();
            string houzui = "&rateType=&orderType=sort_weight&showContent=1";
            //string getstring = myhttpweb.PostOrGet(uri, HttpMethod.GET)[1];
            string gouri = Cf_String.ExtractStringNoQH(getstring, "data-listApi=\"", "\"")[0];
            gouri += "&currentPageNum=";
            myhttpweb.Referer = uri;
            myhttpweb.Timeout = timeout;
            string html_trading = myhttpweb.PostOrGet(gouri + "1" + houzui, HttpMethod.GET).HtmlValue;
            int maxpage = int.Parse(Cf_String.ExtractStringNoQH(html_trading, "maxPage\":", ",")[0]);
            commentcl.Maxpage = maxpage;
            for (int i = 1; i <= maxpage; i++)
            {
                myhttpweb.Referer = uri;
                myhttpweb.Timeout = timeout;
                List<string> name = Cf_String.ExtractStringNoQH(myhttpweb.PostOrGet(gouri + i.ToString() + houzui, HttpMethod.GET).HtmlValue, "nick\":\"", "\"");
                myhttpweb.Timeout = timeout;
                List<string> comment = Cf_String.ExtractStringNoQH(myhttpweb.PostOrGet(gouri + i.ToString() + houzui, HttpMethod.GET).HtmlValue.Replace("reply\":null,\"append\":{\"content\":\"", "").Replace("appendList\":[{\"content", ""), "content\":\"", "\"");
                if (name.Count == comment.Count)
                {
                    for (int i_1 = 0; i_1 < name.Count; i_1++)
                    {
                        commentcl.Comment_name.Add(name[i_1]);
                        commentcl.Comment.Add(comment[i_1]);
                    }
                }
            }
            return commentcl;
        }//获得评论的对象
    }
}
