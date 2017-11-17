using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePlants.Business
{
    public class ResponseMessage
    {
        public int RCode { get; set; }
        public string RColorCode { get; set; }
        public string RMessage { get; set; }
        public string RException { get; set; }
        public string RURL { get; set; }
        public object classobject { get; set; }


    }

    public class ResponseMessage2<T>
    {
        public int RCode { get; set; }
        public string RColorCode { get; set; }
        public string RMessage { get; set; }
        public string RException { get; set; }
        public string RURL { get; set; }
        public object classobject { get; set; }
        public List<T> objectList { get; set; }
        public int TotalListRecords { get; set; }
        public int TotalPages { get; set; }

    }
}
