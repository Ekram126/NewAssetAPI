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
        IEnumerable<IndexHospitalApplicationVM.GetData> GetAllByHospitalId(int hospitalId);
        IEnumerable<IndexHospitalApplicationVM.GetData> GetAllByStatusId(int statusId, int hospitalId);
        IEnumerable<IndexHospitalApplicationVM.GetData> GetAllByAppTypeId(int appTypeId);
        IEnumerable<IndexHospitalApplicationVM.GetData> GetAllByAppTypeIdAndStatusId(int statusId,int appTypeId,int hospitalId);


        IndexHospitalApplicationVM GetAllHospitalExecludes(SearchHospitalApplicationVM searchObj,int statusId, int appTypeId, int hospitalId, int pageNumber, int pageSize);
        IndexHospitalApplicationVM GetAllHospitalHolds(SearchHospitalApplicationVM searchObj,int statusId, int appTypeId, int hospitalId, int pageNumber, int pageSize);


        IndexHospitalApplicationVM GetAllHospitalExecludes(SearchHospitalApplicationVM searchObj);
        IndexHospitalApplicationVM GetAllHospitalHolds(SearchHospitalApplicationVM searchObj);




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
        IEnumerable<IndexHospitalApplicationVM.GetData> SortHospitalApp(SortHospitalApplication sortObj);


        IEnumerable<IndexHospitalApplicationVM.GetData> GetHospitalApplicationByDate(SearchHospitalApplicationVM searchObj);
        GeneratedHospitalApplicationNumberVM GenerateHospitalApplicationNumber();

    }
}
