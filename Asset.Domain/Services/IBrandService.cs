using Asset.Models;
using Asset.ViewModels.BrandVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
  public  interface IBrandService
    {
        IEnumerable<IndexBrandVM.GetData> GetAll();
        IEnumerable<IndexBrandVM.GetData> GetTop10Brands(int hospitalId);
        EditBrandVM GetById(int id);
        IEnumerable<Brand> GetAllBrands();
        IEnumerable<IndexBrandVM.GetData> GetBrandByName(string brandName);
        int Add(CreateBrandVM brandObj);
        int Update(EditBrandVM brandObj);
        int Delete(int id);
        int CountBrands();
        IEnumerable<IndexBrandVM.GetData> SortBrands(SortBrandVM sortObj);
        GenerateBrandCodeVM GenerateBrandCode();
    }
}
