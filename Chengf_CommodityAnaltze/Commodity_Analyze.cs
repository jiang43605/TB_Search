using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chengf;
using Chengf_BaoBeiAnalyze;
using System.Threading;
using System.Net;

namespace Chengf_CommodityAnaltze
{
    /// <summary>
    /// 对搜索后的页面分析
    /// </summary>
    public class Commodity_Analyze
    {
        Cf_HttpWeb myhttpweb = new Cf_HttpWeb();
        bool ishavecontent = true;
        string commodityname;//搜索的名词
        int openThreadNum = 10;
        /// <summary>
        /// 适用整个程序传递Cookie值
        /// </summary>
        public CookieContainer ObjCookieContainer
        {
            set;
            get;
        }
        /// <summary>
        /// 获取数据要开启的线程个数，默认为10
        /// </summary>
        public int OpenThreadNum
        {
            get { return openThreadNum; }
            set { openThreadNum = value; }
        }
        /// <summary>
        /// 搜索的关键词是否有内容
        /// </summary>
        public bool Ishavecontent
        {
            get { return ishavecontent; }
            set { ishavecontent = value; }
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="Commodityname">商品名字</param>
        public Commodity_Analyze(string Commodityname)
        {
            commodityname = Commodityname;
            string uri = "http://s.taobao.com/search?q=" + commodityname + "&commend=all&ssid=s5-e&search_type=item&sourceId=tb.index&spm=1.7274553.1997520841.1&initiative_id=tbindexz_20140711";
            string commodityhtml = myhttpweb.PostOrGet(uri, HttpMethod.GET)[1];
            InitializeCookieContainer();
            //if (commodityhtml.IndexOf("二手") == -1 || commodityhtml.IndexOf("值得买") == -1)
            //{
            //    ishavecontent = false;
            //}
        }
        public void InitializeCookieContainer()
        {
            myhttpweb.Referer = "http://www.taobao.com/";
           CookieContainer cookieContainer = (CookieContainer)myhttpweb.PostOrGet("http://apollon.t.taobao.com/market/AllContentByPage.do?resourceIds=20140506001,20140506002,201040506006,20140506003,20140506004,20140506007,20140506008,20140506009,2014050610&t=1417249350371", HttpMethod.GET, new CookieContainer())[0];
           ObjCookieContainer = cookieContainer;
        }
        /// <summary>
        /// 对传进来的搜索关键词进行分析,调用之前应先判断是否有搜索到的商品
        /// </summary>
        /// <returns></returns>
        public Commodity_Attribute Anlyze()
        {
            string uri = "http://s.taobao.com/search?q=" + commodityname + "&commend=all&ssid=s5-e&search_type=item&sourceId=tb.index&spm=1.7274553.1997520841.1&initiative_id=tbindexz_20140711";
            string commodityhtml = myhttpweb.PostOrGet(uri, HttpMethod.GET)[1];
            Commodity_Attribute newcommodity = new Commodity_Attribute();
            newcommodity.Commodity_name = commodityname;
            newcommodity.Commodity_PageQuantity = Cf_String.ExtractStringNoQH(commodityhtml, "hasmax:true,max:", ",")[0];
            newcommodity.Commodity_PreQuantity = Cf_String.ExtractStringNoQH(commodityhtml, "共 <span class=\"h\">", "</span>")[0];
            return newcommodity;
        }//分析传过来的页面
        /// <summary>
        /// 综合排名下返回指定页面的宝贝
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<string> Commodity_ZPageLink(int page)
        {
            string uri = "http://s.taobao.com/search?q=" + commodityname + "&commend=all&ssid=s5-e&search_type=item&sourceId=tb.index&spm=1.7274553.1997520841.1&initiative_id=tbindexz_20140711&s=";
            uri += (44 * (page - 1)).ToString();
            string pagehtml = myhttpweb.PostOrGet(uri, HttpMethod.GET)[1];
            //if (pagehtml.IndexOf("筛选条件加的太多啦，未找到") != -1) return null;
            List<string> pagelink = Cf_String.ExtractStringNoQH(pagehtml, "{\"nid\":\"", "\"");
            if (pagelink.Count == 0) return null;//表示已经搜索要页的末尾了
            //http://item.taobao.com/item.htm?id=37551039826&ns=1#detail
            for (int i = 0; i < pagelink.Count; i++)
            {
                pagelink[i] = "http://item.taobao.com/item.htm?id=" + pagelink[i] + "&ns=1#detail";
            }
            return pagelink;
        }
        /// <summary>
        /// 综合排名下返回指定数量的宝贝
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<BaoBei_Attribute> Commodity_ZQBaoBei(int count, newdelegate JiDudelegate)
        {
            List<string> allbaobeilink = new List<string>();
            for (int i = 0; i < (count / 44) + 1; i++)
            {
                List<string> list_1 = Commodity_ZPageLink(i + 1);
                if (list_1 == null)
                    break;
                allbaobeilink.AddRange(list_1);//使得加载的页面不会过多
            }
            allbaobeilink.RemoveRange(count, (allbaobeilink.Count - count));
            return ThreadUpdata(allbaobeilink, JiDudelegate, openThreadNum);
        }
        /// <summary>
        /// 销量排名下返回指定页面的宝贝
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<string> Commodity_XPagelink(int page)
        {
            string uri = "http://s.taobao.com/search?promote=0&sort=sale-desc&fs=1&initiative_id=staobaoz_20140712&tab=all&q=" + commodityname + "&stats_click=search_radio_all%253A1&s=";
            uri += (44 * (page - 1)).ToString();
            string pagehtml = myhttpweb.PostOrGet(uri, HttpMethod.GET)[1];
            List<string> pagelink = Cf_String.ExtractStringNoQH(pagehtml, "{\"nid\":\"", "\"");
            if (pagelink.Count == 0) return null;//表示已经搜索要页的末尾了
            //http://item.taobao.com/item.htm?id=37551039826&ns=1#detail
            for (int i = 0; i < pagelink.Count; i++)
            {
                pagelink[i] = "http://item.taobao.com/item.htm?id=" + pagelink[i] + "&ns=1#detail";
            }
            return pagelink;
        }
        /// <summary>
        /// 销量排名下返回指定数量的宝贝
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<BaoBei_Attribute> Commodity_XQBaoBei(int count, newdelegate JiDudelegate)
        {
            List<string> allbaobeilink = new List<string>();
            for (int i = 0; i < (count / 44) + 1; i++)
            {
                List<string> list_1 = Commodity_XPagelink(i + 1);
                if (list_1 == null)
                    break;
                allbaobeilink.AddRange(list_1);//使得加载的页面不会过多
            }
            allbaobeilink.RemoveRange(count, (allbaobeilink.Count - count));
            return ThreadUpdata(allbaobeilink, JiDudelegate, openThreadNum);
        }

        /// <summary>
        /// 获取搜索关键字下综合排名所有宝贝的连接
        /// </summary>
        /// <returns></returns>
        public List<string> Commodity_ZAlllink()
        {
            List<string> alllist = new List<string>();
            for (int i = 1; ; i++)
            {
                object obj = Commodity_ZPageLink(i);
                if (obj != null)
                    alllist.AddRange((List<string>)obj);
                else break;
            }
            return alllist;
        }
        /// <summary>
        /// 获取搜索关键字下销量排名所有宝贝的连接
        /// </summary>
        /// <returns></returns>
        public List<string> Commodity_XAlllink()
        {
            List<string> alllist = new List<string>();
            for (int i = 1; ; i++)
            {
                object obj = Commodity_XPagelink(i);
                if (obj != null)
                    alllist.AddRange((List<string>)obj);
                else break;
            }
            return alllist;
        }
        /// <summary>
        /// 根据综合获得全部排名
        /// </summary>
        /// <returns></returns>
        public List<BaoBei_Attribute> Commodity_Zranking(newdelegate JiDudelegate)
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = 20;//设置最大连接并发数，这是为了防止连接超时
            List<string> allbaobeilink = Commodity_ZAlllink();//获得所有宝贝的连接集合
            return ThreadUpdata(allbaobeilink, JiDudelegate, openThreadNum);
        }
        /// <summary>
        /// 根据销量获得全部排名
        /// </summary>
        /// <returns></returns>
        public List<BaoBei_Attribute> Commodity_Xranking(newdelegate JiDudelegate)
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = 20;//设置最大连接并发数，这是为了防止连接超时
            List<string> allbaobeilink = Commodity_XAlllink();//获得所有宝贝的连接集合   
            return ThreadUpdata(allbaobeilink, JiDudelegate, openThreadNum);
        } 
        /// <summary>
        /// 开启线程刷新或请求宝贝的属性，可自定义线程开启数目
        /// </summary>
        /// <param name="allbaobeilink">要更新数据的集合包</param>
        /// <param name="JiDudelegate">界面显示代码</param>
        /// <param name="threadnum">要开启加速的线程数</param>
        /// <returns></returns>
        public List<BaoBei_Attribute> ThreadUpdata(List<string> allbaobeilink, newdelegate JiDudelegate, int threadnum)
        {
            BaoBei_Attribute[] fanghui = new BaoBei_Attribute[allbaobeilink.Count];//定义该数组为了纠正因为List导致的排名错误
            List<BaoBei_Attribute> baobeilist = new List<BaoBei_Attribute>();//获得所有宝贝属性
            int cancontinue = 0;
            #region 按照给定的线程数进行加速
            for (int i = 0; i < threadnum; i++)
            {
                if (allbaobeilink.Count >= i + 1)
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback((a) =>
                    {
                        int k = (int)a;
                        for (int j = k; j < allbaobeilink.Count; j += threadnum)
                        {
                        gt1: try
                            {
                                BaoBei_Attribute blat = new BaoBei_analyze().Analyze(allbaobeilink[j], 10000, ObjCookieContainer);//ObjCookieContainer用来传递Cookie值
                                System.GC.Collect();
                                if (blat != null)
                                {
                                    fanghui[j] = blat;
                                    if (JiDudelegate != null)
                                        JiDudelegate();
                                }
                            }
                            catch (Exception)
                            {
                                goto gt1;
                            }
                        }
                        cancontinue++;
                    }), i);
                }
            }
            #endregion
            while (cancontinue != threadnum)
            {
                Thread.Sleep(5000);
            }
            System.GC.Collect();
            List<BaoBei_Attribute> baobeilist_copy = fanghui.ToList();
            for (int i = 0; i < baobeilist_copy.Count; i++)
            {
                if (baobeilist_copy[i] != null) { baobeilist.Add(baobeilist_copy[i]); }
            }
            for (int i = 0; i < baobeilist.Count; i++)//提供排名的功能
            {
                int howrking;
                if ((i + 1) % 4 == 0)
                    howrking = ((i + 1) / 4);
                else
                    howrking = ((i + 1) / 4) + 1;
                baobeilist[i].Baobei_Rkingname = (i + 1).ToString() + "（" + "第" + howrking.ToString() + "排" + "）";
            }
            return baobeilist;
        }
        /// <summary>
        /// 开启线程刷新或请求宝贝的属性，静态的方法
        /// </summary>
        /// <param name="allbaobeilink"></param>
        /// <param name="JiDudelegate"></param>
        /// <returns></returns>
        public static List<BaoBei_Attribute> ThreadUpdataStatic(List<string> allbaobeilink, newdelegate JiDudelegate, int threadnum)
        {
            #region 初始化一个CookieContainer
            Cf_HttpWeb staticmyhttpweb = new Cf_HttpWeb();
            staticmyhttpweb.Referer = "http://www.taobao.com/";
            CookieContainer cookieContainer = (CookieContainer)staticmyhttpweb.PostOrGet("http://apollon.t.taobao.com/market/AllContentByPage.do?resourceIds=20140506001,20140506002,201040506006,20140506003,20140506004,20140506007,20140506008,20140506009,2014050610&t=1417249350371", HttpMethod.GET, new CookieContainer())[0]; 
            #endregion
            BaoBei_Attribute[] fanghui = new BaoBei_Attribute[allbaobeilink.Count];//定义该数组为了纠正因为List导致的排名错误
            List<BaoBei_Attribute> baobeilist = new List<BaoBei_Attribute>();//获得所有宝贝属性
            int cancontinue = 0;
            #region 按照给定的线程数进行加速
            for (int i = 0; i < threadnum; i++)
            {
                if (allbaobeilink.Count >= i + 1)
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback((a) =>
                    {
                        int k = (int)a;
                        for (int j = k; j < allbaobeilink.Count; j += threadnum)
                        {
                        gt1: try
                            {
                                BaoBei_Attribute blat = new BaoBei_analyze().Analyze(allbaobeilink[j], 10000, cookieContainer);
                                System.GC.Collect();
                                if (blat != null)
                                {
                                    fanghui[j] = blat;
                                    if (JiDudelegate != null)
                                        JiDudelegate();
                                }
                            }
                            catch (Exception)
                            {
                                goto gt1;
                            }
                        }
                        cancontinue++;
                    }), i);
                }
            }
            #endregion
            while (cancontinue != threadnum)
            {
                Thread.Sleep(5000);
            }
            System.GC.Collect();
            List<BaoBei_Attribute> baobeilist_copy = fanghui.ToList();
            for (int i = 0; i < baobeilist_copy.Count; i++)
            {
                if (baobeilist_copy[i] != null) { baobeilist.Add(baobeilist_copy[i]); }
            }
            for (int i = 0; i < baobeilist.Count; i++)//提供排名的功能
            {
                int howrking;
                if ((i + 1) % 4 == 0)
                    howrking = ((i + 1) / 4);
                else
                    howrking = ((i + 1) / 4) + 1;
                baobeilist[i].Baobei_Rkingname = (i + 1).ToString() + "（" + "第" + howrking.ToString() + "排" + "）";
            }
            return baobeilist;
        }
    }
}
