using Asset.ViewModels.ProblemVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Repositories
{
    public interface IProblemRepository
    {
        IEnumerable<IndexProblemVM> GetAll();
        IndexProblemVM GetById(int id);
        void Add(CreateProblemVM createProblemVM);
        void Update(int id, EditProblemVM editProblemVM);
        void Delete(int id);
    }
}
