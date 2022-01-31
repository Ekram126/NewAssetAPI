using Asset.Models;
using Asset.ViewModels.OriginVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
   public interface IOriginService
    {
        IEnumerable<IndexOriginVM.GetData> GetAll();
        EditOriginVM GetById(int id);

        IEnumerable<Origin> GetAllOrigins();
        IEnumerable<IndexOriginVM.GetData> GetOriginByName(string originName);
        IEnumerable<IndexOriginVM.GetData> SortOrigins(SortOriginVM sortObj);
        int Add(CreateOriginVM originObj);
        int Update(EditOriginVM originObj);
        int Delete(int id);
    }
}
