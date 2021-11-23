using Asset.Domain;
using Asset.Domain.Services;
using Asset.ViewModels.PagingParameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
    public class PagingService: IPagingService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PagingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Count<T>() where T : class
        {
            return _unitOfWork.pagingRepository.Count<T>();
        }

        public IEnumerable<T> GetAll<T>(PagingParameter pageInfo, List<T> objList) where T:class
        {
            return _unitOfWork.pagingRepository.GetAll<T>(pageInfo, objList);
        }
    }
}
