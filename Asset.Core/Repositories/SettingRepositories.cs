using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.BrandVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class SettingRepositories : ISettingRepository
    {
        private ApplicationDbContext _context;

        public SettingRepositories(ApplicationDbContext context)
        {
            _context = context;
        }
    

        public IEnumerable<Setting> GetAll()
        {
            return _context.Settings.ToList();
        }

    }
}
