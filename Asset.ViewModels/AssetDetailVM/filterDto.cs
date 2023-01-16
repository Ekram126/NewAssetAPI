using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.AssetDetailVM
{
    public class filterDto
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string brandName { get; set; }
        public string cityName { get; set; }
        public string hosName { get; set; }
        public string govName { get; set; }
        public string SupplierName { get; set; }

        public string CategoryName { get; set; }
        public string AssetPeriorityName { get; set; }

        public DateTime? purchaseDate { get; set; }
    }
}
