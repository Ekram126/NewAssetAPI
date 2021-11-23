using Asset.Models;
using Asset.ViewModels.BrandVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Repositories
{
    public interface IBrandRepository
    {
        IEnumerable<IndexBrandVM.GetData> GetAll();

        IEnumerable<Brand> GetAllBrands();
        IEnumerable<IndexBrandVM.GetData> GetBrandByName(string brandName);
        EditBrandVM GetById(int id);
        int Add(CreateBrandVM brandObj);
        int Update(EditBrandVM brandObj);
        int Delete(int id);

        int CountBrands();
    }
}
