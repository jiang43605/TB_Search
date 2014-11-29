using Chengf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TB_Test
{
    public class TaoBaoCookie
    {
        static Cf_HttpWeb myhttpweb = new Cf_HttpWeb();
        public CookieContainer Cookie { set; get; }
        public TaoBaoCookie()
        {

        }
        public ArrayList TaoBaoOrignalCookie()
        {

            ArrayList al = myhttpweb.PostOrGet("http://www.taobao.com/", HttpMethod.GET, new CookieContainer());
            return al;
        }
    }
}
