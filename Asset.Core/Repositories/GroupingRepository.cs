using Asset.Domain.Repositories;
using Asset.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class GroupingRepository : IGroupingRepository
    {
        private readonly ApplicationDbContext _context;
        public GroupingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<T> GetAll<T>(List<T> Assets, string groupItem) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
