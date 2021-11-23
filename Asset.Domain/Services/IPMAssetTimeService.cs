using Asset.Models;
using Asset.ViewModels.OriginVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
   public interface IPMAssetTimeService
    {
        IEnumerable<PMAssetTime> GetAll();

        IEnumerable<PMAssetTime> GetDateByAssetDetailId(int assetDetailId);




        PMAssetTime GetById(int id);
        int Add(PMAssetTime timeObj);
        int Update(PMAssetTime timeObj);
        int Delete(int id);
    }
}
