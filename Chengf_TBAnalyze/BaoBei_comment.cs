using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chengf_BaoBeiAnalyze
{
   public class BaoBei_comment
    {
        /// <summary>
        /// 评论人的名字
        /// </summary>
        public List<string> Comment_name
        {
            set;
            get;
        }
        /// <summary>
        /// 评语,与评论的人相对应
        /// </summary>
        public List<string> Comment
        {
            set;
            get;
        }
        /// <summary>
        /// 评论总的页数
        /// </summary>
        public int Maxpage
        {
            set;
            get;
        }
    }
}
