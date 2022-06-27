using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.EngineerVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class EngineerRepository : IEngineerRepository
    {
        private ApplicationDbContext _context;


        public EngineerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Add(CreateEngineerVM model)
        {

            try
            {
                if (model != null)
                {
                    Engineer engineerObj = new Engineer();
                    engineerObj.Code = model.Code;
                    engineerObj.Name = model.Name;
                    engineerObj.NameAr = model.NameAr;
                    engineerObj.CardId = model.CardId;
                    engineerObj.Email = model.Email;
                    engineerObj.Address = model.Address;
                    engineerObj.AddressAr = model.AddressAr;
                    engineerObj.Phone = model.Phone;
                    engineerObj.Dob = model.Dob != "" ? DateTime.Parse(model.Dob) : null;
                    engineerObj.WhatsApp = model.WhatsApp;
                    engineerObj.GenderId = model.GenderId;
                    _context.Engineers.Add(engineerObj);
                    _context.SaveChanges();
                    return engineerObj.Id;
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }

            return 0;

        }

        public int Delete(int id)
        {
            var engineerObj = _context.Engineers.Find(id);
            try
            {
                _context.Engineers.Remove(engineerObj);
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }

        public IEnumerable<IndexEngineerVM.GetData> GetAll()
        {

            List<IndexEngineerVM.GetData> list = new List<IndexEngineerVM.GetData>();
            var lstEngineers = _context.Engineers.ToList();
            if (lstEngineers.Count > 0)
            {
                foreach (var item in lstEngineers)
                {
                    IndexEngineerVM.GetData getDataObj = new IndexEngineerVM.GetData();
                    getDataObj.Id = item.Id;
                    getDataObj.Name = item.Name;
                    getDataObj.NameAr = item.NameAr;
                    getDataObj.Email = item.Email;
                    getDataObj.Code = item.Code;
                    getDataObj.GenderId = item.GenderId;
                    getDataObj.WhatsApp = item.WhatsApp;
                    getDataObj.Phone = item.Phone;
                    getDataObj.Address = item.Address;
                    getDataObj.AddressAr = item.AddressAr;
                    getDataObj.CardId = item.CardId;
                    getDataObj.Dob = item.Dob;


                    list.Add(getDataObj);
                }
            }
            return list;
        }

        public Engineer GetById(int id)
        {
            return _context.Engineers.Find(id);

            //return _context.Engineers.Where(a => a.Id == id).Select(item => new Engineer
            //{
            //    Id = item.Id,
            //    Name = item.Name,
            //    NameAr = item.NameAr,
            //    Dob = item.Dob.Value.ToShortDateString(),
            //    Email = item.Email,
            //    Code = item.Code,
            //    GenderId = item.GenderId,
            //    WhatsApp = item.WhatsApp,
            //    Phone = item.Phone,
            //    Address = item.Address,
            //    AddressAr = item.AddressAr,
            //    CardId = item.CardId
            //}).First();
        }

        public int Update(EditEngineerVM model)
        {
            try
            {
                var engineerObj = _context.Engineers.Find(model.Id);
                engineerObj.Id = model.Id;
                engineerObj.Code = model.Code;
                engineerObj.Name = model.Name;
                engineerObj.NameAr = model.NameAr;
                engineerObj.CardId = model.CardId;
                engineerObj.Email = model.Email;
                engineerObj.Address = model.Address;
                engineerObj.AddressAr = model.AddressAr;
                engineerObj.Phone = model.Phone;
                if (model.Dob != null)
                    engineerObj.Dob = DateTime.Parse(model.Dob);
                engineerObj.WhatsApp = model.WhatsApp;
                engineerObj.GenderId = model.GenderId;

                _context.Entry(engineerObj).State = EntityState.Modified;
                _context.SaveChanges();
                return engineerObj.Id;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }

        public IEnumerable<IndexEngineerVM.GetData> SortEngineer(SortEngineerVM sortObj)
        {
            List<IndexEngineerVM.GetData> list = new List<IndexEngineerVM.GetData>();
            var lstEngineers = _context.Engineers.ToList();
            if (lstEngineers.Count > 0)
            {
                foreach (var item in lstEngineers)
                {
                    IndexEngineerVM.GetData getDataObj = new IndexEngineerVM.GetData();
                    getDataObj.Id = item.Id;
                    getDataObj.Name = item.Name;
                    getDataObj.NameAr = item.NameAr;
                    getDataObj.Email = item.Email;
                    getDataObj.Code = item.Code;
                    getDataObj.GenderId = item.GenderId;
                    getDataObj.WhatsApp = item.WhatsApp;
                    getDataObj.Phone = item.Phone;
                    getDataObj.Address = item.Address;
                    getDataObj.AddressAr = item.AddressAr;
                    getDataObj.CardId = item.CardId;

                    list.Add(getDataObj);
                }
                if (sortObj.Code != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.Code).ToList();
                    else
                        list = list.OrderBy(d => d.Code).ToList();
                }
                else if (sortObj.Name != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.Name).ToList();
                    else
                        list = list.OrderBy(d => d.Name).ToList();
                }
                else if (sortObj.NameAr != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.NameAr).ToList();
                    else
                        list = list.OrderBy(d => d.NameAr).ToList();
                }

                else if (sortObj.Email != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.Email).ToList();
                    else
                        list = list.OrderBy(d => d.Email).ToList();
                }
            }
            return list;
        }

        public IEnumerable<Engineer> GetAllEngineers()
        {
            return _context.Engineers.ToList();
        }

        public Engineer GetByEmail(string email)
        {
            return _context.Engineers.Where(a=>a.Email == email).ToList().FirstOrDefault();
        }
    }
}
