using Asset.ViewModels.PagingParameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Repositories
{
    public interface IPagingRepository 
    {
        IEnumerable<T> GetAll<T>(PagingParameter pageInfo, List<T> objList) where T:class;
        public int Count<T>() where T : class;
    }
}
