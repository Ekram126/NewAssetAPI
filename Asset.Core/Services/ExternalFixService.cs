using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.ExternalFixFileVM;
using Asset.ViewModels.ExternalFixVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
    public class ExternalFixService : IExternalFixService
    {
        private IUnitOfWork _unitOfWork;

        public ExternalFixService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        public int Delete(int id)
        {
            _unitOfWork.ExternalFixRepository.Delete(id);


            return 1;
        }

        public IEnumerable<IndexExternalFixVM.GetData> GetAll()
        {
            return _unitOfWork.ExternalFixRepository.GetAll();
        }

        public IndexExternalFixVM GetAllWithPaging(int hospitalId, int pageNumber, int pageSize)
        {
            return _unitOfWork.ExternalFixRepository.GetAllWithPaging(hospitalId, pageNumber, pageSize);
        }
        public int Add(CreateExternalFixVM externalFixObj)
        {
            return _unitOfWork.ExternalFixRepository.Add(externalFixObj);

        }

        public IEnumerable<ExternalFixFile> GetFilesByExternalFixFileId(int externalFixId)
        {
            return _unitOfWork.ExternalFixRepository.GetFilesByExternalFixFileId(externalFixId);
        }

        public int AddExternalFixFile(CreateExternalFixFileVM externalFixFileObj)
        {
            return _unitOfWork.ExternalFixRepository.AddExternalFixFile(externalFixFileObj);
        }

        public GenerateExternalFixNumberVM GenerateExternalFixNumber()
        {
            return _unitOfWork.ExternalFixRepository.GenerateExternalFixNumber();
        }


        public ViewExternalFixVM ViewExternalFixById(int externalFixId)
        {
            return _unitOfWork.ExternalFixRepository.ViewExternalFixById(externalFixId);

        }


        public void Update(EditExternalFixVM editExternalFixVMObj)
        {
            _unitOfWork.ExternalFixRepository.Update(editExternalFixVMObj);
        }

        public IndexExternalFixVM GetAssetsExceed72HoursInExternalFix(int hospitalId, int pageNumber, int pageSize)
        {
            return _unitOfWork.ExternalFixRepository.GetAssetsExceed72HoursInExternalFix(hospitalId, pageNumber, pageSize);
        }

        public IndexExternalFixVM SearchInExternalFix(SearchExternalFixVM searchObj, int pageNumber, int pageSize)
        {
            return _unitOfWork.ExternalFixRepository.SearchInExternalFix(searchObj, pageNumber, pageSize);
        }

        public IndexExternalFixVM SortExternalFix(SortExternalFixVM sortObj, int pageNumber, int pageSize)
        {
            return _unitOfWork.ExternalFixRepository.SortExternalFix(sortObj, pageNumber, pageSize);
        }
    }
}
