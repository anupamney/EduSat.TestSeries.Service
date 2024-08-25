using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edusat.TestSeries.Service.Domain.Models.Templates
{
    public class ReceiptItem
    {
        public int SerialNumber { get; set; }
        public string Details { get; set; }
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal TotalAmount { get; set; }
        public int Paise { get; set; }
    }
}
