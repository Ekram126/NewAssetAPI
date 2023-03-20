using Asset.Models;
using Asset.ViewModels.BrandVM;
using Asset.ViewModels.ExternalFixFileVM;
using Asset.ViewModels.ExternalFixVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
  public  interface IExternalFixService
    {
        IEnumerable<IndexExternalFixVM.GetData> GetAll();
        IndexExternalFixVM GetAllWithPaging(int hospitalId, int pageNumber, int pageSize);
        int Add(CreateExternalFixVM externalFixObj);
        int Delete(int id);
        GenerateExternalFixNumberVM GenerateExternalFixNumber();
        ViewExternalFixVM ViewExternalFixById(int externalFixId);
        IEnumerable<ExternalFixFile> GetFilesByExternalFixFileId(int externalFixId);
        int AddExternalFixFile(CreateExternalFixFileVM externalFixFileObj);
        void Update(EditExternalFixVM editExternalFixVMObj);
        IndexExternalFixVM GetAssetsExceed72HoursInExternalFix(int hospitalId, int pageNumber, int pageSize);
        IndexExternalFixVM SearchInExternalFix(SearchExternalFixVM searchObj, int pageNumber, int pageSize);
        IndexExternalFixVM SortExternalFix(SortExternalFixVM sortObj, int pageNumber, int pageSize);
    }
}
