using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.ClassificationVM;
using Asset.ViewModels.OriginVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class ClassificationRepositories : IClassificationRepository
    {

        private ApplicationDbContext _context;


        public ClassificationRepositories(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Add(Classification model)
        {
            Classification classObj = new Classification();
            try
            {
                if (model != null)
                {
                    classObj.Name = model.Name;
                    classObj.NameAr = model.NameAr;
                    classObj.Code = model.Code;
                    _context.Classifications.Add(classObj);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return classObj.Id;
        }

        public int Delete(int id)
        {
            var classObj = _context.Classifications.Find(id);
            try
            {
                if (classObj != null)
                {
                    _context.Classifications.Remove(classObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }

        public IEnumerable<Classification> GetAll()
        {
            return _context.Classifications.ToList();
        }

        public Classification GetById(int id)
        {
            return _context.Classifications.Find(id);
        }

        public IEnumerable<Classification> SortClassification(SortClassificationVM sortObj)
        {
            var lstClassify = GetAll().ToList();
            if (sortObj.Code != "")
            {
                if (sortObj.SortStatus == "descending")
                    lstClassify = lstClassify.OrderByDescending(d => d.Code).ToList();
                else
                    lstClassify = lstClassify.OrderBy(d => d.Code).ToList();
            }
            else if (sortObj.Name != "")
            {
                if (sortObj.SortStatus == "descending")
                    lstClassify = lstClassify.OrderByDescending(d => d.Name).ToList();
                else
                    lstClassify = lstClassify.OrderBy(d => d.Name).ToList();
            }

            else if (sortObj.NameAr != "")
            {
                if (sortObj.SortStatus == "descending")
                    lstClassify = lstClassify.OrderByDescending(d => d.NameAr).ToList();
                else
                    lstClassify = lstClassify.OrderBy(d => d.NameAr).ToList();
            }

            return lstClassify;
        }

        public int Update(Classification model)
        {
            try
            {
                var classObj = _context.Classifications.Find(model.Id);
                classObj.Id = model.Id;
                classObj.Name = model.Name;
                classObj.NameAr = model.NameAr;
                classObj.Code = model.Code;
                _context.Entry(classObj).State = EntityState.Modified;
                _context.SaveChanges();
                return classObj.Id;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }

     
    }
}
