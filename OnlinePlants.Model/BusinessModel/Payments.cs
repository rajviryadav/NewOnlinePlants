using System;
using System.ComponentModel.DataAnnotations;

namespace OnlinePlants.Model.BusinessModel
{
    public class Payments
    {
        [Key]
        public int ID { get; set; }
        public int RegID { get; set; }
        public decimal Amount { get; set; }
        public Int64 PaymentType { get; set; }
        public string TransactionID { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
