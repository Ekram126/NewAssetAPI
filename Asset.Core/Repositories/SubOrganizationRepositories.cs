using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.SubOrganizationVM;
using Asset.ViewModels.RoleCategoryVM;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Asset.Core.Repositories
{
    public class SubOrganizationRepositories : ISubOrganizationRepository
    {
        private ApplicationDbContext _context;


        public SubOrganizationRepositories(ApplicationDbContext context)
        {
            _context = context;
        }


        public SubOrganization GetById(int id)
        {
            var subOrganizationObj = _context.SubOrganizations.Find(id);
            return subOrganizationObj;
        }




        public IEnumerable<IndexSubOrganizationVM.GetData> GetAll()
        {
            var lstSubOrganizations = _context.SubOrganizations.ToList().Select(item => new IndexSubOrganizationVM.GetData
            {
                Id = item.Id,
                Name = item.Name,
                NameAr = item.NameAr,
                Mobile = item.Mobile,
                Code = item.Code
            });

            return lstSubOrganizations;
        }

        public int Add(CreateSubOrganizationVM subOrganizationVM)
        {
            SubOrganization SubOrganizationObj = new SubOrganization();
            try
            {
                if (subOrganizationVM != null)
                {
                    SubOrganizationObj.Code = subOrganizationVM.Code;
                    SubOrganizationObj.Name = subOrganizationVM.Name;
                    SubOrganizationObj.NameAr = subOrganizationVM.NameAr;
                    SubOrganizationObj.Address = subOrganizationVM.Address;
                    SubOrganizationObj.AddressAr = subOrganizationVM.AddressAr;
                    SubOrganizationObj.Email = subOrganizationVM.Email;
                    SubOrganizationObj.Mobile = subOrganizationVM.Mobile;
                    SubOrganizationObj.DirectorName = subOrganizationVM.DirectorName;
                    SubOrganizationObj.DirectorNameAr = subOrganizationVM.DirectorNameAr;
                    SubOrganizationObj.OrganizationId = subOrganizationVM.OrganizationId;
                    _context.SubOrganizations.Add(SubOrganizationObj);
                    _context.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return SubOrganizationObj.Id;
        }

        public int Delete(int id)
        {
            var SubOrganizationObj = _context.SubOrganizations.Find(id);
            try
            {
                if (SubOrganizationObj != null)
                {
                    _context.SubOrganizations.Remove(SubOrganizationObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }

            return 0;
        }

        public int Update(EditSubOrganizationVM subOrganizationVM)
        {
            try
            {
                var SubOrganizationObj = _context.SubOrganizations.Find(subOrganizationVM.Id);
                SubOrganizationObj.Id = subOrganizationVM.Id;
                SubOrganizationObj.Code = subOrganizationVM.Code;
                SubOrganizationObj.Name = subOrganizationVM.Name;
                SubOrganizationObj.NameAr = subOrganizationVM.NameAr;
                SubOrganizationObj.Address = subOrganizationVM.Address;
                SubOrganizationObj.AddressAr = subOrganizationVM.AddressAr;
                SubOrganizationObj.Email = subOrganizationVM.Email;
                SubOrganizationObj.Mobile = subOrganizationVM.Mobile;
                SubOrganizationObj.DirectorName = subOrganizationVM.DirectorName;
                SubOrganizationObj.DirectorNameAr = subOrganizationVM.DirectorNameAr;
                SubOrganizationObj.OrganizationId = subOrganizationVM.OrganizationId;
                _context.Entry(SubOrganizationObj).State = EntityState.Modified;
                _context.SaveChanges();
                return SubOrganizationObj.Id;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return 0;
        }

        public IEnumerable<IndexSubOrganizationVM.GetData> GetSubOrganizationByOrgId(int orgId)
        {
            var lstSubOrganization = _context.SubOrganizations.ToList().Where(a => a.OrganizationId == orgId).Select(item => new IndexSubOrganizationVM.GetData
            {
                Id = item.Id,
                Name = item.Name,
                NameAr = item.NameAr,
                Mobile = item.Mobile,
                Code = item.Code
            });

            return lstSubOrganization;
        }

        public IEnumerable<SubOrganization> GetAllSubOrganizations()
        {
            return _context.SubOrganizations.ToList();
        }

        public Organization GetOrganizationBySubId(int subId)
        {
            var orgObj = (from org in _context.Organizations
                          join sub in _context.SubOrganizations on org.Id equals sub.OrganizationId
                          where sub.Id == subId
                          select org).ToList().Select(item => new Organization
                          {
                              Id = item.Id,
                              Name = item.Name,
                              NameAr = item.NameAr
                          }).First();

            return orgObj;
        }

        public IEnumerable<IndexSubOrganizationVM.GetData> GetSubOrganizationByOrgName(string orgName)
        {
            return _context.SubOrganizations.ToList().Where(a => (a.Name == orgName|| a.NameAr == orgName)).Select(item => new IndexSubOrganizationVM.GetData
            {
                Id = item.Id,
                Name = item.Name,
                NameAr = item.NameAr,
                Mobile = item.Mobile,
                Code = item.Code
            });
        }
    }
}