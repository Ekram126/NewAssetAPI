using Asset.ViewModels.SupplierHoldReasonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
    public interface ISupplierHoldReasonService
    {
        IEnumerable<IndexSupplierHoldReasonVM.GetData> GetAll();
        EditSupplierHoldReasonVM GetById(int id);
        int Add(CreateSupplierHoldReasonVM reasonObj);
        int Update(EditSupplierHoldReasonVM reasonObj);
        int Delete(int id);
    }
}
