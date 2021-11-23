using Asset.Domain.Repositories;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.PagingParameter;
using Asset.ViewModels.UserVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class PagingRepository:IPagingRepository
    {
        private readonly ApplicationDbContext _context;
        public PagingRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<T> GetAll<T>(PagingParameter pageInfo,List<T> objList) where T:class
        {
            return objList.Skip((pageInfo.PageNumber - 1) * pageInfo.PageSize)
                .Take(pageInfo.PageSize).ToList();
        }
        public int Count<T>()where T:class
        {
            return _context.Set<T>().Count();
        }
    }
}
