using Asset.ViewModels.SupplierExecludeReasonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Repositories
{
    public interface ISupplierExecludeReasonRepository
    {
        IEnumerable<IndexSupplierExcludeReasonVM.GetData> GetAll();
        EditSupplierExcludeReasonVM GetById(int id);
        int Add(CreateSupplierExcludeReasonVM reasonObj);
        int Update(EditSupplierExcludeReasonVM reasonObj);
        int Delete(int id);
    }
}
