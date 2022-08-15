using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.VisitVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
    public class VisitService : IVisitService
    {
        private IUnitOfWork _unitOfWork;

        public VisitService (IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Add(CreateVisitVM createVisitVM)
        {
            return _unitOfWork.visitRepository.Add(createVisitVM);
        }

        public int Update(EditVisitVM editVisitVM)
        {
            return _unitOfWork.visitRepository.Update(editVisitVM);
        }
        public int Delete(int id)
        {
            var visitObj = _unitOfWork.visitRepository.GetById(id);
            _unitOfWork.visitRepository.Delete(id);
            _unitOfWork.CommitAsync();

            return visitObj.Id;
     
        }

        public List<IndexVisitVM.GetData> GetAll()
        {
            return _unitOfWork.visitRepository.GetAll();
        }

        public Visit GetById(int id)
        {
            return _unitOfWork.visitRepository.GetById(id);
        }

        public IEnumerable<IndexVisitVM.GetData> SearchInVisits(SearchVisitVM searchObj)
        {
            return _unitOfWork.visitRepository.SearchInVisits(searchObj);
        }

        public IEnumerable<IndexVisitVM.GetData> SortVisits(SortVisitVM sortObj, int statusId)
        {
            return _unitOfWork.visitRepository.SortVisits(sortObj, statusId);
        }

        public ViewVisitVM ViewVisitById(int id)
        {
            return _unitOfWork.visitRepository.ViewVisitById(id);
        }

        public int CreateVisitAttachments(VisitAttachment attachObj)
        {
            return _unitOfWork.visitRepository.CreateVisitAttachments(attachObj);
        }

        public IEnumerable<VisitAttachment> GetVisitAttachmentByVisitId(int visitId)
        {
            return _unitOfWork.visitRepository.GetVisitAttachmentByVisitId(visitId);
        }

        public int UpdateVer(EditVisitVM editVisitVM)
        {
            return _unitOfWork.visitRepository.UpdateVer(editVisitVM);
        }

        public GeneratedVisitCodeVM GenerateVisitCode()
        {
            return _unitOfWork.visitRepository.GenerateVisitCode();
        }
    }
}
