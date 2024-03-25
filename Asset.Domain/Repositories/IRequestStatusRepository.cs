using Asset.Models;
using Asset.ViewModels.RequestStatusVM;
using Asset.ViewModels.RequestVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Repositories
{
    public interface IRequestStatusRepository
    {
        IEnumerable<IndexRequestStatusVM.GetData> GetAll();
        IndexRequestStatusVM.GetData GetAll(string userId);
        IndexRequestStatusVM.GetData GetAllByHospitalId(string userId,int hospitalId);
        IndexRequestStatusVM.GetData GetAllForReport();

        IndexRequestStatusVM.GetData GetAllForReport(SearchRequestDateVM requestDateObj);


        RequestStatus GetById(int id);
        int Add(RequestStatus createRequestVM);
        int Update(RequestStatus editRequestVM);
        int Delete(int id);


        IndexRequestStatusVM GetRequestStatusByUserId(string userId, int hospitalId);


        IEnumerable<IndexRequestStatusVM.GetData> SortRequestStatuses(SortRequestStatusVM sortObj);

    }
}
