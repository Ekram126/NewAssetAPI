using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.MasterAssetAttachmentVM;
using Asset.ViewModels.MasterAssetComponentVM;
using Asset.ViewModels.MasterAssetVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class MasterAssetComponentRepositories : IMasterAssetComponentRepository
    {

        private ApplicationDbContext _context;


        public MasterAssetComponentRepositories(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Add(CreateMasterAssetComponentVM model)
        {
            MasterAssetComponent componentObj = new MasterAssetComponent();
            try
            {
                if (model != null)
                {
                    componentObj.Code = model.CompCode;
                    componentObj.Name = model.CompName;
                    componentObj.NameAr = model.CompNameAr;
                    componentObj.Description = model.CompDescription;
                    componentObj.DescriptionAr = model.CompDescriptionAr;
                    componentObj.Price = model.Price;
                    componentObj.PartNo = model.PartNo;
                    componentObj.MasterAssetId = model.MasterAssetId;
                    _context.MasterAssetComponents.Add(componentObj);
                    _context.SaveChanges();
                    return componentObj.Id;
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return 0;
        }



        public int Delete(int id)
        {
            var masterAssetObj = _context.MasterAssetComponents.Find(id);
            try
            {
                if (masterAssetObj != null)
                {
                    _context.MasterAssetComponents.Remove(masterAssetObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return 0;
        }

        public IEnumerable<IndexMasterAssetComponentVM.GetData> GetAll()
        {
            return _context.MasterAssetComponents.Include(a => a.MasterAsset).ToList().Select(item => new IndexMasterAssetComponentVM.GetData
            {
                Id = item.Id,
                Name = item.Name,
                NameAr = item.NameAr,
                Code = item.Code,
                AssetName = item.MasterAsset.Name,
                AssetNameAr = item.MasterAsset.NameAr,
                PartNo = item.PartNo,
                Price = item.Price.ToString()
            }).ToList();

        }

        public EditMasterAssetComponentVM GetById(int id)
        {
            return _context.MasterAssetComponents.Where(a => a.Id == id).Select(item => new EditMasterAssetComponentVM
            {
                Id = item.Id,
                Name = item.Name,
                NameAr = item.NameAr,
                Code = item.Code,
                Description = item.Description,
                DescriptionAr = item.DescriptionAr,
                Price = item.Price,
                PartNo = item.PartNo,
                MasterAssetId = item.MasterAssetId
            }).First();
        }

        public IEnumerable<IndexMasterAssetComponentVM.GetData> GetMasterAssetComponentByMasterAssetId(int masterAssetId)
        {
            return _context.MasterAssetComponents.Include(a => a.MasterAsset).Where(a => a.MasterAssetId == masterAssetId).ToList().Select(item => new IndexMasterAssetComponentVM.GetData
            {
                Id = item.Id,
                Name = item.Name,
                NameAr = item.NameAr,
                Code = item.Code,
                AssetName = item.MasterAsset.Name,
                AssetNameAr = item.MasterAsset.NameAr,
                PartNo = item.PartNo,
                Price = item.Price.ToString()
            }).ToList();
        }

        public int Update(EditMasterAssetComponentVM model)
        {
            try
            {

                var masterAssetObj = _context.MasterAssetComponents.Find(model.Id);
                masterAssetObj.Id = model.Id;
                masterAssetObj.Code = model.Code;
                masterAssetObj.Name = model.Name;
                masterAssetObj.NameAr = model.NameAr;
                masterAssetObj.Description = model.Description;
                masterAssetObj.DescriptionAr = model.DescriptionAr;
                masterAssetObj.PartNo = model.PartNo;
                masterAssetObj.Price = model.Price;
                _context.Entry(masterAssetObj).State = EntityState.Modified;
                _context.SaveChanges();
                return masterAssetObj.Id;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return 0;
        }

        public ViewMasterAssetComponentVM ViewMasterAsset(int id)
        {
            ViewMasterAssetComponentVM model = new ViewMasterAssetComponentVM();
            var masterAssetObj = _context.MasterAssetComponents.Find(id);
            model.Id = masterAssetObj.Id;
            model.Code = masterAssetObj.Code;
            model.Name = masterAssetObj.Name;
            model.NameAr = masterAssetObj.NameAr;
            model.Description = masterAssetObj.Description;
            model.DescriptionAr = masterAssetObj.DescriptionAr;

            model.PartNo = masterAssetObj.PartNo;
            model.Price = masterAssetObj.Price.ToString();


            return model;
        }

        public ViewMasterAssetComponentVM ViewMasterAssetComponent(int id)
        {
            return _context.MasterAssetComponents.Where(a => a.Id == id).Select(item => new ViewMasterAssetComponentVM
            {
                Id = item.Id,
                Name = item.Name,
                NameAr = item.NameAr,
                Code = item.Code,
                AssetName = item.MasterAsset.Name,
                AssetNameAr = item.MasterAsset.NameAr,
                PartNo = item.PartNo,
                Price = item.Price.ToString()
            }).FirstOrDefault();
        }
    }
}
