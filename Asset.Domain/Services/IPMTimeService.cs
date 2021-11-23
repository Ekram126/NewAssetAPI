using Asset.Models;
using Asset.ViewModels.OriginVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
   public interface IPMTimeService
    {
        IEnumerable<PMTime> GetAll();

        //IEnumerable<Origin> GetAllOrigins();
        //IEnumerable<IndexOriginVM.GetData> GetOriginByName(string originName);
        //EditOriginVM GetById(int id);
        //int Add(CreateOriginVM originObj);
        //int Update(EditOriginVM originObj);
        //int Delete(int id);
    }
}
