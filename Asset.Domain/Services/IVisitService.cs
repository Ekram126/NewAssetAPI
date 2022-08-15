using Asset.Models;
using Asset.ViewModels.VisitVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
    public interface IVisitService
    {
        List<IndexVisitVM.GetData> GetAll();
        Visit GetById(int id);
        ViewVisitVM ViewVisitById(int id);
        IEnumerable<IndexVisitVM.GetData> SearchInVisits(SearchVisitVM searchObj);
        IEnumerable<IndexVisitVM.GetData> SortVisits(SortVisitVM sortObj, int statusId);
        int Add(CreateVisitVM createVisitVM);
        int Delete(int id);
        int Update(EditVisitVM editVisitVM); 
        int UpdateVer(EditVisitVM editVisitVM);
        public int CreateVisitAttachments(VisitAttachment attachObj);
        public IEnumerable<VisitAttachment> GetVisitAttachmentByVisitId(int visitId);


        GeneratedVisitCodeVM GenerateVisitCode();
    }
}
