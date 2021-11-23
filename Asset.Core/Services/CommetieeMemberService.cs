using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.CommetieeMemberVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
   public class CommetieeMemberService : ICommetieeMemberService
    {
        private IUnitOfWork _unitOfWork;

        public CommetieeMemberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Add(CreateCommetieeMemberVM CommetieeMemberObj)
        {
            _unitOfWork.CommetieeMemberRepository.Add(CommetieeMemberObj);
            return _unitOfWork.CommitAsync();
        }

        public int CountCommetieeMembers()
        {
            return _unitOfWork.CommetieeMemberRepository.CountCommetieeMembers();
        }

        public int Delete(int id)
        {
            var CommetieeMemberObj = _unitOfWork.CommetieeMemberRepository.GetById(id);
            _unitOfWork.CommetieeMemberRepository.Delete(CommetieeMemberObj.Id);
            _unitOfWork.CommitAsync();
            return CommetieeMemberObj.Id;
        }

        public IEnumerable<IndexCommetieeMemberVM.GetData> GetAll()
        {
            return _unitOfWork.CommetieeMemberRepository.GetAll();
        }

        public IEnumerable<CommetieeMember> GetAllCommetieeMembers()
        {
            return _unitOfWork.CommetieeMemberRepository.GetAllCommetieeMembers();
        }

        public EditCommetieeMemberVM GetById(int id)
        {
            return _unitOfWork.CommetieeMemberRepository.GetById(id);
        }

        public IEnumerable<IndexCommetieeMemberVM.GetData> GetCommetieeMemberByName(string CommetieeMemberName)
        {
            return _unitOfWork.CommetieeMemberRepository.GetCommetieeMemberByName(CommetieeMemberName);
        }

        public int Update(EditCommetieeMemberVM CommetieeMemberObj)
        {
            _unitOfWork.CommetieeMemberRepository.Update(CommetieeMemberObj);
            _unitOfWork.CommitAsync();
            return CommetieeMemberObj.Id;
        }
    }
}
