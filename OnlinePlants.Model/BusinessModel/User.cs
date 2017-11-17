using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePlants.Model.BusinessModel
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public int RegID { get; set; }
        public int UserTypeID { get; set; }
        public string Username { get; set; }        
        public string UserPassword { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }        
    }
}
