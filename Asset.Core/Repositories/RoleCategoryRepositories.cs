using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.RoleCategoryVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Asset.Core.Repositories
{
    public class RoleCategoryRepositories : IRoleCategoryRepository
    {
        private ApplicationDbContext _context;
        string msg;

        public RoleCategoryRepositories(ApplicationDbContext context)
        {
            _context = context;
        }


        public RoleCategory GetById(int id)
        {
            var roleCategoryObj = _context.RoleCategories.Find(id);

            if (roleCategoryObj == null)
            {
                return null;
            }
            return roleCategoryObj;
        }




        public IEnumerable<IndexCategoryVM.GetData> GetAll()
        {
            List<IndexCategoryVM.GetData> lstRoleCategoriesVM = new List<IndexCategoryVM.GetData>();
            var lstRoleCategories = _context.RoleCategories.OrderBy(a=>a.OrderId).ToList().Select(item => new IndexCategoryVM.GetData
            {
                Id = item.Id,
                Name = item.Name,
                NameAr = item.NameAr
            });

            return lstRoleCategories;
        }

        public int Add(CreateRoleCategory roleCategory)
        {
            RoleCategory roleCategoryObj = new RoleCategory();
            try
            {

                if (roleCategory != null)
                {
                    roleCategoryObj.Name = roleCategory.Name;
                    roleCategoryObj.NameAr = roleCategory.NameAr;
                    _context.RoleCategories.Add(roleCategoryObj);
                    _context.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return roleCategoryObj.Id;
        }

        public int Delete(RoleCategory roleCategory)
        {
            var employee = _context.RoleCategories.Find(roleCategory.Id);
            try
            {
                if (employee != null)
                {
                    _context.RoleCategories.Remove(roleCategory);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return 0;
        }

        public int Update(EditRoleCategory roleCategory)
        {
            try
            {
                var roleCategoryObj = _context.RoleCategories.Find(roleCategory.Id);
                roleCategoryObj.Id = roleCategory.Id;
                roleCategoryObj.Name = roleCategory.Name;
                roleCategoryObj.NameAr = roleCategory.NameAr;
                _context.Entry(roleCategoryObj).State = EntityState.Modified;
                _context.SaveChanges();
                return roleCategory.Id;


            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return 0;
        }
    }
}