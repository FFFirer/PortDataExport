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

namespace PortDataExport.UI.Views
{
    /// <summary>
    /// SqlDataWindows.xaml 的交互逻辑
    /// </summary>
    public partial class SqlDataWindows : Window
    {
        public bool IsSave { get; set; }
        public List<string> Sqls { get; set; }
        public SqlDataWindows()
        {
            InitializeComponent();
        }


        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            string sqlText = string.Empty; ;
            foreach(string s in Sqls)
            {
                sqlText += string.Format("{0}\r\n", s);
            }
            System.Windows.Forms.Clipboard.SetText(sqlText);
            MessageBox.Show("已经复制到剪贴板！");
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.SaveFileDialog s_dialog = new System.Windows.Forms.SaveFileDialog();
            s_dialog.FileName = CommonHelpers.GetTimeStamp(DateTime.Now).ToString();
            s_dialog.Filter = "txt files(*.txt)| *.txt";
            System.Windows.Forms.DialogResult result = s_dialog.ShowDialog();
            if(result == System.Windows.Forms.DialogResult.OK)
            {
                string path = s_dialog.FileName;
                CommonHelpers.WriteSql2File(path, Sqls);
                MessageBox.Show("保存成功！");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach(string s in Sqls)
            {
                txtbSqls.Text += string.Format("{0}\n", s);
            }
        }
    }
}
