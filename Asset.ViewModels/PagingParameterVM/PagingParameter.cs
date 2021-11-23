using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.PagingParameter
{
    public class PagingParameter
    {
        const int maxPageSize = 50;
        public int PageNumber { get; set; } = 1;
      //  public string ColumnName { get; set; }
        private int _pageSize = 10;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (_pageSize > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}
