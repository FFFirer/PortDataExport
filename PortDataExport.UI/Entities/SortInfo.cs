using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortDataExport.UI.Entities
{
    public class PortData
    {
        public string PortNo { get; set; }
        public string Number { get; set; }
    }

    public class PortDataOfDay
    {
        public List<PortData> PortDatas { get; set; }
    }

    public class QueryCondition
    {
        public string StartStamp { get; set; }
        public string EndStamp { get; set; }
    }
}
