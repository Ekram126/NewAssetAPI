using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.RequestStatusVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class HospitalSupplierStatusRepository : IHospitalSupplierStatusRepository
    {
        private ApplicationDbContext _context;

        public HospitalSupplierStatusRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<HospitalSupplierStatus> GetAll()
        {
            return _context.HospitalSupplierStatuses.ToList();
        }

  
        public HospitalSupplierStatus GetById(int id)
        {
            return _context.HospitalSupplierStatuses.Find(id);
        }
    }
}
