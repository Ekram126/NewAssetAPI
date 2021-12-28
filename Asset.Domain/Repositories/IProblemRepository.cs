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
        IEnumerable<IndexProblemVM> GetProblemsByMasterAssetId(int masterAssetId);
        IEnumerable<IndexProblemVM> GetProblemBySubProblemId(int subProblemId);
        IndexProblemVM GetById(int id);
        void Add(CreateProblemVM createProblemVM);
        void Update(EditProblemVM editProblemVM);
        void Delete(int id);
    }
}
