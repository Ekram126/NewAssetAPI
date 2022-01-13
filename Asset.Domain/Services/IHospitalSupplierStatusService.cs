using Asset.Models;
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
        IEnumerable<HospitalSupplierStatus> GetAll();
        HospitalSupplierStatus GetById(int id);

    }
}
