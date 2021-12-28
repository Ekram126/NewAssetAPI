using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.ProblemVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class ProblemRepository : IProblemRepository
    {
        private ApplicationDbContext _context;
        string msg;
        public ProblemRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(CreateProblemVM createProblemVM)
        {
            try
            {
                if (createProblemVM != null)
                {
                    Problem problem = new Problem();
                    problem.Name = createProblemVM.Name;
                    problem.NameAr = createProblemVM.NameAr;
                    problem.Code = createProblemVM.Code;
                    problem.MasterAssetId = createProblemVM.MasterAssetId;
                    _context.Problems.Add(problem);
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
            var problem = _context.Problems.Find(id);
            try
            {
                if (problem != null)
                {
                    _context.Problems.Remove(problem);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }

        public IEnumerable<IndexProblemVM> GetAll()
        {
            return _context.Problems.Select(prob => new IndexProblemVM
            {
                Id = prob.Id,
                Name = prob.Name,
                NameAr = prob.NameAr,
                Code = prob.Code,
                MasterAssetId= (int)prob.MasterAssetId,
                MasterAssetName= prob.MasterAsset.Name
            }).ToList();
        }

        public IndexProblemVM GetById(int id)
        {
            return _context.Problems.Select(prob => new IndexProblemVM
            {
                Id = prob.Id,
                Name = prob.Name,
                NameAr = prob.NameAr,
                Code = prob.Code,
                MasterAssetId = (int)prob.MasterAssetId,
                MasterAssetName = prob.MasterAsset.Name
            }).Where(e=>e.Id == id).FirstOrDefault();
        }

        public IEnumerable<IndexProblemVM> GetProblemBySubProblemId(int subProblemId)
        {
            var lst = _context.SubProblems.Include(a=>a.Problem).Where(a=>a.Id == subProblemId).Select(item => new IndexProblemVM
            {
                Id = item.Problem.Id,
                Name = item.Problem.Name,
                NameAr = item.Problem.NameAr,
                Code = item.Problem.Code,
                MasterAssetId = (int)item.Problem.MasterAssetId,
                MasterAssetName = item.Problem.MasterAsset.Name
            }).ToList();

            return lst;
        }

        public IEnumerable<IndexProblemVM> GetProblemsByMasterAssetId(int masterAssetId)
        {
         
            return _context.Problems.Where(a=>a.MasterAssetId == masterAssetId).Select(prob => new IndexProblemVM
            {
                Id = prob.Id,
                Name = prob.Name,
                NameAr = prob.NameAr,
                Code = prob.Code,
                MasterAssetId = (int)prob.MasterAssetId,
                MasterAssetName = prob.MasterAsset.Name
            }).ToList();
        }

        public void Update(EditProblemVM editProblemVM)
        {
            try
            {
                Problem problemObj = _context.Problems.Find(editProblemVM.Id);
                problemObj.Id = editProblemVM.Id;
                problemObj.Name = editProblemVM.Name;
                problemObj.NameAr = editProblemVM.NameAr;
                problemObj.Code = editProblemVM.Code;
                problemObj.MasterAssetId = editProblemVM.MasterAssetId;
                _context.Entry(problemObj).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }
    }
}
