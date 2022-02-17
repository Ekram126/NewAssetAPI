using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.SubProblemVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class SubProblemRepository : ISubProblemRepository
    {
        private ApplicationDbContext _context;
        string msg;
        public SubProblemRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(CreateSubProblemVM createSubProblemVM)
        {
            try
            {
                if (createSubProblemVM != null)
                {
                    SubProblem subProblem = new SubProblem();
                    subProblem.Name = createSubProblemVM.Name;
                    subProblem.NameAr = createSubProblemVM.NameAr;
                    subProblem.Code = createSubProblemVM.Code;
                    subProblem.ProblemId = createSubProblemVM.ProblemId;
                    _context.SubProblems.Add(subProblem);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }

        public void Delete(int id)
        {
            var subproblem = _context.SubProblems.Find(id);
            try
            {
                if (subproblem != null)
                {
                    _context.SubProblems.Remove(subproblem);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }

        public IEnumerable<IndexSubProblemVM> GetAll()
        {
            return _context.SubProblems.Select(prob => new IndexSubProblemVM
            {
                Id = prob.Id,
                Name = prob.Name,
                NameAr = prob.NameAr,
                Code = prob.Code,
                ProblemId=prob.ProblemId != null ? (int)prob.ProblemId : 0,
                ProblemName= prob.ProblemId != null ? prob.Problem.Name:""
            }).ToList();
        }

        public IEnumerable<IndexSubProblemVM> GetAllSubProblemsByProblemId(int ProblemId)
        {
            return _context.SubProblems.Where(p=>p.ProblemId==ProblemId).Select(prob => new IndexSubProblemVM
            {
                Id = prob.Id,
                Name = prob.Name,
                NameAr = prob.NameAr,
                Code = prob.Code,
                ProblemId = prob.ProblemId != null ? (int)prob.ProblemId:0,
                ProblemName = prob.ProblemId != null ? prob.Problem.Name:""
            }).ToList();
        }

        public IndexSubProblemVM GetById(int id)
        {
            return _context.SubProblems.Select(prob => new IndexSubProblemVM
            {
                Id = prob.Id,
                Name = prob.Name,
                NameAr = prob.NameAr,
                Code = prob.Code,
                ProblemId = prob.ProblemId != null? (int)prob.ProblemId:0,
                ProblemName = prob.ProblemId != null ? prob.Problem.Name:""
                
            }).Where(e=>e.Id==id).FirstOrDefault();
        }

        public void Update(EditSubProblemVM editSubProblemVM)
        {
            try
            {
                SubProblem subProblemObj = _context.SubProblems.Find(editSubProblemVM.Id);
                subProblemObj.Id = editSubProblemVM.Id;
                subProblemObj.Name = editSubProblemVM.Name;
                subProblemObj.NameAr = editSubProblemVM.NameAr;
                subProblemObj.Code = editSubProblemVM.Code;
                subProblemObj.ProblemId = editSubProblemVM.ProblemId;
                _context.Entry(subProblemObj).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }
    }
}
