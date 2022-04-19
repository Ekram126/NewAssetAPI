using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.OriginVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class PMAssetTimeRepositories : IPMAssetTimeRepository
    {

        private ApplicationDbContext _context;


        public PMAssetTimeRepositories(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Add(PMAssetTime model)
        {
            PMAssetTime timeObj = new PMAssetTime();
            try
            {
                if (model != null)
                {
                    timeObj.AssetDetailId = model.AssetDetailId;
                    timeObj.HospitalId = model.HospitalId;
                    timeObj.PMDate = model.PMDate;
                    _context.PMAssetTimes.Add(timeObj);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
              string  msg = ex.Message;
            }
            return timeObj.Id;
        }

        public int Delete(int id)
        {
            var timeObj = _context.PMAssetTimes.Find(id);
            try
            {
                if (timeObj != null)
                {
                    _context.PMAssetTimes.Remove(timeObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
             string   msg = ex.Message;
            }

            return 0;
        }

        public IEnumerable<PMAssetTime> GetAll()
        {
            return _context.PMAssetTimes.ToList();
        }

        public PMAssetTime GetById(int id)
        {
            return _context.PMAssetTimes.Find(id);
        }

        public IEnumerable<PMAssetTime> GetDateByAssetDetailId(int assetDetailId)
        {
            return _context.PMAssetTimes.Where(a=>a.AssetDetailId == assetDetailId).ToList();
        }

        public int Update(PMAssetTime model)
        {
            try
            {
                var timeObj = _context.PMAssetTimes.Find(model.Id);
                timeObj.PMDate = model.PMDate;
                timeObj.HospitalId = model.HospitalId;
                _context.Entry(timeObj).State = EntityState.Modified;
                _context.SaveChanges();
                return timeObj.Id;
            }
            catch (Exception ex)
            {
              string  msg = ex.Message;
            }

            return 0;
        }
    }
}
