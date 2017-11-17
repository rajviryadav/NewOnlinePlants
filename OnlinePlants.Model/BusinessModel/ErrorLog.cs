using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePlants.Model.BusinessModel
{
    public class ErrorLog
    {
        [Key]
        public int ID { get; set; }
        public string Source { get; set; }
        public string StackTrace { get; set; }
        public string PageLocation { get; set; } 
        public DateTime CreatedDate { get; set; }
    }
}
