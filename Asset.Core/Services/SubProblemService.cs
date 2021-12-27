using Asset.Domain;
using Asset.Domain.Services;
using Asset.ViewModels.SubProblemVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
    public class SubProblemService : ISubProblemService
    {
        private IUnitOfWork _unitOfWork;

        public SubProblemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void AddSubProblem(CreateSubProblemVM createSubProblemVM)
        {
            _unitOfWork.SubProblem.Add(createSubProblemVM);
        }

        public void DeleteSubProblem(int id)
        {
            _unitOfWork.SubProblem.Delete(id);
        }

        public IEnumerable<IndexSubProblemVM> GetAllSubProblems()
        {
            return _unitOfWork.SubProblem.GetAll();
        }

        public IEnumerable<IndexSubProblemVM> GetAllSubProblemsByProblemId(int ProblemId)
        {
            return _unitOfWork.SubProblem.GetAllSubProblemsByProblemId(ProblemId);
        }

        public IndexSubProblemVM GetSubProblemById(int id)
        {
            return _unitOfWork.SubProblem.GetById(id);
        }

        public void UpdateSubProblem(EditSubProblemVM editSubProblemVM)
        {
            _unitOfWork.SubProblem.Update(editSubProblemVM);
        }
    }
}
