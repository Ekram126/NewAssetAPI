using Asset.ViewModels.PagingParameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
    public interface IPagingService
    {
        IEnumerable<T> GetAll<T>(PagingParameter pageInfo,List<T> objList) where T:class;
        int Count<T>() where T : class;
    }
}
