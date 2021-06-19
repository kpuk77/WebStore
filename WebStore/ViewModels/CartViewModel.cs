using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.ViewModels
{
    public class CartViewModel
    {
        public IEnumerable<(ProductViewModel product, int quantity)> Items { get; set; }

        public int ItemsCount => Items?.Sum(i => i.quantity) ?? 0;

        public decimal TotalPrice => Items?.Sum(i => i.product.Price * i.quantity) ?? 0m;
    }
}
