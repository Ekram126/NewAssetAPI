using Asset.Models;
using Asset.ViewModels.ScrapVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
    public interface IScrapReasonService
    {
        List<ScrapReason> GetAll();
        IndexScrapVM GetById(int id);
    }
}
