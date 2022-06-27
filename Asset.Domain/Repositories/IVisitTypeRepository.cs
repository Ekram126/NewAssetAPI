using Asset.Models;
using Asset.ViewModels.VisitVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Repositories
{
    public interface IVisitTypeRepository
    {
        List<VisitType> GetAll();
        IndexVisitVM GetById(int id);
        void Delete(int id);
    }
}
