using Asset.ViewModels.SupplierHoldReasonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Repositories
{
    public interface ISupplierHoldReasonRepository
    {
        IEnumerable<IndexSupplierHoldReasonVM.GetData> GetAll();
        EditSupplierHoldReasonVM GetById(int id);
        int Add(CreateSupplierHoldReasonVM reasonObj);
        int Update(EditSupplierHoldReasonVM reasonObj);
        int Delete(int id);
    }
}
