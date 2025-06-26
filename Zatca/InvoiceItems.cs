using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zatca
{
    public class InvoiceItems
    {
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal ProductQuantity { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal TotalPriceAfterDiscount { get; set; }
        public decimal VatPercentage { get; set; }
        public decimal VatValue { get; set; }
        public decimal TotalWithVat { get; set; }
    }
}
