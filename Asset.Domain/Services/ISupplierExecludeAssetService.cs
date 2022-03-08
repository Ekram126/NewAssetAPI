using Asset.Models;
using Asset.ViewModels.SupplierExecludeAssetVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
   public interface ISupplierExecludeAssetService
    {

        IEnumerable<IndexSupplierExecludeAssetVM.GetData> GetAll();
        IEnumerable<IndexSupplierExecludeAssetVM.GetData> GetAllByStatusId(int statusId);
        IEnumerable<IndexSupplierExecludeAssetVM.GetData> GetAllByAppTypeId(int appTypeId);
        IEnumerable<IndexSupplierExecludeAssetVM.GetData> GetAllByStatusIdAndAppTypeId(int statusId, int appTypeId);
        EditSupplierExecludeAssetVM GetById(int id);
        ViewSupplierExecludeAssetVM GetSupplierExecludeAssetDetailById(int id);
        int Add(CreateSupplierExecludeAssetVM execludeAssetObj);
        int Update(EditSupplierExecludeAssetVM execludeAssetObj);
        int UpdateExcludedDate(EditSupplierExecludeAssetVM execludeAssetObj);
        int Delete(int id);


        GenerateSupplierExecludeAssetNumberVM GenerateSupplierExecludeAssetNumber();
        IEnumerable<IndexSupplierExecludeAssetVM.GetData> SortSuplierApp(SortSupplierExecludeAssetVM sortObj);
        int CreateSupplierExecludAttachments(SupplierExecludeAttachment attachObj);
        IEnumerable<SupplierExecludeAttachment> GetAttachmentBySupplierExecludeAssetId(int assetId);
        int DeleteSupplierExecludeAttachment(int id);
    }
}
