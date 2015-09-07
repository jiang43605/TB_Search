/*用于删除本地老版本*/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeletOldVersion
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.Sleep(2000);
            string[] deletfileall = Directory.GetFiles(Directory.GetCurrentDirectory());
            for (int i = 0; i < deletfileall.Length; i++)
            {
                if (Path.GetFileName(deletfileall[i]) == "更新包.rar" || Path.GetExtension(deletfileall[i])==".exe")
                    continue;
                File.Delete(deletfileall[i]);
            }
        }
    }
}
