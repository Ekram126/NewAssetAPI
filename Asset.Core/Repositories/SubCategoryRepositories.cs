using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.SubCategoryVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class SubCategoryRepositories : ISubCategoryRepository
    {

        private ApplicationDbContext _context;


        public SubCategoryRepositories(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Add(CreateSubCategoryVM model)
        {
            SubCategory subCategoryObj = new SubCategory();
            try
            {
                if (model != null)
                {
                    subCategoryObj.Code = model.Code;
                    subCategoryObj.Name = model.Name;
                    subCategoryObj.NameAr = model.NameAr;
                    subCategoryObj.CategoryId = model.CategoryId;
                    _context.SubCategories.Add(subCategoryObj);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return subCategoryObj.Id;
        }

        public int Delete(int id)
        {
            var subCategoryObj = _context.SubCategories.Find(id);
            try
            {
                if (subCategoryObj != null)
                {
                    _context.SubCategories.Remove(subCategoryObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }

        public IEnumerable<IndexSubCategoryVM.GetData> GetAll()
        {
            return _context.SubCategories.ToList().Select(item => new IndexSubCategoryVM.GetData
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                NameAr = item.NameAr,
                CategoryId = (int)item.CategoryId
            });
        }

        public IEnumerable<SubCategory> GetAllSubCategories()
        {
            return _context.SubCategories.ToList();
        }

        public EditSubCategoryVM GetById(int id)
        {
            return _context.SubCategories.Where(a => a.Id == id).Select(item => new EditSubCategoryVM
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                NameAr = item.NameAr,
                CategoryId = (int)item.CategoryId
            }).First();
        }

        public IEnumerable<IndexSubCategoryVM.GetData> GetSubCategoriesByName(string subCategoryName)
        {
            return _context.SubCategories.Where(a => a.Name == subCategoryName || a.NameAr == subCategoryName).ToList().Select(item => new IndexSubCategoryVM.GetData
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                NameAr = item.NameAr,
                CategoryId = (int)item.CategoryId
            });
        }

        public IEnumerable<SubCategory> GetSubCategoryByCategoryId(int categoryId)
        {
            return _context.SubCategories.Where(a => a.CategoryId == categoryId).ToList();
        }

        public int Update(EditSubCategoryVM model)
        {
            try
            {
                var subCategoryObj = _context.SubCategories.Find(model.Id);
                subCategoryObj.Code = model.Code;
                subCategoryObj.Name = model.Name;
                subCategoryObj.NameAr = model.NameAr;
                subCategoryObj.CategoryId = model.CategoryId;
                _context.Entry(subCategoryObj).State = EntityState.Modified;
                _context.SaveChanges();
                return subCategoryObj.Id;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }
    }
}
