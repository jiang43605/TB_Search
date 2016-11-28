using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Chengf;
using Chengf_BaoBeiAnalyze;
using System.Threading;
using Chengf_CommodityAnaltze;
using Chengf_BaoBeiMonitor;
using System.IO;
using ConfigSet;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace TaoBao_JiHuo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        #region 变量定义
        Cf_HttpWeb myhttpweb = new Cf_HttpWeb();
        Commodity_Analyze newcommdityA;
        List<BaoBei_Attribute> bat;
        System.Timers.Timer timer = new System.Timers.Timer(5000);
        delegate void newdelegate();
        List<BaoBei_Attribute> originalbat = new List<BaoBei_Attribute>();
        int searchnum;//搜索得到的宝贝数量
        int updataint = 0;
        Chengf_BaoBeiMonitor.TB_BaoBei_Monitor Tbmonitor = new TB_BaoBei_Monitor(20);
        double ProgressInt = 0;//用于配合进度条显示
        TaoBaoConfig ParameterSet = new TaoBaoConfig();
        DataTemporarySave[] DataSaveList;
        int istimeupdata = 0;
        string tbsuosou_text;//搜索关键词的全局变量
        System.Timers.Timer Timer_BanbenUpdata = new System.Timers.Timer(60000);
        Socket socketconnection = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint Point = new IPEndPoint(IPAddress.Parse("10.93.4.126"), 8889);
        string updatafilename;
        bool canupdata = false;//判断监控更新后是否能再次开始 
        private DateTime searchDateTime = DateTime.Now;
        #endregion
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Timer_BanbenUpdata.Elapsed += Timer_BanbenUpdata_Elapsed;

            //try
            //{
            //    socketconnection.Connect(Point);
            //}
            //catch (Exception)
            //{

            //    LbSever.Content = "查找服务器失败，现处于离线模式";
            //    Timer_BanbenUpdata.Start();
            //    return;
            //}
            //LbSever.Content = "已经成功连接到服务器";
            //ThreadPool.QueueUserWorkItem(new WaitCallback(SocketRecive), socketconnection);
        }

        void Timer_BanbenUpdata_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (socketconnection.Poll(10000, SelectMode.SelectError))
            {
                try
                {
                    socketconnection.Connect(Point);
                }
                catch (Exception)
                {
                    return;
                }
            }
            else
            {
                LbSever.Dispatcher.BeginInvoke(new newdelegate(() =>
                {
                    LbSever.Content = "已经成功连接到服务器";
                }), null);
                SocketRecive(socketconnection);
                Timer_BanbenUpdata.Stop();
            }

        }
        void SocketRecive(object obj)
        {
            Socket socket = (Socket)obj;
            while (true)
            {
                byte[] buff = new byte[1024 * 1024 * 30];
                int reallength = socket.Receive(buff, 0, buff.Length, 0);
                string deencodingstr = Encoding.Default.GetString(buff, 0, reallength);
                updatafilename = deencodingstr.Split(new char[] { '|' })[0];
                byte[] filebyte = new byte[reallength];
                Buffer.BlockCopy(buff, Encoding.Default.GetBytes(updatafilename).Length, filebyte, 0, filebyte.Length);
                File.WriteAllBytes(updatafilename, filebyte);
                //File.AppendAllText("UpdataLog.xml",updatafilename);
                MessageBox.Show("软件有新的版本，更新文件已经发送到本地！请解压运行", "版本更新提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                Lbbegintime.Dispatcher.BeginInvoke(new newdelegate(() =>
                {
                    Process.Start("DeletOldVersion.exe");
                    this.Close();
                }), null);
            }
        }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            #region 实现显示宝贝数量和监控数量的赋值
            if ((bool)CbLimitXianShiSize.IsChecked)
            {
                if ((bool)RbZ.IsChecked)
                {
                    ParameterSet.RkingMode = BaoBei_RkingMode.AppointCompsite;
                }
                else
                {
                    ParameterSet.RkingMode = BaoBei_RkingMode.AppointSomeSales;
                }
                searchnum = int.Parse(TbXianShiSize.Text);
            }
            if ((bool)Cbopenmonitor.IsChecked) ParameterSet.OpenMonitor = true;
            else ParameterSet.OpenMonitor = false;
            #endregion
            #region 再次开启搜索时的处理办法
            canupdata = false;//禁止更新程序的运行
            while (ParameterSet.MonitorIsUpdating) Thread.Sleep(2000);//更新程序正在运行，主程序开始等待 
            #endregion

            BtSearch.IsEnabled = false;
            BtSearch.Content = "0%";
            ProgressInt = 0;
            tbsuosou_text = Tbsuosou.Text;
            if (DataSaveList != null)
                DataSaveList = null;
            bool CbLimitXianShiSize_Ischecked = (bool)CbLimitXianShiSize.IsChecked;
            ThreadPool.QueueUserWorkItem((a) =>
            {

                newcommdityA = new Commodity_Analyze(tbsuosou_text);
                newcommdityA.OpenThreadNum = ParameterSet.MianOpenThreadNum;
                if (newcommdityA.Ishavecontent)
                {
                    if (CbLimitXianShiSize_Ischecked) BtSearch.Dispatcher.BeginInvoke(new newdelegate(() => { PbSearch.Maximum = searchnum; }), System.Windows.Threading.DispatcherPriority.Normal);
                    else
                    {
                        searchnum = newcommdityA.Commodity_XAlllink().Count;
                        BtSearch.Dispatcher.BeginInvoke(new newdelegate(() => { PbSearch.Maximum = searchnum; }), System.Windows.Threading.DispatcherPriority.Normal);
                    }
                    System.Net.ServicePointManager.DefaultConnectionLimit = 50;
                    if (ParameterSet.RkingMode == BaoBei_RkingMode.Sales) bat = newcommdityA.Commodity_Xranking(ProgressUI, searchDateTime);
                    else if (ParameterSet.RkingMode == BaoBei_RkingMode.Composite) bat = newcommdityA.Commodity_Zranking(ProgressUI, searchDateTime);
                    else if (ParameterSet.RkingMode == BaoBei_RkingMode.AppointSomeSales) bat = newcommdityA.Commodity_XQBaoBei(searchnum, ProgressUI, searchDateTime);
                    else if (ParameterSet.RkingMode == BaoBei_RkingMode.AppointCompsite) bat = newcommdityA.Commodity_ZQBaoBei(searchnum, ProgressUI, searchDateTime);
                    else { MessageBox.Show("模式选取发生错误，请关闭重启"); return; }
                    DataSaveList = new DataTemporarySave[bat.Count];
                    for (int i = 0; i < bat.Count; i++)
                    {
                        DataSaveList[i] = new DataTemporarySave();
                        DataSaveList[i].VolumeSave = bat[i].Baobei_trading.Quantity;
                        DataSaveList[i].UID_uri = bat[i].Baobei_link;//设定datasavelist的标识符
                        DataSaveList[i].PriceSave = bat[i].Baobei_price.ToString();
                    }
                    foreach (var item in bat)
                    {
                        originalbat.Add(item);
                    }
                    dg1.Dispatcher.BeginInvoke(new newdelegate(() =>
                    {
                        #region 统计总的销售额和人流量
                        int pruchpepoleint = 0;
                        double totaltradeprice = 0;
                        for (int i = 0; i < bat.Count; i++)
                        {
                            pruchpepoleint += bat[i].Baobei_TimeOfPeopleNum;
                            totaltradeprice += bat[i].Baobei_TimeOfPrice;
                        }
                        LbTotalPeople.Content = "预计人流量：" + pruchpepoleint.ToString() + "人，截至目前该版块销售额：" + totaltradeprice + "元";
                        #endregion
                        PbSearch.Value = searchnum;
                        dg1.ItemsSource = bat;
                        List<BaoBei_Attribute> bat_copy = new List<BaoBei_Attribute>();
                        if (bat.Count < 20)
                        {
                            for (int i = 0; i < bat.Count; i++)
                            {
                                bat_copy.Add(bat[i]);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < 20; i++)
                            {
                                bat_copy.Add(bat[i]);
                            }
                        }
                        dgmonitor.ItemsSource = MonitorDataManage.DataManage(bat_copy, bat_copy, ref DataSaveList);
                        PbSearch.Value = 0;
                        BtSearch.Content = "搜索";
                        BtSearch.IsEnabled = true;
                        Lbbegintime.Content = "开始监测时间：" + DateTime.Now.ToString();
                    }), System.Windows.Threading.DispatcherPriority.Normal);
                    newcommdityA = null;
                }
                else
                {
                    MessageBox.Show("未搜索到该宝贝的信息");
                    BtSearch.Dispatcher.BeginInvoke(new newdelegate(() => { BtSearch.IsEnabled = true; BtSearch.Content = "搜索"; }), null);
                    return;
                }
                canupdata = true;
                timer.Elapsed += timer_Elapsed;
                if (ParameterSet.OpenMonitor)
                    timer.Start();
            }, null);
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Stop();
            ParameterSet.MonitorIsUpdating = true;//正在更新，赋值为false
            #region 时间到时将进行全局更新，功能已经开启
            if (istimeupdata < 3600)
            {
                istimeupdata += 10;
            }
            else
            {
                UpdataBat();
                istimeupdata = 0;
                timer.Start();
                return;
            }
            #endregion
            updataint++;
            Tbmonitor.JiDudelegate = null;
            List<BaoBei_Attribute> newattribute = (List<BaoBei_Attribute>)Tbmonitor.BaoBeiUpdata(bat, searchDateTime);//基于传入的宝贝属性进行下一次的更新
            MonitorDataManage.OriginalAttribute = originalbat;//将原值赋值
            List<MonitorDataAttribute> newmonitorattribute = MonitorDataManage.DataManage(newattribute, bat, ref DataSaveList);//处理更新一次后的数据迭代
            bat.Clear();
            for (int i = 0; i < newattribute.Count; i++)
            {
                bat.Add(newattribute[i]);
            }
            //置于UI进行显示
            dg1.Dispatcher.BeginInvoke(new newdelegate(() =>
            {
                #region 统计总的销售额和人流量
                int pruchpepoleint = 0;
                double totaltradeprice = 0;
                for (int i = 0; i < newattribute.Count; i++)
                {
                    pruchpepoleint += newattribute[i].Baobei_TimeOfPeopleNum;
                    totaltradeprice += newattribute[i].Baobei_TimeOfPrice;
                }
                LbTotalPeople.Content = "预计人流量：" + pruchpepoleint.ToString() + "人，截至目前该版块销售额：" + totaltradeprice + "元";
                #endregion
                Lbzj.Content = "已经更新了" + updataint.ToString() + "次。\r\n";
                newmonitorattribute.RemoveRange(20, newmonitorattribute.Count - 20);
                dgmonitor.ItemsSource = newmonitorattribute;
            }), System.Windows.Threading.DispatcherPriority.Normal);
            ParameterSet.MonitorIsUpdating = false;//更新完毕，赋值为true
            if (canupdata == false) return;
            else
            {
                ParameterSet.MonitorIsUpdating = true;//又将开始监视
                timer.Start();
            }
        }
        void ProgressUI()
        {
            dg1.Dispatcher.BeginInvoke(new newdelegate(() =>
            {
                PbSearch.Value = ProgressInt;
                BtSearch.Content = (ProgressInt / (Convert.ToDouble(searchnum)) * 100).ToString("0.00") + "%";
            }), System.Windows.Threading.DispatcherPriority.Normal);
            ProgressInt++;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            timer.Dispose();
            Environment.Exit(0);
        }

        private void RbZ_Checked(object sender, RoutedEventArgs e)
        {
            ParameterSet.RkingMode = BaoBei_RkingMode.Composite;
        }

        private void RbX_Checked(object sender, RoutedEventArgs e)
        {
            ParameterSet.RkingMode = BaoBei_RkingMode.Sales;
        }
        private void UpdataBat()
        {
            ProgressInt = 0;
            BtSearch.Dispatcher.BeginInvoke(new newdelegate(() => { BtSearch.IsEnabled = false; }), null);
            newcommdityA = new Commodity_Analyze(tbsuosou_text);
            if (ParameterSet.RkingMode == BaoBei_RkingMode.Sales) bat = newcommdityA.Commodity_Xranking(ProgressUI, searchDateTime);
            else if (ParameterSet.RkingMode == BaoBei_RkingMode.Composite) bat = newcommdityA.Commodity_Zranking(ProgressUI, searchDateTime);
            else if (ParameterSet.RkingMode == BaoBei_RkingMode.AppointSomeSales) bat = newcommdityA.Commodity_XQBaoBei(searchnum, ProgressUI, searchDateTime);
            else if (ParameterSet.RkingMode == BaoBei_RkingMode.AppointCompsite) bat = newcommdityA.Commodity_ZQBaoBei(searchnum, ProgressUI, searchDateTime);
            else { MessageBox.Show("模式选取发生错误，请关闭重启"); return; }
            #region 对original全新赋值
            List<BaoBei_Attribute> originalbat_copy = bat;
            for (int i = 0; i < bat.Count; i++)
            {
                for (int j = 0; j < originalbat.Count; j++)
                {
                    if (bat[i].Baobei_link == originalbat[j].Baobei_link)
                        originalbat_copy[i] = originalbat[j];
                }
            }
            originalbat.Clear();
            originalbat = originalbat_copy;
            #endregion
            #region 对DataSaveList全新赋值
            DataTemporarySave[] datasvelist_copy = new DataTemporarySave[bat.Count];
            for (int i = 0; i < bat.Count; i++)
            {
                datasvelist_copy[i] = new DataTemporarySave();
                datasvelist_copy[i].VolumeSave = bat[i].Baobei_trading.Quantity;
                datasvelist_copy[i].UID_uri = bat[i].Baobei_link;//设定datasavelist的标识符
                DataSaveList[i].PriceSave = bat[i].Baobei_price.ToString();
            }
            for (int i = 0; i < datasvelist_copy.Length; i++)
            {
                for (int j = 0; j < DataSaveList.Length; j++)
                {
                    if (datasvelist_copy[i].UID_uri == DataSaveList[j].UID_uri)
                        datasvelist_copy[i] = DataSaveList[j];
                }
            }
            DataSaveList = null;
            DataSaveList = datasvelist_copy;
            #endregion
            BtSearch.Dispatcher.BeginInvoke(new newdelegate(() =>
            {
                #region 统计总的销售额和人流量
                int pruchpepoleint = 0;
                double totaltradeprice = 0;
                for (int i = 0; i < bat.Count; i++)
                {
                    pruchpepoleint += bat[i].Baobei_TimeOfPeopleNum;
                    totaltradeprice += bat[i].Baobei_TimeOfPrice;
                }
                LbTotalPeople.Content = "预计人流量：" + pruchpepoleint.ToString() + "人，截至目前该版块销售额：" + totaltradeprice + "元";
                #endregion
                PbSearch.Value = 0;
                BtSearch.Content = "搜索";
                BtSearch.IsEnabled = true;
            }), null);

        }

        private void CbSuperMode_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)CbSuperMode.IsChecked) ParameterSet.MianOpenThreadNum = 30;
            else ParameterSet.MianOpenThreadNum = 5;
        }

        private void dg1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                BaoBei_Attribute openuri = (BaoBei_Attribute)dg1.SelectedItem;
                if (openuri == null) return;
                Process.Start(openuri.Baobei_link);
            }
        }
    }
}
