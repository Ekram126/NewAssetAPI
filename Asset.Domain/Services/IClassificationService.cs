using Asset.Models;
using Asset.ViewModels.ClassificationVM;
using Asset.ViewModels.OriginVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
   public interface IClassificationService
    {
        IEnumerable<Classification> GetAll();
        Classification GetById(int id);
        int Add(Classification classObj);
        int Update(Classification classObj);
        int Delete(int id);

        IEnumerable<Classification> SortClassification(SortClassificationVM sortObj);
    }
}
