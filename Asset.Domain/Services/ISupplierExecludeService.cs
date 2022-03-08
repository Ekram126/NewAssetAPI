using Asset.Models;
using Asset.ViewModels.SupplierExecludeAssetVM;
using Asset.ViewModels.SupplierExecludeVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
   public interface ISupplierExecludeService
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
