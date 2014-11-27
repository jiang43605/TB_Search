using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chengf_BaoBeiAnalyze
{
    /// <summary>
    /// 宝贝排名模式
    /// </summary>
    public enum BaoBei_RkingMode
    {
        
        /// <summary>
        /// 综合排名模式
        /// </summary>
        Composite,
        /// <summary>
        /// 销量排名模式
        /// </summary>
        Sales,
        /// <summary>
        /// 指定数量的综合排名模式
        /// </summary>
        AppointCompsite,
        /// <summary>
        /// 指定数量的销量排名模式
        /// </summary>
        AppointSomeSales
    } 
}
