using Asset.ViewModels.SubProblemVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
    public interface ISubProblemService
    {
        IEnumerable<IndexSubProblemVM> GetAllSubProblems();
        IEnumerable<IndexSubProblemVM> GetAllSubProblemsByProblemId(int ProblemId);
        IndexSubProblemVM GetSubProblemById(int id);
        void AddSubProblem(CreateSubProblemVM createSubProblemVM);
        void UpdateSubProblem(EditSubProblemVM editSubProblemVM);
        void DeleteSubProblem(int id);
    }
}
