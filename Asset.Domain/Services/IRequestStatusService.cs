using Asset.Models;
using Asset.ViewModels.RequestStatusVM;
using Asset.ViewModels.RequestVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
    public interface IRequestStatusService
    {
        IEnumerable<IndexRequestStatusVM.GetData> GetAllRequestStatus();
        IndexRequestStatusVM.GetData GetAllForReport();
        IndexRequestStatusVM.GetData GetAllForReport(SearchRequestDateVM requestDateObj);
        IndexRequestStatusVM.GetData GetAllByHospitalId(string userId, int hospitalId);
        IndexRequestStatusVM.GetData GetAll(string userId);
        RequestStatus GetById(int id);
        int Add(RequestStatus createRequestVM);
        int Update(RequestStatus editRequestVM);
        int Delete(int id);


        IEnumerable<IndexRequestStatusVM.GetData> SortRequestStatuses(SortRequestStatusVM sortObj);
    }
}
