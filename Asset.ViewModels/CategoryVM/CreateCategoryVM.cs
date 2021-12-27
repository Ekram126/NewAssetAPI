using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.CategoryVM
{
    public class CreateCategoryVM
    {
        public string Code { get; set; }

        public string Name { get; set; }
      
        public string NameAr { get; set; }

        public int? CategoryTypeId { get; set; }
    }
}
