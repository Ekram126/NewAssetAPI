using Asset.Domain.Repositories;
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

        public IEnumerable<IndexBrandVM.GetData> GetTop10Brands(int hospitalId)
        {
            if (hospitalId != 0)
            {
                return _context.AssetDetails.Include(a=>a.MasterAsset.brand).Include(a => a.Hospital).Where(a=>a.HospitalId == hospitalId)
                    .ToList().Take(10).GroupBy(a => a.MasterAsset.BrandId).Select(item => new IndexBrandVM.GetData
                {
                    Id = item.FirstOrDefault().MasterAsset.brand.Id,
                    Code = item.FirstOrDefault().MasterAsset.brand.Code,
                    Name = item.FirstOrDefault().MasterAsset.brand.Name,
                    NameAr = item.FirstOrDefault().MasterAsset.brand.NameAr
                });
            }
            else
            {
                return _context.Brands.Take(10).ToList().Select(item => new IndexBrandVM.GetData
                {
                    Id = item.Id,
                    Code = item.Code,
                    Name = item.Name,
                    NameAr = item.NameAr
                });
            }
        }

        public IEnumerable<IndexBrandVM.GetData> SortBrands(SortBrandVM sortObj)
        {
            var lstBrands = GetAll().ToList();
            if (sortObj.Code != "")
            {
                if (sortObj.SortStatus == "descending")
                    lstBrands = lstBrands.OrderByDescending(d => d.Code).ToList();
                else
                    lstBrands = lstBrands.OrderBy(d => d.Code).ToList();
            }
            else if (sortObj.Name != "")
            {
                if (sortObj.SortStatus == "descending")
                    lstBrands = lstBrands.OrderByDescending(d => d.Name).ToList();
                else
                    lstBrands = lstBrands.OrderBy(d => d.Name).ToList();
            }

            else if (sortObj.NameAr != "")
            {
                if (sortObj.SortStatus == "descending")
                    lstBrands = lstBrands.OrderByDescending(d => d.NameAr).ToList();
                else
                    lstBrands = lstBrands.OrderBy(d => d.NameAr).ToList();
            }

            return lstBrands;
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
