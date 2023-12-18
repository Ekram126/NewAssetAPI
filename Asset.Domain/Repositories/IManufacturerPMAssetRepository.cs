using Asset.Models;
using Asset.ViewModels.ManufacturerPMAssetVM;
using Asset.ViewModels.WNPMAssetTimes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Asset.Domain.Repositories
{
    public interface IManufacturerPMAssetRepository
    {
        IndexUnScheduledManfacturerPMAssetVM CreateManfacturerAssetTimes(int pageNumber, int pageSize);
        IndexManfacturerPMAssetVM GetAll(int pageNumber, int pageSize, string userId);
        List<ForCheckManfacturerPMAssetsVM> GetAllForCheck();
        IndexManfacturerPMAssetVM SearchAssetTimes(SearchManfacturerAssetTimeVM searchObj, int pageNumber, int pageSize, string userId);
        List<CalendarManfacturerPMAssetTimeVM> GetAll(int hospitalId, string userId);
        IndexManfacturerPMAssetVM SortManfacturerAssetTimes(SortManfacturerPMAssetTimeVM sortObj, int pageNumber, int pageSize, string userId);
        ViewManfacturerPMAssetTimeVM GetAssetTimeById(int id);
        int Update(ManufacturerPMAsset model);
        ManufacturerPMAsset GetById(int id);
        IndexManfacturerPMAssetVM GetAll(FilterManfacturerTimeVM filterObj, int pageNumber, int pageSize, string userId);

     //   IndexManfacturerPMAssetVM GetAllBrandsByQuarter(FilterAssetTimeVM filterObj, int pageNumber, int pageSize, string userId);
    }
}
