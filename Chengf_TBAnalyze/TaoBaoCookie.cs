using Chengf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Chengf_BaoBeiAnalyze
{
    public class TaoBaoCookie
    {
        Cf_HttpWeb myhttpweb = new Cf_HttpWeb();
        public CookieContainer Cookie { set; get; }
        public TaoBaoCookie()
        {
            Cookie = (CookieContainer)myhttpweb.PostOrGet("http://www.taobao.com/", HttpMethod.GET, new CookieContainer())[0];
        }
    }
}
