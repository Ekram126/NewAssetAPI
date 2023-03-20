using Asset.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.SupplierVM
{
    public class CreateSupplierVM
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string NameAr { get; set; }

        public string Mobile { get; set; }

        public string Website { get; set; }


        public string EMail { get; set; }

        public string Address { get; set; }
        public string AddressAr { get; set; }

        public string ContactPerson { get; set; }
        public string Notes { get; set; }

        public string Fax { get; set; }

        public List<SupplierAttachment> Attachments { get; set; }
    }
}
