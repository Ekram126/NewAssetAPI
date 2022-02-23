using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.HospitalSupplierStatusVM;
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
        public IndexHospitalSupplierStatusVM GetAllByHospitals()
        {
            IndexHospitalSupplierStatusVM ItemObj = new IndexHospitalSupplierStatusVM();
            var list = _context.HospitalSupplierStatuses.ToList();
            ItemObj.ListStatus = list;
            foreach (var itm in list)
            {
                var lstHospitalStatus = _context.HospitalApplications.Include(a => a.AssetDetail).Where(a => a.StatusId == itm.Id).ToList();

                if (itm.Id == 1)
                {
                    ItemObj.OpenStatus = lstHospitalStatus.Count;
                }
                if (itm.Id == 2)
                {
                    ItemObj.ApproveStatus = lstHospitalStatus.Count;
                }
                if (itm.Id == 3)
                {
                    ItemObj.RejectStatus = lstHospitalStatus.Count;
                }
                if (itm.Id == 4)
                {
                    ItemObj.SystemRejectStatus = lstHospitalStatus.Count;
                }
            }
            return ItemObj;
        }

        public IndexHospitalSupplierStatusVM GetAll(int appTypeId,int? hospitalId)
        {

            IndexHospitalSupplierStatusVM ItemObj = new IndexHospitalSupplierStatusVM();
            var list = _context.HospitalSupplierStatuses.ToList();
            ItemObj.ListStatus = list;



            if (hospitalId == null || hospitalId == 0)
            {
                if (list.Count > 0)
                {
                    foreach (var itm in list)
                    {
                        var lstStatus = _context.SupplierExecludeAssets.Where(a => a.StatusId == itm.Id && a.AppTypeId == appTypeId).ToList();
                        if (itm.Id == 1)
                        {
                            ItemObj.OpenStatus = lstStatus.Count;
                        }
                        if (itm.Id == 2)
                        {
                            ItemObj.ApproveStatus = lstStatus.Count;
                        }
                        if (itm.Id == 3)
                        {
                            ItemObj.RejectStatus = lstStatus.Count;
                        }
                        if (itm.Id == 4)
                        {
                            ItemObj.SystemRejectStatus = lstStatus.Count;
                        }
                    }
                }
            }
            else
            {
                foreach (var itm in list)
                {
                    var lstHospitalStatus = _context.HospitalApplications.Include(a => a.AssetDetail).Where(a => a.AssetDetail.HospitalId == hospitalId && a.StatusId == itm.Id && a.AppTypeId == appTypeId).ToList();

                    if (itm.Id == 1)
                    {
                        ItemObj.OpenStatus = lstHospitalStatus.Count;
                    }
                    if (itm.Id == 2)
                    {
                        ItemObj.ApproveStatus = lstHospitalStatus.Count;
                    }
                    if (itm.Id == 3)
                    {
                        ItemObj.RejectStatus = lstHospitalStatus.Count;
                    }
                    if (itm.Id == 4)
                    {
                        ItemObj.SystemRejectStatus = lstHospitalStatus.Count;
                    }

                }
            }
            return ItemObj;
        }


        public HospitalSupplierStatus GetById(int id)
        {
            return _context.HospitalSupplierStatuses.Find(id);
        }
    }
}
