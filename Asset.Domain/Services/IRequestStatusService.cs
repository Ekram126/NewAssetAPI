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
        IEnumerable<IndexRequestStatusVM> GetAllRequestStatus();
        IndexRequestStatusVM GetById(int id);
        int Add(RequestStatus createRequestVM);
        int Update(IndexRequestStatusVM editRequestVM);
        int Delete(int id);
    }
}
