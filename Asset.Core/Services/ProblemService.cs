using Asset.Domain;
using Asset.Domain.Services;
using Asset.ViewModels.ProblemVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
    public class ProblemService : IProblemService
    {
        private IUnitOfWork _unitOfWork;

        public ProblemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void AddProblem(CreateProblemVM createProblemVM)
        {
            _unitOfWork.Problem.Add(createProblemVM);
        }

        public void DeleteProblem(int id)
        {
            _unitOfWork.Problem.Delete(id);
        }

        public IEnumerable<IndexProblemVM> GetAllProblems()
        {
            return _unitOfWork.Problem.GetAll();
        }

        public IndexProblemVM GetProblemById(int id)
        {
            return _unitOfWork.Problem.GetById(id);
        }

        public IEnumerable<IndexProblemVM> GetProblemBySubProblemId(int subProblemId)
        {
            return _unitOfWork.Problem.GetProblemBySubProblemId(subProblemId);
        }

        public IEnumerable<IndexProblemVM> GetProblemsByMasterAssetId(int masterAssetId)
        {
            return _unitOfWork.Problem.GetProblemsByMasterAssetId(masterAssetId);
        }

        public void UpdateProblem(EditProblemVM editProblemVM)
        {
            _unitOfWork.Problem.Update( editProblemVM);
        }
    }
}
