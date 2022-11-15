using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Asset.Models;
using Asset.ViewModels.ScrapVM;


namespace Asset.Domain.Repositories
{
    public interface IScrapReasonRepository
    {
        List<ScrapReason> GetAll();
        IndexScrapVM GetById(int id);
    }
}
