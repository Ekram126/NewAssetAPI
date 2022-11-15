using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.ScrapVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class ScrapReasonRepository : IScrapReasonRepository
    {
        private ApplicationDbContext _context;
        public ScrapReasonRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<ScrapReason> GetAll()
        {
            return _context.ScrapReasons.ToList();
        }

        public IndexScrapVM GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
