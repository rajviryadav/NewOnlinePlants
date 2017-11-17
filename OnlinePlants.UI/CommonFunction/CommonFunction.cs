using OnlinePlants.Data;
using OnlinePlants.Model.BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlinePlants.UI.CommonFunction
{
    public class CommonFunction
    {
        public static void ErrorLog(Exception ex)
        {
            HttpContext objContext = System.Web.HttpContext.Current;
            using (OnlinePlantsContext context = new OnlinePlantsContext())
            {
                ErrorLog log = new ErrorLog();
                log.Source = ex.Source;
                log.StackTrace = ex.StackTrace;
                log.PageLocation = objContext.Request.RawUrl;
                log.CreatedDate = DateTime.UtcNow;
                context.tblErrorLog.Add(log);
            }           
        }
        public static string GetCategoryName(string name)
        {
            name = name.Replace("-n-", " & ");
            name = name.Replace("-", " ");
            name = name.Replace("_", "/");
            return name.ToUpper();
        }
        public static string GetPageName(string name)
        {
            name = name.Trim();
            name = name.Replace('&', 'n');
            name = name.Replace(' ', '-');
            name = name.Replace('/', '_');
            return name.ToLower();
        }
    }
}