using Asset.Models;
using Asset.ViewModels.SupplierExecludeVM;
using System.Collections.Generic;

namespace Asset.Domain.Repositories
{
    public interface ISupplierExecludeRepository
    {

        IEnumerable<SupplierExeclude> GetAll();
        SupplierExeclude GetById(int id);
        int Add(SupplierExeclude execludeAssetObj);
        int Update(SupplierExeclude execludeAssetObj);
        int Delete(int id);

        IEnumerable<SupplierExeclude> GetSupplierExecludesBySupplierExecludeAssetId(int supplierExecludeAssetId);
        List<IndexSupplierExecludeVM.GetData> GetAttachmentBySupplierExecludeAssetId(int supplierExecludeAssetId);
    }
}
