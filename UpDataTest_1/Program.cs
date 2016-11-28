using Chengf_BaoBeiAnalyze;
using Chengf_BaoBeiMonitor;
using Chengf_CommodityAnaltze;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chengf;
using System.Threading;

namespace UpDataTest_1
{
    class Program
    {
        static void Main(string[] args)
        {

            BaoBei_analyze ba = new BaoBei_analyze();
            Cf_HttpWeb web = new Cf_HttpWeb();
            web.CPostOrGet("https://item.taobao.com/item.htm?id=520704235964&ns=1&abbucket=16#detail", HttpMethod.GET);
            BaoBei_Attribute bat = ba.Analyze("https://item.taobao.com/item.htm?id=520704235964&ns=1&abbucket=16#detail", 10000, web.HttpCookieContainer,DateTime.Now);
            Console.ReadKey();

            ////BaoBei_analyze oi = new BaoBei_analyze();
            ////BaoBei_Attribute attr = oi.Analyze("http://item.taobao.com/item.htm?spm=a230r.1.14.122.gJBUTb&id=40014412300&ns=1#detail", 10000);
            //Chengf.Cf_HttpWeb web = new Chengf.Cf_HttpWeb();
            //if (web.PostOrGet("http://item.taobao.com/item.htm?spm=a230r.1.14.15.3aSpkc&id=40421416981&ns=1#detail", Chengf.HttpMethod.GET)[1].IndexOf("此宝贝已下架") != -1)
            //    Console.WriteLine("宝贝已经下架啦");
            //else Console.WriteLine("宝贝未下架");
            //Console.ReadKey();

            //var intm = 98;
            //if (intm / 60 == 0) Console.WriteLine(intm.ToString() + "秒");
            //else if (intm / 60 != 0 && intm / 3600 == 0) Console.WriteLine((intm / 60).ToString() + "分" + (intm % 60).ToString() + "秒");
            //else Console.WriteLine((intm / 3600).ToString() + "小时" + (intm % 3600).ToString() + "分" + (intm % 60).ToString() + "秒");
            //Console.ReadKey();


            //            BaoBei_Attribute oi = new BaoBei_Attribute();
            //            oi.Baobei_manager = "hehe";
            //            oi.Baobei_price = 100;


            //            List<BaoBei_Attribute> ou = new List<BaoBei_Attribute>();
            //            ou.Add(oi);

            //;


            //            if( ou.IndexOf(oi)!=-1)
            //                Console.WriteLine("找到了");
            //            else Console.WriteLine("没找到");
            //            Console.ReadKey();

            //string uri = "http://item.taobao.com/item.htm?spm=a217o.1220302.1998078166.1.b8mkOI&scm=1007.10481.1368.100200300000000&pvid=80217a22-1d1d-4701-8da0-16326f414491&id=40583404304";
            //Chengf.Cf_HttpWeb newhttpweb = new Chengf.Cf_HttpWeb();
            //string html = newhttpweb.PostOrGet(uri, Chengf.HttpMethod.GET)[1];
            //string tradrecordh_tml = Cf_String.ExtractStringNoQH(html, "data-api=\"", "\"")[0];
            //tradrecordh_tml += "&callback=Hub.data.records_reload";
            //string record_html = newhttpweb.PostOrGet(tradrecordh_tml, Chengf.HttpMethod.GET)[1];
            //List<string> buyname_list = Cf_String.ExtractStringNoQH(record_html, "tb-sellnick\\\">", "</span> <");
            //for (int i = 0; i < buyname_list.Count; i++)
            //{
            //    buyname_list[i] = Cf_String.DeleteSpecificString(buyname_list[i], "<", ">");
            //}
            //List<string> buyprice_list = Cf_String.ExtractStringNoQH(record_html, "tb-rmb-num\\\">", "</em>");
            //List<string> buynum_list = Cf_String.ExtractStringNoQH(record_html, "<td class=\\\"tb-amount\\\">", "</td>");
            //List<string> buytime_time = Cf_String.ExtractStringNoQH(record_html, "<td class=\\\"tb-start\\\">", "</td>");
            //DateTime iu = DateTime.Parse("2014-08-31 15:25:30");
            //string uy = iu.ToString();
            //int u = iu.Month;

            //BaoBei_TradeRecord newrecord = new BaoBei_TradeRecord();
            //newrecord = BaoBei_TradeRecord.GetTradeRecord("http://item.taobao.com/item.htm?spm=a230r.1.14.92.dy8cEP&id=40158964828&ns=1#detail", DateTime.Now);
            //Console.ReadKey();


            //DateTime datetime = DateTime.Now;
            //BaoBei_TradeRecord traceRecord = new BaoBei_TradeRecord();
            //Chengf.Cf_HttpWeb newhttpweb = new Chengf.Cf_HttpWeb();
            //int endpoint = 0;
            //string html = newhttpweb.PostOrGet("http://item.taobao.com/item.htm?spm=a230r.1.14.1.RjT9WZ&id=37109010718&ns=1#detail", Chengf.HttpMethod.GET)[1];
            //string tradrecordh_html = Cf_String.ExtractStringNoQH(html, "data-api=\"", "\"")[0];
            //tradrecordh_html += "&callback=Hub.data.records_reload";
            //string recordhtml = "";
            //string strreplace = "bid_page=1";//为下面循环进行数据暂存
            //BaoBei_TradeRecord endrecordlist = new BaoBei_TradeRecord();//存储末尾的交易记录
            //for (int i = 1; ; i += 1)
            //{
            //    try
            //    {
            //        string pagenum = "bid_page=" + i.ToString();
            //        tradrecordh_html = tradrecordh_html.Replace(strreplace, pagenum);
            //        strreplace = pagenum;
            //        string recordhtml_copy = newhttpweb.PostOrGet(tradrecordh_html, HttpMethod.GET)[1];
            //        if (!IsDateEnd(HtmlToTradeRecord(recordhtml_copy).BuyTime, datetime, out endpoint))
            //        {
            //            endrecordlist = HtmlToTradeRecord(recordhtml_copy);
            //            break;
            //        }
            //        recordhtml += recordhtml_copy;
            //        Thread.Sleep(500);
            //    }
            //    catch (Exception)
            //    {
            //        throw;
            //    }
            //    Console.WriteLine("已经抓取到第" + i + "页");
            //}
            //BaoBei_TradeRecord totalrecord = new BaoBei_TradeRecord();
            //totalrecord = HtmlToTradeRecord(recordhtml);
            ////将末尾记录和全段记录合并
            //for (int i = 0; i < endpoint; i++)
            //{
            //    totalrecord.BuyerName.Add(endrecordlist.BuyerName[i]);
            //    totalrecord.BuyNum.Add(endrecordlist.BuyNum[i]);
            //    totalrecord.BuyPrice.Add(endrecordlist.BuyPrice[i]);
            //    totalrecord.BuyTime.Add(endrecordlist.BuyTime[i]);
            //}
        }
        /// <summary>
        /// 通过URI解析特定网页的内容，返回一个解析的对象
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        //private static BaoBei_TradeRecord UriToTradeRecord(string uri)
        //{
        //    BaoBei_TradeRecord tradrecord = new BaoBei_TradeRecord();
        //    Chengf.Cf_HttpWeb newhttpweb = new Chengf.Cf_HttpWeb();
        //    string record_html = newhttpweb.PostOrGet(uri, Chengf.HttpMethod.GET)[1];
        //    return HtmlToTradeRecord(record_html);
        //}
        ///// <summary>
        ///// 解析特定网页的内容，返回一个解析的对象
        ///// </summary>
        ///// <param name="html"></param>
        ///// <returns></returns>
        //private static BaoBei_TradeRecord HtmlToTradeRecord(string html)
        //{
        //    BaoBei_TradeRecord tradrecord = new BaoBei_TradeRecord();
        //    #region 得到买家的名字
        //    List<string> buyname_list = Cf_String.ExtractStringNoQH(html, "tb-sellnick\\\">", "</span> <");
        //    for (int i = 0; i < buyname_list.Count; i++)
        //    {
        //        buyname_list[i] = Cf_String.DeleteSpecificString(buyname_list[i], "<", ">");
        //    }
        //    #endregion
        //    tradrecord.BuyerName = buyname_list;
        //    #region 得到购买的价格
        //    List<string> buyprice_strlist = Cf_String.ExtractStringNoQH(html, "tb-rmb-num\\\">", "</em>");
        //    List<double> buyprice_doblist = new List<double>();
        //    foreach (var item in buyprice_strlist)
        //    {
        //        buyprice_doblist.Add(double.Parse(item));
        //    }
        //    #endregion
        //    tradrecord.BuyPrice = buyprice_doblist;
        //    #region 得到购买的数量
        //    List<string> buynum_strlist = Cf_String.ExtractStringNoQH(html, "<td class=\\\"tb-amount\\\">", "</td>");
        //    List<int> buynum_intlist = new List<int>();
        //    foreach (var item in buynum_strlist)
        //    {
        //        buynum_intlist.Add(int.Parse(item));
        //    }
        //    #endregion
        //    tradrecord.BuyNum = buynum_intlist;
        //    #region 得到购买的时间
        //    List<string> buytime_strtime = Cf_String.ExtractStringNoQH(html, "<td class=\\\"tb-start\\\">", "</td>");
        //    List<DateTime> buytime_datertime = new List<DateTime>();
        //    foreach (var item in buytime_strtime)
        //    {
        //        buytime_datertime.Add(DateTime.Parse(item));
        //    }
        //    #endregion
        //    tradrecord.BuyTime = buytime_datertime;
        //    return tradrecord;
        //}
        ///// <summary>
        ///// 判断时间点是否吻合
        ///// </summary>
        ///// <param name="datetime"></param>
        ///// <param name="datebool"></param>
        ///// <param name="newdatetime"></param>
        ///// <returns></returns>
        //private static bool IsDateEnd(List<DateTime> datetime, DateTime datebool, out int newdatetime)
        //{
        //    bool returnbool = true;
        //    newdatetime = 0;
        //    for (int i = 0; i < datetime.Count; i++)
        //    {
        //        if (datetime[i].ToShortDateString() != datebool.ToShortDateString())
        //        {
        //            newdatetime = i;
        //            return false;
        //        }
        //    }
        //    return returnbool;
        //}
        //private List<BaoBei_Attribute> ThreadUpdata(List<string> allbaobeilink, newdelegate JiDudelegate, int threadnum)
        //{
        //    BaoBei_Attribute[] fanghui = new BaoBei_Attribute[allbaobeilink.Count];//定义该数组为了纠正因为List导致的排名错误
        //    List<BaoBei_Attribute> baobeilist = new List<BaoBei_Attribute>();//获得所有宝贝属性
        //    bool bl1 = false, bl2 = false, bl3 = false, bl4 = false;
        //    bool[] isthreadend = new bool[threadnum];
        //    for (int i = 0; i < threadnum; i++)
        //    {
        //        if (allbaobeilink.Count >= i + 1)
        //        {
        //            ThreadPool.QueueUserWorkItem(new WaitCallback((a) =>
        //            {
        //                for (int j = i; j < allbaobeilink.Count; j += i)
        //                {
        //                gt1: try
        //                    {
        //                        BaoBei_Attribute blat = new BaoBei_analyze().Analyze(allbaobeilink[j], 10000);
        //                        System.GC.Collect();
        //                        if (blat != null)
        //                        {
        //                            fanghui[i] = blat;
        //                            if (JiDudelegate != null)
        //                                JiDudelegate();
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        goto gt1;
        //                    }
        //                }
        //                isthreadend[i] = true;
        //            }));
        //        }
        //    }
        //    #region 开启4个线程加速
        //    if (allbaobeilink.Count >= 1)
        //    {
        //        ThreadPool.QueueUserWorkItem((a) =>
        //        {

        //            for (int i = 0; i < allbaobeilink.Count; i += 4)
        //            {
        //            gt1: try
        //                {
        //                    BaoBei_Attribute blat = new BaoBei_analyze().Analyze(allbaobeilink[i], 10000);
        //                    System.GC.Collect();
        //                    if (blat != null)
        //                    {
        //                        fanghui[i] = blat;
        //                        if (JiDudelegate != null)
        //                            JiDudelegate();
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    goto gt1;
        //                }
        //            }
        //            bl1 = true;
        //        });
        //    }
        //    if (allbaobeilink.Count >= 2)
        //    {
        //        ThreadPool.QueueUserWorkItem((a) =>
        //        {

        //            for (int i = 1; i < allbaobeilink.Count; i += 4)
        //            {
        //            gt2: try
        //                {
        //                    BaoBei_Attribute blat = new BaoBei_analyze().Analyze(allbaobeilink[i], 10000);
        //                    System.GC.Collect();
        //                    if (blat != null)
        //                    {

        //                        fanghui[i] = blat;
        //                        if (JiDudelegate != null)
        //                            JiDudelegate();
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    goto gt2;
        //                }
        //            }
        //            bl2 = true;
        //        });
        //    }
        //    if (allbaobeilink.Count >= 3)
        //    {
        //        ThreadPool.QueueUserWorkItem((a) =>
        //        {

        //            for (int i = 2; i < allbaobeilink.Count; i += 4)
        //            {
        //            gt3: try
        //                {

        //                    BaoBei_Attribute blat = new BaoBei_analyze().Analyze(allbaobeilink[i], 10000);
        //                    System.GC.Collect();
        //                    if (blat != null)
        //                    {

        //                        fanghui[i] = blat;
        //                        if (JiDudelegate != null)
        //                            JiDudelegate();
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    goto gt3;
        //                }
        //            }
        //            bl3 = true;
        //        });
        //    }
        //    if (allbaobeilink.Count >= 4)
        //    {
        //        ThreadPool.QueueUserWorkItem((a) =>
        //        {

        //            for (int i = 3; i < allbaobeilink.Count; i += 4)
        //            {
        //            gt4: try
        //                {

        //                    BaoBei_Attribute blat = new BaoBei_analyze().Analyze(allbaobeilink[i], 10000);
        //                    System.GC.Collect();
        //                    if (blat != null)
        //                    {

        //                        fanghui[i] = blat;
        //                        if (JiDudelegate != null)
        //                            JiDudelegate();
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    goto gt4;
        //                }
        //            }
        //            bl4 = true;
        //        });
        //    }
        //    #endregion
        //    while (!bl1 || !bl2 || !bl3 || !bl4)
        //    {
        //        Thread.Sleep(5000);
        //    }
        //    List<BaoBei_Attribute> baobeilist_copy = fanghui.ToList();
        //    for (int i = 0; i < baobeilist_copy.Count; i++)
        //    {
        //        if (baobeilist_copy[i] != null) { baobeilist.Add(baobeilist_copy[i]); }
        //    }
        //    for (int i = 0; i < baobeilist.Count; i++)//提供排名的功能
        //    {
        //        int howrking;
        //        if ((i + 1) % 4 == 0)
        //            howrking = ((i + 1) / 4);
        //        else
        //            howrking = ((i + 1) / 4) + 1;
        //        baobeilist[i].Baobei_Rkingname = (i + 1).ToString() + "（" + "第" + howrking.ToString() + "排" + "）";
        //    }
        //    return baobeilist;
        //}
    }
}
