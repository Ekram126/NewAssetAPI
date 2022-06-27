using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.VisitVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class VisitTypeRepository : IVisitTypeRepository
    {

        private ApplicationDbContext _context;
        public VisitTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<VisitType> GetAll()
        {
           // List<IndexVisitVM.GetData> list = new List<IndexVisitVM.GetData>();
           return _context.VisitTypes.ToList();
            //foreach (var item in lstVisitTypes)
            //{
            //    IndexVisitVM.GetData visitTypeObj = new IndexVisitVM.GetData();
            //    visitTypeObj.Id = item.Id;
            //    visitTypeObj.VisitTypeName = item.Name;
            //    visitTypeObj.VisitTypeNameAr = item.NameAr;
            //    list.Add(visitTypeObj);
            //}

            //return list;
        }
    

        public IndexVisitVM GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
