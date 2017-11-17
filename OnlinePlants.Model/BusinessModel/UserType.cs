using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OnlinePlants.Model.BusinessModel
{
    public class UserType
    {
        [Key]
        public int ID { get; set; }
        public string TypeName { get; set; }

       
    }
}
