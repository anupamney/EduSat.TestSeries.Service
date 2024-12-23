using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edusat.TestSeries.Service.Domain.Models.Templates
{
    internal class ReceiptModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public List<ReceiptItem> Items { get; set; }
    }
}
