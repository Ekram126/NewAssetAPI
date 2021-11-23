using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.CommetieeMemberVM;
using Asset.ViewModels.SupplierVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class CommetieeMemberRepositories : ICommetieeMemberRepository
    {
        private ApplicationDbContext _context;


        public CommetieeMemberRepositories(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Add(CreateCommetieeMemberVM model)
        {
            CommetieeMember memberObj = new CommetieeMember();
            try
            {
                if (model != null)
                {
                    memberObj.Code = model.Code;
                    memberObj.Name = model.Name;
                    memberObj.NameAr = model.NameAr;
                    memberObj.Mobile = model.Mobile;
                    memberObj.Website = model.Website;
                    memberObj.EMail = model.EMail;
                    memberObj.Address = model.Address;
                    memberObj.AddressAr = model.AddressAr;
                    _context.CommetieeMembers.Add(memberObj);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return memberObj.Id;
        }

        public int CountCommetieeMembers()
        {
            return _context.CommetieeMembers.Count();
        }

        public int Delete(int id)
        {
            var memberObj = _context.CommetieeMembers.Find(id);
            try
            {
                _context.CommetieeMembers.Remove(memberObj);
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }

        public IEnumerable<IndexCommetieeMemberVM.GetData> GetAll()
        {
            return _context.CommetieeMembers.ToList().Select(item => new IndexCommetieeMemberVM.GetData
            {
                Id = item.Id,
                Code=item.Code,
                Name = item.Name,
                NameAr = item.NameAr
            });
        }

        public IEnumerable<CommetieeMember> GetAllCommetieeMembers()
        {
            return _context.CommetieeMembers.ToList();
        }

        public EditCommetieeMemberVM GetById(int id)
        {
            return _context.CommetieeMembers.Where(a => a.Id == id).Select(item => new EditCommetieeMemberVM
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                NameAr = item.NameAr
            }).First();
        }

        public IEnumerable<IndexCommetieeMemberVM.GetData> GetCommetieeMemberByName(string name)
        {
            return _context.CommetieeMembers.Where(a => a.Name == name || a.NameAr == name).ToList().Select(item => new IndexCommetieeMemberVM.GetData
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                NameAr = item.NameAr
            });
        }

        public int Update(EditCommetieeMemberVM model)
        {
            try
            {
                var memberObj = _context.CommetieeMembers.Find(model.Id);
                memberObj.Id = model.Id;
                memberObj.Code = model.Code;
                memberObj.Name = model.Name;
                memberObj.NameAr = model.NameAr;
                memberObj.Mobile = model.Mobile;
                memberObj.Website = model.Website;
                memberObj.EMail = model.EMail;
                memberObj.Address = model.Address;
                memberObj.AddressAr = model.AddressAr;
                _context.Entry(memberObj).State = EntityState.Modified;
                _context.SaveChanges();
                return memberObj.Id;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }
    }
}
