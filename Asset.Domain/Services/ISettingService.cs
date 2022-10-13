using Asset.Models;
using Asset.ViewModels.BrandVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
  public  interface ISettingService
    {
        IEnumerable<Setting> GetAll();
        //EditBrandVM GetById(int id);
        //int Add(CreateBrandVM brandObj);
        //int Update(EditBrandVM brandObj);
        //int Delete(int id);

   
    }
}
