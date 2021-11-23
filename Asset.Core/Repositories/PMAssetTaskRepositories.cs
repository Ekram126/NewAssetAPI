using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.PMAssetTaskVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class PMAssetTaskRepositories : IPMAssetTaskRepository
    {
        private ApplicationDbContext _context;


        public PMAssetTaskRepositories(ApplicationDbContext context)
        {
            _context = context;
        }




        public int Add(CreatePMAssetTaskVM model)
        {
            PMAssetTask taskObj = new PMAssetTask();
            taskObj.Name = model.TaskName;
            taskObj.NameAr = model.TaskNameAr;
            taskObj.MasterAssetId = model.MasterAssetId;
            _context.PMAssetTasks.Add(taskObj);
            return _context.SaveChanges();
        }

        public int Delete(int id)
        {
            try
            {
                var taskObj = _context.PMAssetTasks.Find(id);
                _context.PMAssetTasks.Remove(taskObj);
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return 0;
        }

        public IEnumerable<PMAssetTask> GetAll()
        {
            return _context.PMAssetTasks.ToList();
        }

        public PMAssetTask GetById(int id)
        {
            return  _context.PMAssetTasks.Find(id);
        }

        public IEnumerable<PMAssetTask> GetPMAssetTaskByMasterAssetId(int masterAssetId)
        {
            return _context.PMAssetTasks.Where(a=>a.MasterAssetId == masterAssetId).ToList();
        }

        public PMAssetTask GetPMAssetTaskByTaskIdAndMasterAssetId(int masterAssetId, int taskId)
        {
            return _context.PMAssetTasks.Where(a => a.MasterAssetId == masterAssetId && a.Id == taskId).First();
        }

        public int Update(PMAssetTask model)
        {
            try
            {
                var taskObj = _context.PMAssetTasks.Find(model.Id);
                taskObj.Id = model.Id;
                taskObj.Name = model.Name;
                taskObj.NameAr = model.NameAr;
                taskObj.MasterAssetId = model.MasterAssetId;
                _context.Entry(taskObj).State = EntityState.Modified;
                _context.SaveChanges();
                return taskObj.Id;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }
    }
}
