using Asset.ViewModels.ScrapVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Asset.Models;


namespace Asset.Domain.Repositories
{
    public interface IScrapRepository
    {
        List<IndexScrapVM.GetData> GetAll();
        Scrap GetById(int id);
        ViewScrapVM ViewScrapById(int id);
        IEnumerable<IndexScrapVM.GetData> SearchInScraps(SearchScrapVM searchObj);

        List<ViewScrapVM> GetScrapReasonByScrapId(int scrapId);
        IEnumerable<IndexScrapVM.GetData> SortScraps(SortScrapVM sortObj, int statusId);
        int Add(CreateScrapVM createVisitVM);
        int Delete(int id);
        public int CreateScrapAttachments(ScrapAttachment attachObj);
        GeneratedScrapNumberVM GenerateScrapNumber();
        public IEnumerable<ScrapAttachment> GetScrapAttachmentByScrapId(int visitId);
    }
}
