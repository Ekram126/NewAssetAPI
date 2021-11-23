using Asset.Domain;
using Asset.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
    public class GroupingService : IGroupingService
    {
        private IUnitOfWork _unitOfWork;

        public GroupingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<T> GetAll<T>(List<T> Assets, string groupItem) where T : class
        {
            return _unitOfWork.groupingRepository.GetAll<T>(Assets, groupItem);
        }
    }
}
