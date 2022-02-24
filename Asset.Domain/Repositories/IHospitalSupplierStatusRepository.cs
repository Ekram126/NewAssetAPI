using Asset.Models;
using Asset.ViewModels.HospitalSupplierStatusVM;
using System.Collections.Generic;


namespace Asset.Domain.Repositories
{
    public interface IHospitalSupplierStatusRepository
    {
        IndexHospitalSupplierStatusVM GetAll(int statusId, int appTypeId, int? hospitalId);
        IndexHospitalSupplierStatusVM GetAllByHospitals();
        IndexHospitalSupplierStatusVM GetAllByHospitals(int statusId, int appTypeId, int? hospitalId);
        HospitalSupplierStatus GetById(int id);

    }
}
