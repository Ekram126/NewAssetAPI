using Asset.ViewModels.ProblemVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
    public interface IProblemService
    {
        IEnumerable<IndexProblemVM> GetAllProblems();
        IndexProblemVM GetProblemById(int id);
        void AddProblem(CreateProblemVM createProblemVM);
        void UpdateProblem(int id, EditProblemVM editProblemVM);
        void DeleteProblem(int id);
    }
}
