using Asset.Models;
using Asset.ViewModels.ScrapVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
    public interface IScrapService
    {
        List<IndexScrapVM.GetData> GetAll();
        IndexScrapVM GetAllScraps(int pageNumber, int pageSize);

        Scrap GetById(int id);
        IndexScrapVM.GetData GetById2(int id);
        ViewScrapVM ViewScrapById(int id);
        List<ViewScrapVM> GetScrapReasonByScrapId(int scrapId);
        IEnumerable<IndexScrapVM.GetData> SearchInScraps(SearchScrapVM searchObj);
        IndexScrapVM SearchInScraps(SearchScrapVM searchObj, int pageNumber, int pageSize);
        IEnumerable<IndexScrapVM.GetData> SortScraps(SortScrapVM sortObj, int statusId);
        int Add(CreateScrapVM createScrapVM);
        int Delete(int id);
        public int CreateScrapAttachments(ScrapAttachment attachObj);
        GeneratedScrapNumberVM GenerateScrapNumber();
        public IEnumerable<ScrapAttachment> GetScrapAttachmentByScrapId(int scrapId);


        List<IndexScrapVM.GetData> PrintListOfScraps(List<IndexScrapVM.GetData> scraps);
    }
}
