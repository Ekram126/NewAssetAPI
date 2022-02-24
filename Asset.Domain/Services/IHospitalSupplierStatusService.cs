using Asset.Models;
using Asset.ViewModels.HospitalSupplierStatusVM;
using Asset.ViewModels.RequestStatusVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
    public interface IHospitalSupplierStatusService
    {
        IndexHospitalSupplierStatusVM GetAll(int statusId, int appTypeId, int? hospitalId);
        IndexHospitalSupplierStatusVM GetAllByHospitals();
        IndexHospitalSupplierStatusVM GetAllByHospitals(int statusId, int appTypeId, int? hospitalId);
        HospitalSupplierStatus GetById(int id);

    }
}
