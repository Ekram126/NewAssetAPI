using Asset.Models;
using Asset.ViewModels.HospitalSupplierStatusVM;
using System.Collections.Generic;


namespace Asset.Domain.Repositories
{
    public interface IHospitalSupplierStatusRepository
    {
        IndexHospitalSupplierStatusVM GetAll(int appTypeId, int? hospitalId);
        IndexHospitalSupplierStatusVM GetAllByHospitals();
        HospitalSupplierStatus GetById(int id);

    }
}
