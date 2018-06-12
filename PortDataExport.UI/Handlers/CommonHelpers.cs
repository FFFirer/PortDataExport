using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortDataExport.UI.Handlers
{
    public class CommonHelpers
    {
        //获取当前时间时间戳
        public static Int64 GetTimeStamp(DateTime oneday)
        {
            TimeSpan Ts = oneday - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(Ts.TotalMilliseconds - 8*60*60*1000);
        }

        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp">Unix时间戳格式</param>
        /// <returns>C#格式时间</returns>
        public static DateTime GetTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));

            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        //保存SQL语句
        public static void WriteSql2File(string path, List<string> content)
        {
            if (!File.Exists(path))
            {
                var myfile = File.Create(path);
                myfile.Close();
            }
            using (StreamWriter sw = new StreamWriter(path))
            {
                foreach (var sql in content)
                {
                    sw.WriteLine(sql);
                }
            }
        }
    }
}
