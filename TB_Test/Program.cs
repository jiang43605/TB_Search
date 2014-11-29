using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Chengf;

namespace TB_Test
{
    class Program
    {
        static Cf_HttpWeb myhttpweb = new Cf_HttpWeb();
        static void Main(string[] args)
        {
            //string uri = "http://apollon.t.taobao.com/market/AllContentByPage.do?resourceIds=20140506001,20140506002,201040506006,20140506003,20140506004,20140506007,20140506008,20140506009,2014050610&t=1417224817821";

            //CookieContainer cookiecon = (CookieContainer)myhttpweb.PostOrGet("http://www.taobao.com/", HttpMethod.GET, new CookieContainer())[0];
            //CookieCollection clo = cookiecon.GetCookies(new Uri("http://www.taobao.com/"));

            //myhttpweb.Referer = "http://www.taobao.com/";
            //CookieContainer al = (CookieContainer)myhttpweb.PostOrGet(uri, HttpMethod.GET, new CookieContainer())[0];
            //CookieCollection clu = al.GetCookies(new Uri("http://www.taobao.com/"));
           

            //myhttpweb.Referer = "http://www.taobao.com/";
            //List<string> str = myhttpweb.PostOrGet(uri, HttpMethod.GET);

            ////cna来源uri="http://pcookie.taobao.com/app.gif?&cna=ZRAADaFzCFsCAW75v0icRY67";
            //myhttpweb.Referer = "http://www.taobao.com/";
            //string cnacookie = myhttpweb.PostOrGet("http://pcookie.taobao.com/app.gif?&cna=ZRAADaFzCFsCAW75v0icRY67", HttpMethod.GET)[0];

            //string zcookie = str[0] + ";" + cnacookie;

            //string html = myhttpweb.PostOrGet("http://detailskip.taobao.com/json/show_buyer_list.htm?step=false&bid_page=1&page_size=15&item_type=b&ends=1417775226000&starts=1417170426000&item_id=40236324569&user_tag=270274562&old_quantity=666666&zhichong=true&sold_total_num=16&seller_num_id=2037166702&dk=0&title=win%BC%A4%BB%EE7%2F8%2F8.1%C6%EC%BD%A2%B0%E6%2F%D7%A8%D2%B5%B0%E6%2F%C6%F3%D2%B5%B0%E6%BC%D2%CD%A5%B8%DF%BC%B6%B0%E632%CE%BB64%CE%BBsp1%C3%DC%D4%BF&sbn=783613c50bdc9dc4d9aa4f67ded6debd&isTKA=false&msc=1&callback=Hub.data.records_reload", HttpMethod.GET, zcookie)[1];

        }
    }
}
