﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Asset.Models;
using Asset.ViewModels.HospitalVM;


namespace Asset.Domain.Repositories
{
    public interface IHospitalRepository
    {
        IEnumerable<Hospital> GetAllHospitals();
        IEnumerable<IndexHospitalVM.GetData> GetAll();
        EditHospitalVM GetById(int id);
        DetailHospitalVM GetHospitalDetailById(int id);
        Task<IEnumerable<IndexHospitalVM.GetData>> GetHospitalsByUserId(string userId);

        IEnumerable<IndexHospitalVM.GetData> SearchHospitals(SearchHospitalVM searchObj);
        IEnumerable<Hospital> GetHospitalsByCityId(int cityId);
        IEnumerable<Hospital> GetHospitalsBySubOrganizationId(int subOrgId);
        List<SubOrganization> GetSubOrganizationsByHospitalId(int hospitalId);
        int Add(CreateHospitalVM Hospital);
        int Update(EditHospitalVM Hospital);
        int UpdateHospitalDepartment(EditHospitalDepartmentVM hospitalDepartmentVM);
        List<HospitalDepartment> GetHospitalDepartmentByHospitalId(int hospitalId);
        List<CountHospitalVM> CountHospitalsByCities();
        List<IndexHospitalDepartmentVM.GetData> GetHospitalDepartmentByHospitalId2(int hospitalId);
        int CountHospitals();
        int Delete(int id);
    }
}
