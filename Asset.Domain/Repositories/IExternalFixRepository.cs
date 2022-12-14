using Asset.Models;
using Asset.ViewModels.ExternalFixFileVM;
using Asset.ViewModels.ExternalFixVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Repositories
{
    public interface IExternalFixRepository
    {
        IEnumerable<IndexExternalFixVM.GetData> GetAll();

        IndexExternalFixVM GetAllWithPaging(int hospitalId,int pageNumber,int pageSize);

        // EditExternalFixVM GetById(int id);
        int Add(CreateExternalFixVM createExternalFixObj);
        int Delete(int id);
        object GetById(int id);


        GenerateExternalFixNumberVM GenerateExternalFixNumber();
        IEnumerable<ExternalFixFile> GetFilesByExternalFixFileId(int externalFixId);
        ViewExternalFixVM ViewExternalFixById(int externalFixId);
        int AddExternalFixFile(CreateExternalFixFileVM externalFixFileObj);
        void Update(EditExternalFixVM editExternalFixVMObj);
        
    }
}
