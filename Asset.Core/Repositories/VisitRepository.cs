using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.VisitVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Repositories
{
    public class VisitRepository :
        IVisitRepository
    {

        private ApplicationDbContext _context;
       
        public VisitRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Add(CreateVisitVM createVisitVM)
        {
            Engineer engObj = new Engineer();
            if (createVisitVM.UserId != "")
            {
                var lstUsers = _context.ApplicationUser.Where(a => a.Id == createVisitVM.UserId).ToList();
                if (lstUsers.Count > 0)
                {
                    var userObj = lstUsers[0];
                    var lstEngineers = _context.Engineers.Where(a => a.Email == userObj.Email).ToList();
                    if (lstEngineers.Count > 0)
                    {
                        engObj = lstEngineers[0];
                    }
                }
            }
            Visit visitObj = new Visit();
            visitObj.HospitalId = createVisitVM.HospitalId;
            visitObj.StatusId = 0;
            visitObj.VisitDate = createVisitVM.VisitDate;
            visitObj.VisitTypeId = createVisitVM.VisitTypeId;
            visitObj.VisitDescr = createVisitVM.VisitDescr;
            visitObj.Code = createVisitVM.Code;
            visitObj.ListAttachments = createVisitVM.ListAttachments;
            visitObj.EngineerId = engObj.Id;
            visitObj.Latitude = createVisitVM.Latitude;
            visitObj.Longtitude = createVisitVM.Longtitude;
            visitObj.IsMode = createVisitVM.IsMode;
            _context.Visits.Add(visitObj);
            _context.SaveChanges();
            // model.Id = visitObj.Id;
            return visitObj.Id;
        }
        public int Update(EditVisitVM editVisitVM)
        {
            Visit visitObj = _context.Visits.Find(editVisitVM.Id);
            visitObj.HospitalId = editVisitVM.HospitalId;
            visitObj.StatusId = 0;
            visitObj.VisitDate = editVisitVM.VisitDate;
            visitObj.VisitTypeId = editVisitVM.VisitTypeId;
            visitObj.VisitDescr = editVisitVM.VisitDescr;
            visitObj.Code = editVisitVM.Code;
            _context.Entry(visitObj).State = EntityState.Modified;
            _context.SaveChanges();
            return visitObj.Id;
        }
        public int UpdateVer(EditVisitVM editVisitVM)
        {
            Visit visitObj = _context.Visits.Find(editVisitVM.Id);
            if (visitObj.StatusId == 0)
                visitObj.StatusId = editVisitVM.StatusId;

            _context.Entry(visitObj).State = EntityState.Modified;
            _context.SaveChanges();
            return visitObj.Id;
        }

        public int Delete(int id)
        {
            var visitObj = _context.Visits.Find(id);
            try
            {
                if (visitObj != null)
                {
                    _context.Visits.Remove(visitObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return 0;
        }

        public List<IndexVisitVM.GetData> GetAll()
        {

            List<IndexVisitVM.GetData> list = new List<IndexVisitVM.GetData>();
            var lstVisits = _context.Visits
                .Include(a => a.Engineer)
                .Include(a => a.Hospital)
                 .Include(a => a.VisitType).ToList();

            foreach (var item in lstVisits)
            {
                IndexVisitVM.GetData visitObj = new IndexVisitVM.GetData();
                visitObj.Id = item.Id;
                visitObj.EngineerName = item.Engineer.Name;
                visitObj.EngineerNameAr = item.Engineer.NameAr;
                visitObj.HospitalName = item.Hospital.Name;
                visitObj.HospitalNameAr = item.Hospital.NameAr;
                visitObj.VisitTypeName = item.VisitType.Name;
                visitObj.VisitTypeNameAr = item.VisitType.NameAr;
                visitObj.Code = item.Code;
                visitObj.VisitDate = item.VisitDate.ToString();
                visitObj.StatusId = int.Parse(item.StatusId.ToString());
                list.Add(visitObj);
            }


            return list;
        }

        public IEnumerable<IndexVisitVM.GetData> SearchInVisits(SearchVisitVM searchObj)
        {
            List<IndexVisitVM.GetData> lstData = new List<IndexVisitVM.GetData>();

            var list = _context.Visits.Include(a => a.Engineer)
                .Include(a => a.Hospital)
                .Include(a => a.VisitType).ToList();

            if (searchObj.EngineerId != 0)
            {
                list = list.Where(a => a.Engineer.Id == searchObj.EngineerId).ToList();
            }
            else
            {
                list = list.ToList();
            }
            if (searchObj.HospitalId != 0)
            {
                list = list.Where(a => a.Hospital.Id == searchObj.HospitalId).ToList();
            }
            else
            {
                list = list.ToList();
            }
            if (searchObj.VisitTypeId != 0)
            {
                list = list.Where(a => a.VisitTypeId == searchObj.VisitTypeId).ToList();
            }
            else
            {
                list = list.ToList();
            }
            if (searchObj.FromVisitDate != null)
            {
                list = list.Where(a => a.VisitDate >= searchObj.FromVisitDate).ToList();
            }
            else
            {
                list = list.ToList();
            }
            if (searchObj.ToVisitDate != null)
            {
                list = list.Where(a => a.VisitDate <= searchObj.ToVisitDate).ToList();
            }
            else
            {
                list = list.ToList();
            }


            foreach (var item in list)
            {
                IndexVisitVM.GetData getDataObj = new IndexVisitVM.GetData();
                getDataObj.Id = item.Id;
                getDataObj.Code = item.Code;
                getDataObj.VisitDate = item.VisitDate.Value.ToShortDateString();
                getDataObj.StatusId = int.Parse(item.StatusId.ToString());
                if (item.HospitalId != null)
                {
                    getDataObj.HospitalName = item.Hospital.Name;
                    getDataObj.HospitalNameAr = item.Hospital.NameAr;
                }
                if (item.VisitTypeId != null)
                {
                    getDataObj.VisitTypeName = item.VisitType.Name;
                    getDataObj.VisitTypeNameAr = item.VisitType.NameAr;
                }

                if (item.EngineerId != null)
                {
                    getDataObj.EngineerName = item.Engineer.Name;
                    getDataObj.EngineerNameAr = item.Engineer.NameAr;
                }
                lstData.Add(getDataObj);
            }



            return lstData;
        }


        public Visit GetById(int id)
        {
            return _context.Visits.Find(id);
        }

        public ViewVisitVM ViewVisitById(int id)
        {
            var lstVisits = _context.Visits.Include(a => a.Hospital).Include(a => a.VisitType)
                .Include(a => a.Engineer).Where(a => a.Id == id).ToList();

            if (lstVisits.Count > 0)
            {
                Visit visitObj = lstVisits[0];
                ViewVisitVM editvisitObj = new ViewVisitVM();
                visitObj.Id = visitObj.Id;
                editvisitObj.EngineerName = visitObj.Engineer.Name;
                editvisitObj.EngineerNameAr = visitObj.Engineer.NameAr;
                editvisitObj.HospitalName = visitObj.Hospital.Name;
                editvisitObj.HospitalNameAr = visitObj.Hospital.NameAr;
                editvisitObj.VisitTypeName = visitObj.VisitType.Name;
                editvisitObj.VisitTypeNameAr = visitObj.VisitType.NameAr;
                editvisitObj.VisitDate = visitObj.VisitDate.ToString();
                editvisitObj.VisitDescr = visitObj.VisitDescr;
                editvisitObj.Code = visitObj.Code;
                return editvisitObj;
            }
            return null;
        }

        public IEnumerable<IndexVisitVM.GetData> SortVisits(SortVisitVM sortObj, int statusId)
        {
            List<IndexVisitVM.GetData> list = new List<IndexVisitVM.GetData>();

            var lstVisits = _context.Visits.Include(a => a.Engineer)
                   .Include(a => a.Hospital)
                   .Include(a => a.VisitType).ToList();

            foreach (var item in lstVisits)
            {
                IndexVisitVM.GetData visitobj = new IndexVisitVM.GetData();
                visitobj.Id = item.Id;
                visitobj.StatusId = int.Parse(item.StatusId.ToString());
                visitobj.HospitalName = item.Hospital.Name;
                visitobj.HospitalNameAr = item.Hospital.NameAr;
                visitobj.EngineerName = item.Engineer.Name;
                visitobj.EngineerNameAr = item.Engineer.NameAr;
                visitobj.VisitDate = item.VisitDate.Value.ToShortDateString();
                visitobj.SortVisitDate = item.VisitDate;
                visitobj.Code = item.Code;
                visitobj.VisitTypeName = item.VisitType.Name;
                visitobj.VisitTypeNameAr = item.VisitType.NameAr;


                list.Add(visitobj);
            }
            //if(statusId== 0)
            //{
            //    list = list.ToList();
            //}
            //else
            //{
            //   list = list.Where(a=>a.StatusId == statusId).ToList();
            //}

            if (sortObj != null)
            {
                //     var userObj = _context.ApplicationUser.Find(sortObj.Id);
                if (sortObj.HospitalName != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.HospitalName).ToList();
                    else
                        list = list.OrderBy(d => d.HospitalName).ToList();
                }
                else if (sortObj.HospitalNameAr != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.HospitalNameAr).ToList();
                    else
                        list = list.OrderBy(d => d.HospitalNameAr).ToList();
                }

                else if (sortObj.EngineerName != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.EngineerName).ToList();
                    else
                        list = list.OrderBy(d => d.EngineerName).ToList();
                }
                else if (sortObj.EngineerNameAr != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.EngineerNameAr).ToList();
                    else
                        list = list.OrderBy(d => d.EngineerNameAr).ToList();
                }


                else if (sortObj.VisitTypeName != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.VisitTypeName).ToList();
                    else
                        list = list.OrderBy(d => d.VisitTypeName).ToList();
                }
                else if (sortObj.VisitTypeNameAr != "")
                {
                    if (sortObj.SortStatus == "descending")
                        list = list.OrderByDescending(d => d.VisitTypeNameAr).ToList();
                    else
                        list = list.OrderBy(d => d.VisitTypeNameAr).ToList();
                }

                else if (sortObj.VisitDate != "")
                {
                    if (sortObj.SortStatus == "descending")
                    {
                        list = list.OrderByDescending(d => d.SortVisitDate).ToList();
                    }
                    else
                        list = list.OrderBy(d => d.SortVisitDate).ToList();
                }


            }
            return list;
        }

        public int CreateVisitAttachments(VisitAttachment attachObj)
        {
            VisitAttachment documentObj = new VisitAttachment();
            documentObj.Title = attachObj.Title;
            documentObj.FileName = attachObj.FileName;
            documentObj.VisitId = attachObj.VisitId;
            _context.VisitAttachments.Add(documentObj);
            _context.SaveChanges();
            return attachObj.Id;
        }

        //public GeneratedVisitNumberVM GenerateVisitNumber()
        //{
        //    GeneratedVisitNumberVM numberObj = new GeneratedVisitNumberVM();
        //    string VI = "Vis";

        //    var lstIds = _context.Visit.ToList();
        //    if (lstIds.Count > 0)
        //    {
        //        var code = lstIds.LastOrDefault().Id;
        //        numberObj.RequestCode = VI + (code + 1);
        //    }
        //    else
        //    {
        //        numberObj.VisitCode = VI + 1;
        //    }

        //    return numberObj;
        //}

        public IEnumerable<VisitAttachment> GetVisitAttachmentByVisitId(int visitId)
        {
            var lstAttachments = _context.VisitAttachments.Where(a => a.VisitId == visitId).ToList();
            return lstAttachments;
        }

        public GeneratedVisitCodeVM GenerateVisitCode()
        {
            GeneratedVisitCodeVM numberObj = new GeneratedVisitCodeVM();
            string WO = "VS";

            var lstIds = _context.Visits.ToList();
            if (lstIds.Count > 0)
            {
                var code = lstIds.LastOrDefault().Id;
                numberObj.VisitCode = WO + (code + 1);
            }
            else
            {
                numberObj.VisitCode = WO + 1;
            }

            return numberObj;
        }
    }
}

