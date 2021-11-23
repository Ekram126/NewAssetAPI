using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.AssetPeriorityVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class AssetPeriorityRepositories : IAssetPeriorityRepository
    {

        private ApplicationDbContext _context;


        public AssetPeriorityRepositories(ApplicationDbContext context)
        {
            _context = context;
        }



        public int Add(CreateAssetPeriorityVM model)
        {
            AssetPeriority assetPeriorityObj = new AssetPeriority();
            try
            {
                if (model != null)
                {
                    assetPeriorityObj.Name = model.Name;
                    assetPeriorityObj.NameAr = model.NameAr;
                    _context.AssetPeriorities.Add(assetPeriorityObj);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return assetPeriorityObj.Id;
        }

        public int Delete(int id)
        {
            var assetPeriorityObj = _context.AssetPeriorities.Find(id);
            try
            {
                if (assetPeriorityObj != null)
                {
                    _context.AssetPeriorities.Remove(assetPeriorityObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
              string  msg = ex.Message;
            }

            return 0;
        }

        public IEnumerable<IndexAssetPeriorityVM.GetData> GetAll()
        {
            return _context.AssetPeriorities.ToList().Select(item => new IndexAssetPeriorityVM.GetData
            {
                Id = item.Id,
                Name = item.Name,
                NameAr = item.NameAr
            });
        }

        public EditAssetPeriorityVM GetById(int id)
        {
            return _context.AssetPeriorities.Where(a => a.Id == id).Select(item => new EditAssetPeriorityVM
            {
                Id = item.Id,
                Name = item.Name,
                NameAr = item.NameAr
            }).First();
        }

        public int Update(EditAssetPeriorityVM model)
        {
            try
            {

                var assetPeriorityObj = _context.AssetPeriorities.Find(model.Id);
                assetPeriorityObj.Name = model.Name;
                assetPeriorityObj.NameAr = model.NameAr;
                _context.Entry(assetPeriorityObj).State = EntityState.Modified;
                _context.SaveChanges();
                return assetPeriorityObj.Id;



            }
            catch (Exception ex)
            {
             string   msg = ex.Message;
            }

            return 0;
        }
    }
}
