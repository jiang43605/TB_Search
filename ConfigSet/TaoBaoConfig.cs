using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chengf_BaoBeiAnalyze;
using Chengf_BaoBeiMonitor;
using Chengf_CommodityAnaltze;

namespace ConfigSet
{
    public class TaoBaoConfig
    {
       
        public TaoBaoConfig()
        {
            RkingMode = BaoBei_RkingMode.Sales;
            MonitorDisplayNum = 10;
            DisplayNum = 100;
            OpenMonitor = false;
            MonitorIsUpdating = false;
            MianOpenThreadNum = 1;
        }
        /// <summary>
        /// 主进程中要打开的线程个数
        /// </summary>
        public int MianOpenThreadNum
        {
            set;
            get;
        }
        /// <summary>
        /// 指示监控是否正在进行，默认为否
        /// </summary>
        public bool MonitorIsUpdating
        {
            set;
            get;
        }
        /// <summary>
        /// 是否开启监视，默认不开启监视
        /// </summary>
        public bool OpenMonitor
        {
            set;
            get;
        }
        /// <summary>
        /// 宝贝排名的模式，例如综合、销量等模式，默认销量排名
        /// </summary>
        public BaoBei_RkingMode RkingMode
        {
            set;
            get;
        }
        /// <summary>
        /// 主界面展示的的宝贝数量，默认100
        /// </summary>
        public int DisplayNum
        {
            set;
            get;
        }
        /// <summary>
        /// 监测宝贝的数量，默认10
        /// </summary>
        public int MonitorDisplayNum
        {
            set;
            get;
        }
    }
}
