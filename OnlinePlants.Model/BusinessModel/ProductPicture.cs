using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePlants.Model.BusinessModel
{
    public class ProductPicture
    {
        [Key]
        public int ID { get; set; }
        public int ProductId { get; set; }
        public string OriginalImage { get; set; }
        public string ThumnailSmall { get; set; }
        public string ThumnailMeduim { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}