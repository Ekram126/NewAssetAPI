﻿using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.ECRIVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class ECRIRepositories : IECRIRepository
    {

        private ApplicationDbContext _context;


        public ECRIRepositories(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Add(CreateECRIVM model)
        {
            ECRI ECRIObj = new ECRI();
            try
            {
                if (model != null)
                {
                    ECRIObj.Code = model.Code;
                    ECRIObj.Name = model.Name;
                    ECRIObj.NameAr = model.NameAr;
                    _context.ECRIS.Add(ECRIObj);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return ECRIObj.Id;
        }

        public int Delete(int id)
        {
            var ECRIObj = _context.ECRIS.Find(id);
            try
            {
                if (ECRIObj != null)
                {
                    _context.ECRIS.Remove(ECRIObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }

        public IEnumerable<IndexECRIVM.GetData> GetAll()
        {
            return _context.ECRIS.ToList().Select(item => new IndexECRIVM.GetData
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                NameAr = item.NameAr
            });
        }

        public IEnumerable<ECRI> GetAllECRIs()
        {
            return _context.ECRIS.ToList();
        }


        public EditECRIVM GetById(int id)
        {
            return _context.ECRIS.Where(a => a.Id == id).Select(item => new EditECRIVM
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                NameAr = item.NameAr
            }).First();
        }

        public IEnumerable<IndexECRIVM.GetData> GetECRIByName(string ECRIName)
        {
            return _context.ECRIS.Where(a => a.Name == ECRIName || a.NameAr == ECRIName).ToList().Select(item => new IndexECRIVM.GetData
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                NameAr = item.NameAr
            });
        }

        public int Update(EditECRIVM model)
        {
            try
            {
                var ECRIObj = _context.ECRIS.Find(model.Id);
                ECRIObj.Id = model.Id;
                ECRIObj.Code = model.Code;
                ECRIObj.Name = model.Name;
                ECRIObj.NameAr = model.NameAr;
                _context.Entry(ECRIObj).State = EntityState.Modified;
                _context.SaveChanges();
                return ECRIObj.Id;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }
    }
}
