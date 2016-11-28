using Chengf_BaoBeiAnalyze;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chengf_BaoBeiMonitor
{
    public interface IMonitorable
    {
        object BaoBeiUpdata(object obj,DateTime searchDateTime);
    }
}
