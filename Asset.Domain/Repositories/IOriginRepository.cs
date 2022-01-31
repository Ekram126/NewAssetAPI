using Asset.Models;
using Asset.ViewModels.OriginVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Repositories
{
   public interface IOriginRepository
    {
        IEnumerable<IndexOriginVM.GetData> GetAll();

        IEnumerable<Origin> GetAllOrigins();
        IEnumerable<IndexOriginVM.GetData> GetOriginByName(string originName);
        IEnumerable<IndexOriginVM.GetData> SortOrigins(SortOriginVM sortObj);

        EditOriginVM GetById(int id);
        int Add(CreateOriginVM originObj);
        int Update(EditOriginVM originObj);
        int Delete(int id);
    }
}
