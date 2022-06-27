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
    public class VisitTypeService : IVisitTypeService
    {
        private IUnitOfWork _unitOfWork;

        public VisitTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<VisitType> GetAll()
        {
            return _unitOfWork.visitTypeRepository.GetAll();
        }

        public IndexVisitVM GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
