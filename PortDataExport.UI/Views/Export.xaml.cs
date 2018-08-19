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
using PortDataExport.UI.Handlers;
using System.IO;
using NPOI;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using ICSharpCode.SharpZipLib;
using NPOI.HSSF.Util;

namespace PortDataExport.UI.Views
{
    /// <summary>
    /// Export.xaml 的交互逻辑
    /// </summary>
    public partial class Export : Window
    {
        public DateTime Today { get; set; }
        public DateTime StartDay { get; set; }
        public DateTime EndDay { get; set; }
        //public string filepath { get; set; }
        public FileArgs args { get; set; }
        public Export()
        {
            InitializeComponent();
            Today = DateTime.Now;
            this.Closing += Export_Closing;
        }

        private void Export_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        //创建查询语句
        private void btnNewSql_Click(object sender, RoutedEventArgs e)
        {
            if(dpStart.SelectedDate == null || dpEnd.SelectedDate == null)
            {
                MessageBox.Show("请选择日期!");
            }
            else if(dpStart.SelectedDate > dpEnd.SelectedDate)
            {
                MessageBox.Show("起始日期不可大于结束日期！");
            }
            else
            {
                StartDay = (DateTime)dpStart.SelectedDate;
                EndDay = (DateTime)dpEnd.SelectedDate;
                List<string> res = GetSqls(StartDay, EndDay, Today);
                SqlDataWindows sdw = new SqlDataWindows
                {
                    Owner = this,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Sqls = res
                };
                sdw.ShowDialog();
                bool IsSave = sdw.IsSave;
            }
        }

        //废弃
        private void btnSetSavePath_Click(object sender, RoutedEventArgs e)
        {

        }

        //选择文件
        private void btnSelectFile_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog o_dialog = new System.Windows.Forms.OpenFileDialog();
            o_dialog.DefaultExt = ".txt";
            o_dialog.Filter = "txt files(*.txt)| *.txt";
            System.Windows.Forms.DialogResult result = o_dialog.ShowDialog();
            if(result == System.Windows.Forms.DialogResult.OK)
            {
                txtFilePath.Text = o_dialog.FileName;
            }
        }

        //保存进Excel文件
        private void btnNewExcel_Click(object sender, RoutedEventArgs e)
        {
            string filepath;
            System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.FileName = string.Format("r{0}", CommonHelpers.GetTimeStamp(DateTime.Now));
            saveFileDialog.Filter = "xls files(*.xls)|*.xls|xlsx files(*.xlsx)|*.xlsx";
            System.Windows.Forms.DialogResult result = saveFileDialog.ShowDialog();
            if(result == System.Windows.Forms.DialogResult.OK)
            {
                filepath = saveFileDialog.FileName;
                //读取文件，准备数据,仅适用于格口数据导出的txt文件
                string[,] data = new string[203, 32];
                data[0, 0] = "";
                data[1, 0] = "no";
                for(int i = 1; i < 202; i++)
                {
                    data[i + 1, 0] = i.ToString();
                }
                using (StreamReader sr = new StreamReader(txtFilePath.Text.Trim()))
                {
                    int col = 1;
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        if (line.Contains("count"))
                        {
                            continue;
                        }
                        else
                        {
                            string[] s = line.Split('\t');
                            if (s[1] == "")
                            {
                                data[0, col] = s[0];
                            }
                            else if (s[1] == "no")
                            {
                                data[1, col] = s[0];
                            }
                            else
                            {
                                int portcode;
                                try
                                {
                                    portcode = int.Parse(s[1]);
                                    data[portcode + 1, col] = s[0];
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                            }
                            if (line.Contains("no"))
                            {
                                col++;
                            }
                        }
                    }
                }
                Write2Excel(filepath, data);
                MessageBox.Show("数据生成成功！");
            }
            else
            {
                return;
            }
        }

        //弃用
        private void btnNewExcelx_Click(object sender, RoutedEventArgs e)
        {

        }

        //获取SQL语句
        public List<string> GetSqls(DateTime start, DateTime end, DateTime today)
        {
            List<string> result = new List<string>();
            Int64 startStamp = CommonHelpers.GetTimeStamp(start);
            Int64 endStamp = CommonHelpers.GetTimeStamp(end);
            Int64 oneDay = Convert.ToInt64(24 * 60 * 60 * 1000);
            Int64 Today = CommonHelpers.GetTimeStamp(today);
            for(Int64 i = startStamp; i <= endStamp; i += oneDay)
            {
                result.Add(string.Format("select count(billcode),sortportcode from alreadysortinfohist where sorttime>'{0}' and sorttime<'{0}' group by sortportcode;", i, i + oneDay)); 
            }

            return result;
        }

        //写入Excel文件
        public void Write2Excel(string filepath, string[,] data)
        {
            //新建xls工作簿
            IWorkbook wb;
            string extension = System.IO.Path.GetExtension(filepath);
            if (extension.Equals(".xls"))
            {
                wb = new HSSFWorkbook();    // 新建xls工作簿
            }
            else
            {
                wb = new XSSFWorkbook();    // 新建xlsx工作簿
            }

            //设置样式
            ICellStyle styleNormal = wb.CreateCellStyle();
            styleNormal.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;   //文字水平垂直对齐
            styleNormal.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;

            ICellStyle styleTitle = wb.CreateCellStyle();
            styleTitle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;    //文字水平垂直对齐
            styleTitle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            styleTitle.FillPattern = FillPattern.SolidForeground;
            styleTitle.FillForegroundColor = HSSFColor.Grey25Percent.Index;

            //创建表单
            ISheet sheet = wb.CreateSheet("sheet1");
            //设置列宽
            for(int i= 0; i < 32; i++)
            {
                sheet.SetColumnWidth(i, 256 * 10);
            }

            //准备数据

            //填充数据
            IRow row;
            ICell cell;
            for(int i = 0; i < 203; i++)
            {
                row = sheet.CreateRow(i);
                for(int j = 0; j < 32; j++)
                {
                    cell = row.CreateCell(j);
                    if(j == 0)
                    {
                        cell.CellStyle = styleTitle;
                    }
                    else
                    {
                        cell.CellStyle = styleNormal;
                    }
                    if(data[i,j] == null)
                    {
                        cell.SetCellValue(" ");
                    }
                    else
                    {
                        cell.SetCellValue(data[i, j]);
                    }
                }
            }

            try
            {
                FileStream fs = File.OpenWrite(filepath);
                wb.Write(fs);   //向这个路径写入数据
                fs.Flush();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        
    }

    //要写入的Excel的一些参数
    public class FileArgs
    {
        public string[] daysName { get; set; }

    }
}
