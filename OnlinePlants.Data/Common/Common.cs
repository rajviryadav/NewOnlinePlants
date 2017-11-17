using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazzGlobal.Data.Common
{
    public class Common
    {
        public static string GetUserIP()
        {
            string clientip = string.Empty;
            try
            {
                string strHostName = System.Net.Dns.GetHostName();
                string clientIPAddress = System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();
                clientip = clientIPAddress.ToString();
            }
            catch (Exception ex)
            {

            }
            return clientip;
        }
    }
}
