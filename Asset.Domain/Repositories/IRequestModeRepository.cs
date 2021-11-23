using Asset.Models;
using Asset.ViewModels.BrandVM;
using Asset.ViewModels.RequestModeVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Repositories
{
   public interface IRequestModeRepository
    {
        IEnumerable<IndexRequestMode> GetAll();
        IndexRequestMode GetById(int id);
        void Add(CreateRequestMode createRequestMode);
        void Update(int id, EditRequestMode editRequestMode);
        void Delete(int id);
    }
}
