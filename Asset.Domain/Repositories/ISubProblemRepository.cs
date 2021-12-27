using Asset.ViewModels.SubProblemVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Repositories
{
    public interface ISubProblemRepository
    {
        IEnumerable<IndexSubProblemVM> GetAll();
        IEnumerable<IndexSubProblemVM> GetAllSubProblemsByProblemId(int ProblemId);
        IndexSubProblemVM GetById(int id);
        void Add(CreateSubProblemVM createSubProblemVM);
        void Update(EditSubProblemVM editSubProblemVM);
        void Delete(int id);
    }
}
