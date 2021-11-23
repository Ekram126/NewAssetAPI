using Asset.Models;
using Asset.ViewModels.CommetieeMemberVM;
using System;
using System.Collections.Generic;

namespace Asset.Domain.Repositories
{
    public interface ICommetieeMemberRepository
    {

        IEnumerable<IndexCommetieeMemberVM.GetData> GetAll();
        EditCommetieeMemberVM GetById(int id);
        IEnumerable<CommetieeMember> GetAllCommetieeMembers();
        IEnumerable<IndexCommetieeMemberVM.GetData> GetCommetieeMemberByName(string commetieeMemberName);
        int Add(CreateCommetieeMemberVM commetieeMemberObj);
        int Update(EditCommetieeMemberVM commetieeMemberObj);

        int CountCommetieeMembers();
        int Delete(int id);
    }
}
