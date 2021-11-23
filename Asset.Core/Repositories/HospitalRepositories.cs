using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.HospitalVM;
using Asset.ViewModels.RoleCategoryVM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class HospitalRepositories : IHospitalRepository
    {
        private ApplicationDbContext _context;
        string msg;

        public HospitalRepositories(ApplicationDbContext context)
        {
            _context = context;
        }


        public EditHospitalVM GetById(int id)
       {
            var HospitalObj = _context.Hospitals.Where(a => a.Id == id).Select(item => new EditHospitalVM
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                NameAr = item.NameAr,
                Address = item.Address,
                AddressAr = item.AddressAr,
                Email = item.Email,
                Mobile = item.Mobile,
                Latitude = item.Latitude,
                Longtitude = item.Longtitude,
                ManagerName = item.ManagerName,
                ManagerNameAr = item.ManagerNameAr,
                GovernorateId = item.GovernorateId,
                CityId = item.CityId,
                OrganizationId = item.OrganizationId,
                SubOrganizationId = item.SubOrganizationId,
                Departments = _context.HospitalDepartments.Where(a => a.HospitalId == item.Id).Select(a=>a.DepartmentId).ToList(),
                EnableDisableDepartments = _context.HospitalDepartments.Where(a => a.HospitalId == item.Id).Select(item=> new EnableDisableDepartment { 
                DepartmentId = item.DepartmentId,
                IsActive = item.IsActive
                }).ToList(),

            }).First();
            return HospitalObj;
        }




        public IEnumerable<IndexHospitalVM.GetData> GetAll()
        {
            var lstHospitals = _context.Hospitals.ToList().Select(item => new IndexHospitalVM.GetData
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                NameAr = item.NameAr,
                CityName = item.CityId != 0 ? _context.Cities.Where(a => a.Id == item.CityId).ToList().First().Name : "",
                CityNameAr = item.CityId != 0 ? _context.Cities.Where(a => a.Id == item.CityId).ToList().First().NameAr : "",
                GovernorateName = item.GovernorateId != 0 ? _context.Governorates.Where(a => a.Id == item.GovernorateId).ToList().First().Name : "",
                GovernorateNameAr = item.GovernorateId != 0 ? _context.Governorates.Where(a => a.Id == item.GovernorateId).ToList().First().NameAr : "",
                OrgName = item.OrganizationId != 0 ? _context.Organizations.Where(a => a.Id == item.OrganizationId).ToList().First().Name : "",
                OrgNameAr = item.OrganizationId != 0 ? _context.Organizations.Where(a => a.Id == item.OrganizationId).ToList().First().NameAr : "",
                SubOrgName = item.SubOrganizationId != 0 ? _context.SubOrganizations.Where(a => a.Id == item.SubOrganizationId).ToList().First().Name : "",
                SubOrgNameAr = item.SubOrganizationId != 0 ? _context.SubOrganizations.Where(a => a.Id == item.SubOrganizationId).ToList().First().NameAr : ""
            });

            return lstHospitals;
        }

        public int Add(CreateHospitalVM HospitalVM)
        {
            Hospital HospitalObj = new Hospital();
            try
            {
                if (HospitalVM != null)
                {
                    HospitalObj.Code = HospitalVM.Code;
                    HospitalObj.Name = HospitalVM.Name;
                    HospitalObj.NameAr = HospitalVM.NameAr;
                    HospitalObj.Address = HospitalVM.Address;
                    HospitalObj.AddressAr = HospitalVM.AddressAr;
                    HospitalObj.Email = HospitalVM.Email;
                    HospitalObj.Mobile = HospitalVM.Mobile;
                    HospitalObj.ManagerName = HospitalVM.ManagerName;
                    HospitalObj.ManagerNameAr = HospitalVM.ManagerNameAr;
                    HospitalObj.Latitude = HospitalVM.Latitude;
                    HospitalObj.Longtitude = HospitalVM.Longtitude;
                    HospitalObj.GovernorateId = HospitalVM.GovernorateId;
                    HospitalObj.CityId = HospitalVM.CityId;
                    HospitalObj.OrganizationId = HospitalVM.OrganizationId;
                    HospitalObj.SubOrganizationId = HospitalVM.SubOrganizationId;
                    _context.Hospitals.Add(HospitalObj);
                    _context.SaveChanges();


                    int hospitalId = HospitalObj.Id;
                    if (HospitalVM.Departments.Count > 0)
                    {
                        foreach (var depart in HospitalVM.Departments)
                        {
                            HospitalDepartment hospitalDepartmentObj = new HospitalDepartment();
                            hospitalDepartmentObj.DepartmentId = depart;
                            hospitalDepartmentObj.HospitalId = hospitalId;
                            hospitalDepartmentObj.IsActive = true;
                            _context.HospitalDepartments.Add(hospitalDepartmentObj);
                            int hdId = _context.SaveChanges();
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return HospitalObj.Id;
        }

        public int Delete(int id)
        {
            var HospitalObj = _context.Hospitals.Find(id);
            try
            {
                if (HospitalObj != null)
                {
                   
                    _context.Hospitals.Remove(HospitalObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return 0;
        }

        public int Update(EditHospitalVM HospitalVM)
        {
            try
            {

                var HospitalObj = _context.Hospitals.Find(HospitalVM.Id);
                HospitalObj.Id = HospitalVM.Id;
                HospitalObj.Code = HospitalVM.Code;
                HospitalObj.Name = HospitalVM.Name;
                HospitalObj.NameAr = HospitalVM.NameAr;
                HospitalObj.Address = HospitalVM.Address;
                HospitalObj.AddressAr = HospitalVM.AddressAr;
                HospitalObj.Email = HospitalVM.Email;
                HospitalObj.Mobile = HospitalVM.Mobile;
                HospitalObj.ManagerName = HospitalVM.ManagerName;
                HospitalObj.ManagerNameAr = HospitalVM.ManagerNameAr;
                HospitalObj.Latitude = HospitalVM.Latitude;
                HospitalObj.Longtitude = HospitalVM.Longtitude;
                HospitalObj.GovernorateId = HospitalVM.GovernorateId;
                HospitalObj.CityId = HospitalVM.CityId;
                HospitalObj.OrganizationId = HospitalVM.OrganizationId;
                HospitalObj.SubOrganizationId = HospitalVM.SubOrganizationId;
                _context.Entry(HospitalObj).State = EntityState.Modified;
                _context.SaveChanges();


                if (HospitalVM.Departments.Count > 0 && HospitalVM.Departments != null)
                {
                    var savedId = _context.HospitalDepartments.Where(a => a.HospitalId == HospitalVM.Id).ToList().Select(x => x.DepartmentId).ToList();
                    var newIds = HospitalVM.Departments.ToList().Except(savedId);
                    if (newIds.Count() > 0)
                    {
                        foreach (var newId in newIds)
                        {
                            HospitalDepartment hospitalDepartmentObj = new HospitalDepartment();
                            hospitalDepartmentObj.DepartmentId = newId;
                            hospitalDepartmentObj.HospitalId = HospitalObj.Id;
                            hospitalDepartmentObj.IsActive = true;
                            _context.HospitalDepartments.Add(hospitalDepartmentObj);
                            int hdId = _context.SaveChanges();
                        }
                    }

                    var removedIds = savedId.ToList().Except(HospitalVM.Departments);
                    if (removedIds.Count() > 0)
                    {
                        foreach (var removedId in removedIds)
                        {
                            var hospitalDepartmentObj = _context.HospitalDepartments.Where(a => a.HospitalId == HospitalObj.Id && a.DepartmentId == removedId).First();
                            _context.HospitalDepartments.Remove(hospitalDepartmentObj);
                            int hdId = _context.SaveChanges();
                        }
                    }
                }

                return HospitalObj.Id;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return 0;
        }

        public IEnumerable<Hospital> GetAllHospitals()
        {
            return _context.Hospitals.ToList();
        }

        public async Task<IEnumerable<IndexHospitalVM.GetData>> GetHospitalsByUserId(string userId)
        {
            if (userId != null)
            {
                List<IndexHospitalVM.GetData> lstHospitals = new List<IndexHospitalVM.GetData>();
                var userObj = await _context.Users.FindAsync(userId);


                if (userObj.GovernorateId == 0 && userObj.CityId == 0 && userObj.OrganizationId == 0 && userObj.SubOrganizationId == 0 && userObj.HospitalId == 0)
                {
                    lstHospitals = _context.Hospitals.ToList().Select(item => new IndexHospitalVM.GetData
                    {
                        Id = item.Id,
                        Code = item.Code,
                        Name = item.Name,
                        NameAr = item.NameAr,
                        CityName = item.CityId != 0 ? _context.Cities.Where(a => a.Id == item.CityId).ToList().First().Name : "",
                        CityNameAr = item.CityId != 0 ? _context.Cities.Where(a => a.Id == item.CityId).ToList().First().NameAr : "",
                        GovernorateName = item.GovernorateId != 0 ? _context.Governorates.Where(a => a.Id == item.GovernorateId).ToList().First().Name : "",
                        GovernorateNameAr = item.GovernorateId != 0 ? _context.Governorates.Where(a => a.Id == item.GovernorateId).ToList().First().NameAr : "",
                        OrgName = item.OrganizationId != 0 ? _context.Organizations.Where(a => a.Id == item.OrganizationId).ToList().First().Name : "",
                        OrgNameAr = item.OrganizationId != 0 ? _context.Organizations.Where(a => a.Id == item.OrganizationId).ToList().First().NameAr : "",
                        SubOrgName = item.SubOrganizationId != 0 ? _context.SubOrganizations.Where(a => a.Id == item.SubOrganizationId).ToList().First().Name : "",
                        SubOrgNameAr = item.SubOrganizationId != 0 ? _context.SubOrganizations.Where(a => a.Id == item.SubOrganizationId).ToList().First().NameAr : ""
                    }).ToList();
                }


                if (userObj.GovernorateId > 0 && userObj.CityId == 0 && userObj.HospitalId == 0)
                {
                    lstHospitals = (from host in _context.Hospitals

                                    join sub in _context.SubOrganizations on host.SubOrganizationId equals sub.Id into hostptlSubOrg
                                    from hso in hostptlSubOrg.DefaultIfEmpty()


                                    join org in _context.Organizations on host.OrganizationId equals org.Id into hostOrg
                                    from ho in hostOrg.DefaultIfEmpty()


                                    join city in _context.Cities on host.CityId equals city.Id into hostcity
                                    from hc in hostcity.DefaultIfEmpty()


                                    join gov in _context.Governorates on host.GovernorateId equals gov.Id into govcity
                                    from gc in govcity.DefaultIfEmpty()


                                    where gc.Id == userObj.GovernorateId && host.GovernorateId == userObj.GovernorateId

                                    select new IndexHospitalVM.GetData
                                    {
                                        Id = host.Id,
                                        Code = host.Code,
                                        Name = host.Name,
                                        NameAr = host.NameAr,
                                        GovernorateName = gc.Name,
                                        GovernorateNameAr = gc.NameAr,
                                        CityName = hc.Name,
                                        CityNameAr = hc.NameAr,
                                        OrgName = ho.Name,
                                        OrgNameAr = ho.NameAr,
                                        SubOrgName = hso.Name,
                                        SubOrgNameAr = hso.NameAr
                                    })
                                         .ToList();
                }
                if (userObj.GovernorateId > 0 && userObj.CityId > 0 && userObj.HospitalId == 0)
                {
                    lstHospitals = await (from host in _context.Hospitals

                                          join sub in _context.SubOrganizations on host.SubOrganizationId equals sub.Id
                                          //into hostptlSubOrg
                                       //   from hso in hostptlSubOrg.DefaultIfEmpty()


                                          join org in _context.Organizations on host.OrganizationId equals org.Id
                                          //into hostOrg
                                       //   from ho in hostOrg.DefaultIfEmpty()


                                          join city in _context.Cities on host.CityId equals city.Id 
                                          //into hostcity
                                      //    from hc in hostcity.DefaultIfEmpty()


                                          join gov in _context.Governorates on host.GovernorateId equals gov.Id
                                          //into govcity
                                        //  from gc in govcity.DefaultIfEmpty()

                                          where city.Id == userObj.CityId && gov.Id == userObj.GovernorateId

                                          select new IndexHospitalVM.GetData
                                          {
                                              Id = host.Id,
                                              Code = host.Code,
                                              Name = host.Name,
                                              NameAr = host.NameAr,
                                              GovernorateName = gov.Name,
                                              GovernorateNameAr = gov.NameAr,
                                              CityName = city.Name,
                                              CityNameAr = city.NameAr,
                                              OrgName = org.Name,
                                              OrgNameAr = org.NameAr,
                                              SubOrgName = sub.Name,
                                              SubOrgNameAr = sub.NameAr
                                          })
                                          .ToListAsync();
                }
                if (userObj.GovernorateId > 0 && userObj.CityId > 0 && userObj.HospitalId > 0)
                {
                    lstHospitals = await (from host in _context.Hospitals
                                          join sub in _context.SubOrganizations on host.SubOrganizationId equals sub.Id
                                          join org in _context.Organizations on host.OrganizationId equals org.Id
                                          join city in _context.Cities on host.CityId equals city.Id
                                          join gov in _context.Governorates on host.GovernorateId equals gov.Id
                                          where host.GovernorateId == userObj.GovernorateId
                                         && host.CityId == userObj.CityId
                                         && host.Id == userObj.HospitalId
                                         && userObj.Id == userId
                                          select new IndexHospitalVM.GetData
                                          {
                                              Id = host.Id,
                                              Code = host.Code,
                                              Name = host.Name,
                                              NameAr = host.NameAr,
                                              GovernorateName = gov.Name,
                                              GovernorateNameAr = gov.NameAr,
                                              CityName = city.Name,
                                              CityNameAr = city.NameAr,
                                              OrgName = org.Name,
                                              OrgNameAr = org.NameAr,
                                              SubOrgName = sub.Name,
                                              SubOrgNameAr = sub.NameAr
                                          })
                                        .ToListAsync();
                }
                if (userObj.OrganizationId > 0 && userObj.SubOrganizationId == 0)
                {

                    lstHospitals = await (from host in _context.Hospitals

                                          join org in _context.Organizations on host.OrganizationId equals org.Id into hostOrg
                                          from ho in hostOrg.DefaultIfEmpty()


                                          join sub in _context.SubOrganizations on host.SubOrganizationId equals sub.Id into hostptlSubOrg
                                          from hso in hostptlSubOrg.DefaultIfEmpty()


                                          join city in _context.Cities on host.CityId equals city.Id into hostcity
                                          from hc in hostcity.DefaultIfEmpty()


                                          join gov in _context.Governorates on host.GovernorateId equals gov.Id into govcity
                                          from gc in govcity.DefaultIfEmpty()

                                          where host.OrganizationId == userObj.OrganizationId

                                          select new IndexHospitalVM.GetData
                                          {
                                              Id = host.Id,
                                              Code = host.Code,
                                              Name = host.Name,
                                              NameAr = host.NameAr,
                                              GovernorateName = gc.Name,
                                              GovernorateNameAr = gc.NameAr,
                                              CityName = hc.Name,
                                              CityNameAr = hc.NameAr,
                                              OrgName = ho.Name,
                                              OrgNameAr = ho.NameAr,
                                              SubOrgName = hso.Name,
                                              SubOrgNameAr = hso.NameAr
                                          })
                                           .ToListAsync();


                }
                if (userObj.OrganizationId > 0 && userObj.SubOrganizationId > 0 && userObj.HospitalId == 0)
                {

                    lstHospitals = await (from host in _context.Hospitals

                                          join sub in _context.SubOrganizations on host.SubOrganizationId equals sub.Id into hostptlSubOrg
                                          from hso in hostptlSubOrg.DefaultIfEmpty()


                                          join org in _context.Organizations on host.OrganizationId equals org.Id into hostOrg
                                          from ho in hostOrg.DefaultIfEmpty()


                                          join city in _context.Cities on host.CityId equals city.Id into hostcity
                                          from hc in hostcity.DefaultIfEmpty()


                                          join gov in _context.Governorates on host.GovernorateId equals gov.Id into govcity
                                          from gc in govcity.DefaultIfEmpty()

                                          where host.SubOrganizationId == userObj.SubOrganizationId && host.OrganizationId == userObj.OrganizationId

                                          select new IndexHospitalVM.GetData
                                          {
                                              Id = host.Id,
                                              Code = host.Code,
                                              Name = host.Name,
                                              NameAr = host.NameAr,
                                              GovernorateName = gc.Name,
                                              GovernorateNameAr = gc.NameAr,
                                              CityName = hc.Name,
                                              CityNameAr = hc.NameAr,
                                              OrgName = ho.Name,
                                              OrgNameAr = ho.NameAr,
                                              SubOrgName = hso.Name,
                                              SubOrgNameAr = hso.NameAr
                                          })
                                           .ToListAsync();


                }
                if (userObj.OrganizationId > 0 && userObj.SubOrganizationId > 0 && userObj.HospitalId > 0)
                {

                    lstHospitals = await (from host in _context.Hospitals
                                          join sub in _context.SubOrganizations on host.SubOrganizationId equals sub.Id
                                          join org in _context.Organizations on host.OrganizationId equals org.Id
                                          join city in _context.Cities on host.CityId equals city.Id
                                          join gov in _context.Governorates on host.GovernorateId equals gov.Id
                                          where host.SubOrganizationId == userObj.SubOrganizationId
                                          && host.OrganizationId == userObj.OrganizationId
                                          && host.Id == userObj.HospitalId
                                          && userObj.Id == userId

                                          select new IndexHospitalVM.GetData
                                          {
                                              Id = host.Id,
                                              Code = host.Code,
                                              Name = host.Name,
                                              NameAr = host.NameAr,
                                              GovernorateName = gov.Name,
                                              GovernorateNameAr = gov.NameAr,
                                              CityName = city.Name,
                                              CityNameAr = city.NameAr,
                                              OrgName = org.Name,
                                              OrgNameAr = org.NameAr,
                                              SubOrgName = sub.Name,
                                              SubOrgNameAr = sub.NameAr
                                          })
                                           .ToListAsync();


                }





                //if (userObj.OrganizationId > 0 && userObj.SubOrganizationId > 0 && userObj.HospitalId > 0)
                //{
                //    lstHospitals = await (from host in _context.Hospitals
                //                          join sub in _context.SubOrganizations on host.SubOrganizationId equals sub.Id
                //                          join org in _context.Organizations on host.OrganizationId equals org.Id
                //                          join city in _context.Cities on host.CityId equals city.Id
                //                          join gov in _context.Governorates on host.GovernorateId equals gov.Id
                //                          where host.GovernorateId == userObj.GovernorateId
                //                         && host.CityId == userObj.CityId
                //                         && host.Id == userObj.HospitalId
                //                         && userObj.Id == userId
                //                          select new IndexHospitalVM.GetData
                //                          {
                //                              Id = host.Id,
                //                              Code = host.Code,
                //                              AssetName = host.Name,
                //                              AssetNameAr = host.NameAr,
                //                              GovernorateName = gov.Name,
                //                              GovernorateNameAr = gov.NameAr,
                //                              CityName = city.Name,
                //                              CityNameAr = city.NameAr,
                //                              OrgName = org.Name,
                //                              OrgNameAr = org.NameAr,
                //                              SubOrgName = sub.Name,
                //                              SubOrgNameAr = sub.NameAr
                     
                //                          })
                //                        .ToListAsync();
                //}

                return lstHospitals;
            }
            return null;
        }

        public IEnumerable<Hospital> GetHospitalsByCityId(int cityId)
        {
            return _context.Hospitals.ToList().Where(a => a.CityId == cityId).ToList();

        }

        public IEnumerable<Hospital> GetHospitalsBySubOrganizationId(int subOrgId)
        {
            return _context.Hospitals.ToList().Where(a => a.SubOrganizationId == subOrgId).ToList();
        }

        public DetailHospitalVM GetHospitalDetailById(int id)
        {
            var HospitalObj = _context.Hospitals.Where(a => a.Id == id).Select(item => new DetailHospitalVM
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                NameAr = item.NameAr,
                Address = item.Address,
                AddressAr = item.AddressAr,
                Email = item.Email,
                Mobile = item.Mobile,
                Latitude = item.Latitude,
                Longtitude = item.Longtitude,
                ManagerName = item.ManagerName,
                ManagerNameAr = item.ManagerNameAr,


                Departments = _context.HospitalDepartments.Where(a => a.HospitalId == item.Id).Select(a => a.DepartmentId).ToList(),
                EnableDisableDepartments = _context.HospitalDepartments.Where(a => a.HospitalId == item.Id).Select(item => new EnableDisableDepartment
                {
                    DepartmentId = item.DepartmentId,
                    IsActive = item.IsActive
                }).ToList(),




                GovernorateName = (item.GovernorateId != null || item.GovernorateId != 0) ? _context.Governorates.Where(a => a.Id == item.GovernorateId).First().Name : "",
                GovernorateNameAr = (item.GovernorateId != null || item.GovernorateId != 0) ? _context.Governorates.Where(a => a.Id == item.GovernorateId).First().NameAr : "",
                CityName = (item.CityId != null || item.CityId != 0) ? _context.Cities.Where(a => a.Id == item.CityId).First().Name : "",
                CityNameAr = (item.CityId != null || item.CityId != 0) ? _context.Cities.Where(a => a.Id == item.CityId).First().NameAr : "",


                SubOrganizationName = (item.SubOrganizationId != null || item.SubOrganizationId != 0) ? _context.SubOrganizations.Where(a => a.Id == item.SubOrganizationId).First().Name : "",
                SubOrganizationNameAr = (item.SubOrganizationId != null || item.SubOrganizationId != 0) ? _context.SubOrganizations.Where(a => a.Id == item.SubOrganizationId).First().NameAr : "",


                OrganizationName = (item.SubOrganizationId != null || item.SubOrganizationId != 0) ? (from org in _context.Organizations
                                                                                                      join sub in _context.SubOrganizations on org.Id equals sub.OrganizationId
                                                                                                      select org).First().Name : "",

                OrganizationNameAr = (item.SubOrganizationId != null || item.SubOrganizationId != 0) ? (from org in _context.Organizations
                                                                                                        join sub in _context.SubOrganizations on org.Id equals sub.OrganizationId
                                                                                                        select org).First().NameAr : ""

            }).First();

            return HospitalObj;
        }

        public int UpdateHospitalDepartment(EditHospitalDepartmentVM hospitalDepartmentVM)
        {
            try
            {

                var HospitalDepartmentObj = _context.HospitalDepartments.Where(a => a.HospitalId == hospitalDepartmentVM.HospitalId && a.DepartmentId == hospitalDepartmentVM.DepartmentId).First();
                HospitalDepartmentObj.Id = HospitalDepartmentObj.Id;

                if (HospitalDepartmentObj.IsActive == false)
                    HospitalDepartmentObj.IsActive = true;
                if (HospitalDepartmentObj.IsActive == true)
                    HospitalDepartmentObj.IsActive = false;
                _context.Entry(HospitalDepartmentObj).State = EntityState.Modified;
                _context.SaveChanges();
                return HospitalDepartmentObj.Id;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return 0;
        }




        public List<HospitalDepartment> GetHospitalDepartmentByHospitalId(int hospitalId)
        {
            var lstHospitalDepartments = _context.HospitalDepartments.Where(a => a.HospitalId == hospitalId).ToList();
            return lstHospitalDepartments;
        }

        public List<IndexHospitalDepartmentVM.GetData> GetHospitalDepartmentByHospitalId2(int hospitalId)
        {
            var lstHospitalDepartments = _context.HospitalDepartments.Where(a => a.HospitalId == hospitalId).ToList()
                .Select(item => new IndexHospitalDepartmentVM.GetData
                {
                    Id= item.Id,
                    DepartmentName = _context.Departments.Where(a => a.Id == item.DepartmentId).First().Name,
                    DepartmentNameAr = _context.Departments.Where(a => a.Id == item.DepartmentId).First().NameAr,
                    IsActive = item.IsActive,
                    HospitalId = item.HospitalId,
                    DepartmentId = item.DepartmentId
                }).ToList();

            return lstHospitalDepartments;
        }

        public List<SubOrganization> GetSubOrganizationsByHospitalId(int hospitalId)
        {
            return (from hospital in _context.Hospitals
                    join sub in _context.SubOrganizations on hospital.SubOrganizationId equals sub.Id
                    where hospital.Id == hospitalId
                    select sub).ToList();
        }

        public List<CountHospitalVM> CountHospitalsByCities()
        {
            List<CountHospitalVM> list = new List<CountHospitalVM>();


            var lstCities = _context.Cities.ToList().Take(5);
            foreach (var item in lstCities)
            {
                CountHospitalVM countHospitalObj = new CountHospitalVM();
                countHospitalObj.CityName = item.Name;
                countHospitalObj.CityNameAr = item.NameAr;
                countHospitalObj.CountOfHospitals = _context.Hospitals.Where(a => a.CityId == item.Id).ToList().Count;
                list.Add(countHospitalObj);
            }

            return list;
        }

        public int CountHospitals()
        {
            return _context.Hospitals.Count();
        }
       
    }
}