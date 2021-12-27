using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.CategoryTypeVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class CategoryTypeRepositories : ICategoryTypeRepository
    {

        private ApplicationDbContext _context;


        public CategoryTypeRepositories(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Add(CreateCategoryTypeVM model)
        {
            CategoryType categoryTypeObj = new CategoryType();
            try
            {
                if (model != null)
                {
                    categoryTypeObj.Code = model.Code;
                    categoryTypeObj.Name = model.Name;
                    categoryTypeObj.NameAr = model.NameAr;
                    _context.CategoryTypes.Add(categoryTypeObj);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return categoryTypeObj.Id;
        }

        public int Delete(int id)
        {
            var categoryTypeObj = _context.CategoryTypes.Find(id);
            try
            {
                if (categoryTypeObj != null)
                {
                    _context.CategoryTypes.Remove(categoryTypeObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }

        public IEnumerable<IndexCategoryTypeVM.GetData> GetAll()
        {
            return _context.CategoryTypes.ToList().Select(item => new IndexCategoryTypeVM.GetData
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                NameAr = item.NameAr
            });
        }

    

        public EditCategoryTypeVM GetById(int id)
        {
            return _context.CategoryTypes.Where(a => a.Id == id).Select(item => new EditCategoryTypeVM
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                NameAr = item.NameAr
            }).FirstOrDefault();
        }

    

        public int Update(EditCategoryTypeVM model)
        {
            try
            {
                var categoryObj = _context.CategoryTypes.Find(model.Id);
                categoryObj.Id = model.Id;
                categoryObj.Code = model.Code;
                categoryObj.Name = model.Name;
                categoryObj.NameAr = model.NameAr;
                _context.Entry(categoryObj).State = EntityState.Modified;
                _context.SaveChanges();
                return categoryObj.Id;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }
    }
}
