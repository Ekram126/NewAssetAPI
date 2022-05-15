using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.BrandVM;
using Asset.ViewModels.DateVM;
using Asset.ViewModels.DepartmentVM;
using Asset.ViewModels.MultiIDVM;
using Asset.ViewModels.OrganizationVM;
using Asset.ViewModels.SubOrganizationVM;
using Asset.ViewModels.SupplierVM;
using System.Collections.Generic;
using System.Linq;

namespace Asset.Core.Repositories
{
    public class HealthRepository : IHealthRepository
    {
        private ApplicationDbContext _context;

        public HealthRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<HealthBrandVM> GetBrandsetails(int[] model)
        {
            var BrandComparer = new Services.Comparer<HealthBrandVM>("Id");
            var brands = new List<HealthBrandVM>();
            if (model != null || model.Length != 0)
            {
                var query = (from d in _context.Departments
                           //  join hd in _context.HospitalDepartments
                           //  on d.Id equals hd.DepartmentId
                           //  join h in _context.Hospitals
                           //  on hd.HospitalId equals h.Id
                             join a in _context.AssetDetails
                             on d.Id equals a.DepartmentId
                             join m in _context.MasterAssets
                             on a.MasterAssetId equals m.Id
                             join b in _context.Brands
                             on m.BrandId equals b.Id
                             where model.Contains(d.Id)
                             select new
                             {
                                 BrandName = b.Name,
                                 BrandNameAr = b.NameAr,
                                 Id = b.Id,
                                 HospitalCode = a.Hospital.Code
                             }).AsEnumerable().Select(x => new HealthBrandVM()
                             {
                                 Name = x.BrandName,
                                 NameAr = x.BrandNameAr,
                                 Id = x.Id,
                                 HospitalCode = x.HospitalCode
                             });

                return query.Distinct(BrandComparer).ToList();
            }
            return null;
        }

        public IEnumerable<Hospital> GetDateRange(dateVM dates)
        {
            var hosList = new List<Hospital>();
            var Assets = new List<AssetDetail>();
            if (dates.from == null)
            {
                Assets = _context.AssetDetails.Where(a => a.InstallationDate <= dates.to).ToList();
            }
            else if (dates.to == null)
            {
                Assets = _context.AssetDetails.Where(a => a.InstallationDate >= dates.from).ToList();
            }
            else
            {
                Assets = _context.AssetDetails.Where(a => a.InstallationDate >= dates.from && a.InstallationDate <= dates.to).ToList();
            }
            foreach (var Asset in Assets)
            {
                var hos = _context.Hospitals.Where(h => h.Id == Asset.HospitalId).FirstOrDefault();
                if (!hosList.Any(hospital => hospital.Id == hos.Id))
                {
                    hosList.Add(hos);
                }
            }
            return hosList;
        }

        public IEnumerable<HealthDepartmentVM> GetDepartmants(int[] orgIds)
        {
            var DeptComparer = new Services.Comparer<HealthDepartmentVM>("DepartmentID");
            if (orgIds != null)
            {
                var query = (from h in _context.Hospitals
                             join s in _context.SubOrganizations
                             on h.SubOrganizationId equals s.Id
                             join hd in _context.HospitalDepartments
                             on h.Id equals hd.HospitalId
                             join d in _context.Departments
                             on hd.DepartmentId equals d.Id
                             where orgIds.Contains(s.Id)
                             select new
                             {
                                 DeptId = d.Id,
                                 DeptName = d.Name,
                                 DeptNameAr = d.NameAr,
                                 HospitalId = h.Id,
                                 HospitalCode = h.Code,
                                 suborg=s.Id

                             }).AsEnumerable().Select(x => new HealthDepartmentVM
                             {
                                 DepartmentID = x.DeptId,
                                 DepartmentArName = x.DeptNameAr,
                                 DepartmentEngName = x.DeptName,
                                 HospitalCode = x.HospitalCode,
                                 HospitalID = x.HospitalId,
                                 subOrgId=x.suborg
                             });
                return query.ToList();

            }
            return null;
        }

        public IEnumerable<Hospital> GetHospitalInCity(string[] modelID)
        {
            var hospitalLst = new List<Hospital>();
            foreach (var cityCode in modelID)
            {
                var city = _context.Cities.Where(c => c.Code == cityCode).FirstOrDefault();
                if (city != null)
                {
                    var hos = _context.Hospitals.Where(h => h.CityId == city.Id).ToList();
                    hospitalLst.AddRange(hos);
                }
            }
            return hospitalLst;
        }

        public IEnumerable<Hospital> GetHospitalInDepartment(int[] DeptIds)
        {
            var hos = (from d in _context.Departments
                       join hd in _context.HospitalDepartments
                       on d.Id equals hd.DepartmentId
                       join h in _context.Hospitals
                       on hd.HospitalId equals h.Id
                       where DeptIds.Contains(d.Id)
                       select new
                       {
                           hospitalName = h.Name,
                           hospitalId = h.Id,
                           hospitalCode = h.Code
                       }).AsEnumerable().Select(x => new Hospital
                       {
                           Name = x.hospitalName,
                           Id = x.hospitalId,
                           Code = x.hospitalCode
                       }).ToList();
            return hos;
        }

        public IEnumerable<Hospital> GetHospitalInSubOrganization(int[] subOrgIds)
        {
            var hospitalLst = new List<Hospital>();
            foreach (var id in subOrgIds)
            {
                var hos = _context.Hospitals.Where(h => h.SubOrganizationId == id).ToList();
                hospitalLst.AddRange(hos);
            }
            return hospitalLst;
        }

        public IEnumerable<Hospital> GetHospitalsBySupplier(int[] supplierIds)
        {
            var query = (from a in _context.AssetDetails
                         join h in _context.Hospitals
                         on a.HospitalId equals h.Id
                         join s in _context.Suppliers
                         on a.SupplierId equals s.Id
                         where supplierIds.Contains(s.Id)
                         select new
                         {
                             name = h.Name,
                             nameAr = h.NameAr,
                             code = h.Code
                         }).AsEnumerable().Select(x => new Hospital
                         {
                             Name = x.name,
                             NameAr = x.nameAr,
                             Code = x.code
                         });
            return query.ToList();
        }

        public IEnumerable<Hospital> GetHospitalsInOrganization(int[] orgIds)
        {
            var hospitalLst = new List<Hospital>();
            foreach (var orgid in orgIds)
            {
                var hos = _context.Hospitals.Where(h => h.OrganizationId == orgid).ToList();
                hospitalLst.AddRange(hos);
            }
            return hospitalLst;
        }

        public IEnumerable<HealthOrganizationVM> GetOrganizationDetails(getMultiIDVM model)
        {
            var orgComparer = new Services.Comparer<HealthOrganizationVM>("Id");
            var query = (from a in _context.Hospitals
                         join b in _context.SubOrganizations
                         on a.SubOrganizationId equals b.Id
                         join c in _context.Organizations
                         on b.OrganizationId equals c.Id
                         where model.Id.Contains(a.City.Code)
                         select new
                         {
                             Id = c.Id,
                             orgName = c.Name,
                             orgNameAr = c.NameAr,
                             HospitalCode = a.Code,
                             subOrgId = b.Id,
                         }).AsEnumerable().Select(x => new HealthOrganizationVM()
                         {

                             Id = x.Id,
                             Name = x.orgName,
                             NameAr = x.orgNameAr,
                             HospitalCode = x.HospitalCode,
                             subOrganizationId = x.subOrgId,
                         });
            return query.Distinct(orgComparer).ToList();
        }

        public IEnumerable<Hospital> GetPriceRange(decimal FPrice, decimal ToPrice)
        {
            var hosList = new List<Hospital>();
            var Assets = new List<AssetDetail>();
            if (FPrice == 0)
            {
                Assets = _context.AssetDetails.Where(a => a.Price <= ToPrice).ToList();
            }
            else if (ToPrice == 0)
            {
                Assets = _context.AssetDetails.Where(a => a.Price >= FPrice).ToList();
            }
            else
            {
                Assets = _context.AssetDetails.Where(a => a.Price >= FPrice && a.Price <= ToPrice).ToList();
            }
            foreach (var Asset in Assets)
            {
                var hos = _context.Hospitals.Where(h => h.Id == Asset.HospitalId).FirstOrDefault();
                if (!hosList.Any(hospital => hospital.Id == hos.Id))
                {
                    hosList.Add(hos);
                }
            }
            return hosList;
        }

        public IEnumerable<HealthSubOrganizationVM> GetSubOrganizationDetails(int[] orgId)
        {
            var suborgs = new List<HealthSubOrganizationVM>();
            if (orgId != null)
            {
                var query = (from a in _context.Organizations
                             join b in _context.SubOrganizations
                             on a.Id equals b.OrganizationId
                             where orgId.Contains(a.Id)
                             select new
                             {
                                 subOrgName = b.Name,
                                 subOrgNameAr = b.NameAr,
                                 orgId = a.Id,
                                 Id = b.Id,
                             }).AsEnumerable().Select(x => new HealthSubOrganizationVM()
                             {
                                 Name = x.subOrgName,
                                 NameAr = x.subOrgNameAr,
                                 OrganizationId = x.orgId,
                                 Id = x.Id,
                             });

                return query.ToList();
            }
            return null;
        }

        public IEnumerable<HealthSupplierVM> GetSuppliersDetails(int[] brandId)
        {
            var orgComparer = new Services.Comparer<HealthSupplierVM>("Id");
            var query = (from a in _context.AssetDetails
                         join m in _context.MasterAssets
                         //join h in _context.Hospitals
                         on a.MasterAssetId equals m.Id
                         join b in _context.Brands
                         on m.BrandId equals b.Id
                         join s in _context.Suppliers
                         on a.SupplierId equals s.Id
                         where brandId.Contains(b.Id)
                         select new
                         {
                             Name = s.Name,
                             NameAr = s.NameAr,
                             //HospitalCod = h.Code,
                             Id = s.Id
                         }).AsEnumerable().Select(x => new HealthSupplierVM
                         {
                             Name = x.Name,
                             NameAr = x.NameAr,
                             //HospitalCode = x.HospitalCod,
                             Id = x.Id
                         });
            return query.Distinct(orgComparer).ToList();
        }
    }
}
