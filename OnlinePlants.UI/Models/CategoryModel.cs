using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MazzGlobal.UI.Models
{
    public class CategoryModel
    {
        public Int64 ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
    }
}