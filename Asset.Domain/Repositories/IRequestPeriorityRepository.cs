using Asset.Models;
using Asset.ViewModels.BrandVM;
using Asset.ViewModels.RequestPeriorityVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Repositories
{
    public interface IRequestPeriorityRepository
    {
        IEnumerable<IndexRequestPeriority> GetAll();
        IndexRequestPeriority GetById(int id);


        IEnumerable<IndexRequestPeriority> GetByPeriorityName(string name);


        void Add(CreateRequestPeriority createRequestPeriority);
        void Update(int id, EditRequestPeriority editRequestPeriority);
        void Delete(int id);
    }
}
