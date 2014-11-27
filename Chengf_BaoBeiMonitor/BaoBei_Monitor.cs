using Chengf_BaoBeiAnalyze;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chengf_BaoBeiMonitor
{
    /// <summary>
    /// 淘宝宝贝监测
    /// </summary>
    public class TB_BaoBei_Monitor : IMonitorable
    {
        public Chengf_CommodityAnaltze.newdelegate JiDudelegate
        {
            set;
            get;
        }
        /// <summary>
        /// 要监测的宝贝数量
        /// </summary>
        public int Monitornum
        {
            set;
            get;
        }
        public TB_BaoBei_Monitor(int monitornum)
        {
                Monitornum = monitornum;
        }
        /// <summary>
        /// 实现淘宝的数据更新，返回的是List<BaoBei_Attribute>类型
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="JiDudelegate"></param>
        /// <returns></returns>
        public object BaoBeiUpdata(object obj)
        {
            List<BaoBei_Attribute> bbat_copy = (List<BaoBei_Attribute>)obj;
            List<string> allbaobeilink = new List<string>();
            if(bbat_copy.Count>=Monitornum)//此处if用于判断实际的宝贝数量是否小于要监测的数量，如果小于的话就监测全部的宝贝
            {
                for (int i = 0; i < Monitornum; i++)
                {
                    allbaobeilink.Add(bbat_copy[i].Baobei_link);
                }
            }
            else
            {
                for (int i = 0; i < bbat_copy.Count; i++)
                {
                    allbaobeilink.Add(bbat_copy[i].Baobei_link);
                }
            }
            List<BaoBei_Attribute> baobeilist = Chengf_CommodityAnaltze.Commodity_Analyze.ThreadUpdataStatic(allbaobeilink, JiDudelegate,5);
            for (int i = Monitornum; i < bbat_copy.Count; i++)
            {
                baobeilist.Add(bbat_copy[i]);
            }
            return baobeilist;
        }
    }
}
