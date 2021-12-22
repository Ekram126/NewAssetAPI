﻿using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.BrandVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class BrandRepositories : IBrandRepository
    {
        private ApplicationDbContext _context;

        public BrandRepositories(ApplicationDbContext context)
        {
            _context = context;
        }
        public int Add(CreateBrandVM model)
        {
            Brand brandObj = new Brand();
            try
            {
                if (model != null)
                {
                    brandObj.Code = model.Code;
                    brandObj.Name = model.Name;
                    brandObj.NameAr = model.NameAr;
                    _context.Brands.Add(brandObj);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return brandObj.Id;
        }

        public int CountBrands()
        {
            return _context.Brands.Count();
        }

        public int Delete(int id)
        {
            var brandObj = _context.Brands.Find(id);
            try
            {
                if (brandObj != null)
                {
                    _context.Brands.Remove(brandObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }

        public IEnumerable<IndexBrandVM.GetData> GetAll()
        {
            return _context.Brands.ToList().Select(item => new IndexBrandVM.GetData
            {
                Id = item.Id,
                Code= item.Code,
                Name = item.Name,
                NameAr = item.NameAr
            });
        }

        public IEnumerable<Brand> GetAllBrands()
        {
            return _context.Brands.ToList();
        }

        public IEnumerable<IndexBrandVM.GetData> GetBrandByName(string brandName)
        {
            return _context.Brands.Where(a => (a.Name == brandName || a.NameAr == brandName)).ToList().Select(item => new IndexBrandVM.GetData
            {

                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                NameAr = item.NameAr
            });
        }

        public EditBrandVM GetById(int id)
        {
            return _context.Brands.Where(a => a.Id == id).Select(item => new EditBrandVM
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                NameAr = item.NameAr
            }).First();
        }

        public IEnumerable<IndexBrandVM.GetData> GetTop10Brands()
        {
            return _context.Brands.Take(10).ToList().Select(item => new IndexBrandVM.GetData
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                NameAr = item.NameAr
            });
        }

        public int Update(EditBrandVM model)
        {
            try
            {
                var brandObj = _context.Brands.Find(model.Id);
                brandObj.Code = model.Code;
                brandObj.Name = model.Name;
                brandObj.NameAr = model.NameAr;
                _context.Entry(brandObj).State = EntityState.Modified;
                _context.SaveChanges();
                return brandObj.Id;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return 0;
        }
    }
}
