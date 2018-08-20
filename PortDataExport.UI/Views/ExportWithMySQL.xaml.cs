using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Configuration;
using PortDataExport.UI.Entities;

namespace PortDataExport.UI.Views
{
    /// <summary>
    /// ExportWithMySQL.xaml 的交互逻辑
    /// </summary>
    public partial class ExportWithMySQL : Window
    {
        public string PortNum { get; set; } //格口数
        int Ports = 0;
        int Days = 0;

        public ExportWithMySQL()
        {
            InitializeComponent();
            this.Closing += ExportWithMySQL_Closing;
        }

        private void ExportWithMySQL_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// 将格口数据导出到xls文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportData2Xls_Click(object sender, RoutedEventArgs e)
        {
            //判断是否有格口数，没有则发出警示并退出
            if(txtPortNum.Text.Trim() == "")
            {
                return;
            }
            //判断格口数是否改变，改变后将新的值保存
            if(PortNum != txtPortNum.Text.Trim())
            {
                if (int.TryParse(txtPortNum.Text.Trim(), out this.Ports))
                {
                    PortNum = txtPortNum.Text.Trim();
                    SavePortNum();
                }
                else
                {
                    MessageBox.Show("请输入一个正确的整数");
                }
            }
            //判断所选日期是否正确
            Days = dpEnd.SelectedDate.Value.Day - dpStart.SelectedDate.Value.Day;
            if (Days <= 0)
            {
                MessageBox.Show("起始日期必须小于结束日期！");
                return;
            }
            //选择导出文件存储位置
            string filepath;
            System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.FileName = string.Format("格口数据{0}", DateTime.Now.ToShortDateString());
            saveFileDialog.Filter = "xls files(*.xls)|*.xls|xlsx files(*.xlsx)|*.xlsx";
            System.Windows.Forms.DialogResult result = saveFileDialog.ShowDialog();
            if(result == System.Windows.Forms.DialogResult.OK)
            {
                filepath = saveFileDialog.FileName;
            }
            else
            {
                return;
            }
            //连接数据库查询数据，返回二维数组
            string[,] Data = GetData(Ports, Days, dpStart.SelectedDate.Value, dpEnd.SelectedDate.Value);
            //写入xls文件,需要：查询日期总天数、总格口数
        }

        /// <summary>
        /// 窗口加载完之后进行数据的初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //从配置文件读取格口数
            this.PortNum = ConfigurationManager.AppSettings["PortNum"].ToString();
            this.txtPortNum.Text = PortNum;
        }

        /// <summary>
        /// 保存格口数到配置文件
        /// </summary>
        private void SavePortNum()
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings["PortNum"].Value = this.txtPortNum.Text.Trim();
            configuration.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appsettings");
        }

        public string[,] GetData(int portnum, int days, DateTime start, DateTime end)
        {
            //第一行日期，第二行空，第三行no，剩下具体格口数据
            string[,] Data = new string[portnum + 3, days + 1];
            //select count(billcode),sortportcode from alreadysortinfo where sorttime>'起始时间戳' and sorttime<'结束时间戳' group by sortportcode
            using(var db = new WcsDbContext())
            {
                for(int i = 0; i<(end.Day-start.Day); i++)
                {
                    if (start.AddDays(i) < DateTime.Now.AddDays(-2))
                    {
                        //已经转移到历史表

                    }
                    else
                    {
                        //还在正表里
                    }
                }
            }
            return Data;
        }
    }
}
