using Asset.Models;
using Asset.ViewModels.ECRIVM;
using System;
using System.Collections.Generic;

namespace Asset.Domain.Services
{
   public interface IECRIService
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
