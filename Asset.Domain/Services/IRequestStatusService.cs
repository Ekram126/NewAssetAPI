using Asset.Models;
using Asset.ViewModels.RequestStatusVM;
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

        //  IEnumerable<IndexRequestStatusVM.GetData> GetAll(string userId);
        IndexRequestStatusVM.GetData GetAllByHospitalId(string userId, int hospitalId);
        IndexRequestStatusVM.GetData GetAll(string userId);
        RequestStatus GetById(int id);
        int Add(RequestStatus createRequestVM);
        int Update(RequestStatus editRequestVM);
        int Delete(int id);


        IEnumerable<IndexRequestStatusVM.GetData> SortRequestStatuses(SortRequestStatusVM sortObj);
    }
}
