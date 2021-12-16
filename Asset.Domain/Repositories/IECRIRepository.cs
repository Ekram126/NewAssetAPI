using System.Collections.Generic;
using Asset.Models;
using Asset.ViewModels.ECRIVM;

namespace Asset.Domain.Repositories
{
  public  interface IECRIRepository
    {
        IEnumerable<IndexECRIVM.GetData> GetAll();
        EditECRIVM GetById(int id);
        IEnumerable<ECRI> GetAllECRIs();
        int Add(CreateECRIVM ecriObj);
        int Update(EditECRIVM ecriObj);
        int Delete(int id);

        IEnumerable<IndexECRIVM.GetData> sortECRI(SortECRIVM searchObj);
    }
}
