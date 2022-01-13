using Asset.Models;
using System.Collections.Generic;


namespace Asset.Domain.Repositories
{
    public interface IHospitalSupplierStatusRepository
    {
        IEnumerable<HospitalSupplierStatus> GetAll();
        HospitalSupplierStatus GetById(int id);

    }
}
