using Asset.Domain.Repositories;
using Asset.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class AssetOwnerRepositories : IAssetOwnerRepository
    {

        private ApplicationDbContext _context;


        public AssetOwnerRepositories(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<AssetOwner> GetAll()
        {
            return _context.AssetOwners.ToList();
        }
        public int Delete(int id)
        {
            var AssetOwnerObj = _context.AssetOwners.Find(id);
            try
            {
                if (AssetOwnerObj != null)
                {
                    _context.AssetOwners.Remove(AssetOwnerObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }

        public List<AssetOwner> GetOwnersByAssetDetailId(int assetDetailId)
        {
          return  _context.AssetOwners.Where(a => a.AssetDetailId == assetDetailId).ToList();
        }
    }
}
