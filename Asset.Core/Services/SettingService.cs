using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.BrandVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
    public class SettingService : ISettingService
    {
        private IUnitOfWork _unitOfWork;

        public SettingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IEnumerable<Setting> GetAll()
        {
            return _unitOfWork.SettingRepository.GetAll();
        }

        
    }
}
