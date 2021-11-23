using Asset.Models;
using Asset.ViewModels.HospitalApplicationVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Repositories
{
    public interface IHospitalApplicationRepository
    {
        IEnumerable<IndexHospitalApplicationVM.GetData> GetAll();
        IEnumerable<IndexHospitalApplicationVM.GetData> GetAllByStatusId(int statusId);
        EditHospitalApplicationVM GetById(int id);
        ViewHospitalApplicationVM GetHospitalApplicationById(int id);

        int GetAssetHospitalId(int assetId);



        int Add(CreateHospitalApplicationVM hospitalApplicationObj);
        int Update(EditHospitalApplicationVM hospitalApplicationObj);
        int UpdateExcludedDate(EditHospitalApplicationVM hospitalApplicationObj);
        int Delete(int id);




        int CreateHospitalApplicationAttachments(HospitalApplicationAttachment attachObj);
        IEnumerable<HospitalApplicationAttachment> GetAttachmentByHospitalApplicationId(int assetId);
        int DeleteHospitalApplicationAttachment(int id);
    }
}
