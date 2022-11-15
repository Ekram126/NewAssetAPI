using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.ScrapVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
    public class ScrapReasonService : IScrapReasonService
    {
        private IUnitOfWork _unitOfWork;

        public ScrapReasonService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public List<ScrapReason> GetAll()
        {
            return _unitOfWork.scrapReasonRepository.GetAll();
        }

        public IndexScrapVM GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
