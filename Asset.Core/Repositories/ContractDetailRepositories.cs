using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.AssetDetailVM;
using Asset.ViewModels.ContractVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Asset.Core.Repositories
{
    public class ContractDetailRepositories : IContractDetailRepository
    {
        private ApplicationDbContext _context;

        public ContractDetailRepositories(ApplicationDbContext context)
        {
            _context = context;
        }



        public int Add(ContractDetail model)
        {
            ContractDetail contractDetailObj = new ContractDetail();
            try
            {
                if (model != null)
                {
                    contractDetailObj.ContractDate = model.ContractDate;
                    contractDetailObj.AssetDetailId = model.AssetDetailId;
                    contractDetailObj.ResponseTime = model.ResponseTime;
                    contractDetailObj.HasSpareParts = model.HasSpareParts;
                    contractDetailObj.MasterContractId = model.MasterContractId;
                    contractDetailObj.HospitalId = model.HospitalId;
                    _context.ContractDetails.Add(contractDetailObj);
                    _context.SaveChanges();
                    return contractDetailObj.Id;
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
            var contractDetailObj = _context.ContractDetails.Find(id);
            try
            {
                if (contractDetailObj != null)
                {
                    _context.ContractDetails.Remove(contractDetailObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return 0;
        }

        public IEnumerable<ContractDetail> GetAll()
        {
            return _context.ContractDetails.ToList();
        }

        public ContractDetail GetById(int id)
        {
            return _context.ContractDetails.Find(id);
        }

        public IEnumerable<IndexContractVM.GetData> GetContractAssetsByHospitalId(int hospitalId, int masterContractId)
        {
            List<IndexContractVM.GetData> lstAssetDetails = new List<IndexContractVM.GetData>();

            var lstAssets = (from master in _context.MasterContracts
                             join detail in _context.ContractDetails on master.Id equals detail.MasterContractId
                             join assetDetail in _context.AssetDetails on detail.AssetDetailId equals assetDetail.Id
                             join host in _context.Hospitals on assetDetail.HospitalId equals host.Id
                             where assetDetail.HospitalId == hospitalId
                             && detail.MasterContractId == masterContractId
                             select detail).ToList();
            foreach (var item in lstAssets)
            {
                IndexContractVM.GetData getDataObj = new IndexContractVM.GetData();
                getDataObj.Id = item.Id;




              
                var lstassets = _context.AssetDetails.Include(a=>a.MasterAsset)
                    .Include(a => a.MasterAsset.brand)
                    .Include(a => a.Department).Where(a => a.Id == item.AssetDetailId).ToList();
                if (lstassets.Count > 0)
                {
                    AssetDetail assetDetailObj = lstassets[0];
                    getDataObj.SerialNumber = assetDetailObj.SerialNumber;
                    getDataObj.HospitalId = assetDetailObj.HospitalId;
                    getDataObj.BarCode = assetDetailObj.Barcode;

                    if (assetDetailObj.MasterAsset.brand != null)
                    {
                        getDataObj.BrandName = assetDetailObj.MasterAsset.brand.Name;
                        getDataObj.BrandNameAr = assetDetailObj.MasterAsset.brand.NameAr;
                    }


                    if (assetDetailObj.Department != null)
                    {
                        getDataObj.DepartmentName = assetDetailObj.Department.Name;
                        getDataObj.DepartmentNameAr = assetDetailObj.Department.NameAr;
                    }








                    var lstmasters = _context.MasterAssets.Where(a => a.Id == lstassets[0].MasterAssetId).ToList();
                    if (lstmasters.Count > 0)
                    {
                        MasterAsset masterAssetObj = lstmasters[0];
                        getDataObj.AssetName = masterAssetObj.Name;
                        getDataObj.AssetNameAr = masterAssetObj.NameAr;
                    }
                }
                getDataObj.HasSpareParts = item.HasSpareParts.ToString();
                getDataObj.ResponseTime = item.ResponseTime.ToString();
                lstAssetDetails.Add(getDataObj);
            }

            return lstAssetDetails;
        }

        public IEnumerable<IndexContractVM.GetData> GetContractByHospitalId(int hospitalId)
        {
            List<IndexContractVM.GetData> lstAssetDetails = new List<IndexContractVM.GetData>();

            //var lstAssets = (from master in _context.MasterContracts
            //                 join detail in _context.ContractDetails on master.Id equals detail.MasterContractId
            //                 join assetDetail in _context.AssetDetails on detail.AssetDetailId equals assetDetail.Id
            //                 join host in _context.Hospitals on assetDetail.HospitalId equals host.Id
            //                 where assetDetail.HospitalId == hospitalId                           
            //                 select detail).ToList();


            var lstAssets = _context.ContractDetails
                .Include(a => a.MasterContract)
               .Include(a => a.AssetDetail)
                .Include(a => a.AssetDetail.Hospital)
                .Where(a => a.HospitalId == hospitalId).OrderByDescending(a => a.Id).ToList();




            foreach (var item in lstAssets)
            {
                IndexContractVM.GetData getDataObj = new IndexContractVM.GetData();
                getDataObj.ContractName = item.MasterContract.Subject;
                getDataObj.Id = item.Id;
                var lstassets = _context.AssetDetails.Where(a => a.Id == item.AssetDetailId).ToList();
                if (lstassets.Count > 0)
                {
                    AssetDetail assetDetailObj = lstassets[0];
                    getDataObj.SerialNumber = assetDetailObj.SerialNumber;
                    getDataObj.HospitalId = assetDetailObj.HospitalId;
                    getDataObj.BarCode = assetDetailObj.Barcode;

                    var lstmasters = _context.MasterAssets.Where(a => a.Id == lstassets[0].MasterAssetId).ToList();
                    if (lstmasters.Count > 0)
                    {
                        MasterAsset masterAssetObj = lstmasters[0];
                        getDataObj.AssetName = masterAssetObj.Name;
                        getDataObj.AssetNameAr = masterAssetObj.NameAr;
                    }
                }
                getDataObj.HasSpareParts = item.HasSpareParts.ToString();
                getDataObj.ResponseTime = item.ResponseTime.ToString();
                lstAssetDetails.Add(getDataObj);
            }

            return lstAssetDetails;
        }

        public IEnumerable<IndexContractVM.GetData> GetContractsByMasterContractId(int masterContractId)
        {

            return _context.ContractDetails
                .Include(a => a.AssetDetail)
                .Include(a => a.AssetDetail.MasterAsset)
                .Include(a => a.AssetDetail.Hospital).Include(a => a.MasterContract)
                .Where(a => a.MasterContract.Id == masterContractId)
                .Select(item => new IndexContractVM.GetData
                {
                    Id = item.Id,
                    ContractDate = item.ContractDate,
                    MasterContractId = item.MasterContractId,
                    HasSpareParts = item.HasSpareParts.ToString(),
                    ResponseTime = item.ResponseTime.ToString(),

                    AssetDetailId = item.AssetDetail.Id,
                    HospitalId = item.AssetDetail.HospitalId,
                    SerialNumber = item.AssetDetail.SerialNumber,
                    BarCode = item.AssetDetail.Barcode,
                    AssetName = item.AssetDetail.MasterAsset.Name,
                    AssetNameAr = item.AssetDetail.MasterAsset.NameAr,
                    HospitalName = item.AssetDetail.Hospital.Name,
                    HospitalNameAr = item.AssetDetail.Hospital.NameAr,

                }).ToList();


            //return (from master in _context.MasterContracts
            //        join detail in _context.ContractDetails on master.Id equals detail.MasterContractId
            //        join assetDetail in _context.AssetDetails on detail.AssetDetailId equals assetDetail.Id
            //        join mainAsset in _context.MasterAssets on assetDetail.MasterAssetId equals mainAsset.Id
            //        where master.Id == masterContractId
            //        select new IndexContractVM.GetData
            //        {
            //            Id = detail.Id,
            //            AssetDetailId = assetDetail.Id,
            //            HospitalId = assetDetail.HospitalId,
            //            SerialNumber = assetDetail.SerialNumber,
            //            BarCode = assetDetail.Barcode,
            //            ContractDate = detail.ContractDate,
            //            MasterContractId = master.Id,
            //            AssetName = mainAsset.Name,
            //            AssetNameAr = mainAsset.NameAr,
            //            HospitalName = _context.Hospitals.Where(a => a.Id == assetDetail.HospitalId).FirstOrDefault().Name,
            //            HospitalNameAr = _context.Hospitals.Where(a => a.Id == assetDetail.HospitalId).FirstOrDefault().NameAr,
            //            HasSpareParts = detail.HasSpareParts.ToString(),
            //            ResponseTime = detail.ResponseTime.ToString()
            //        }).ToList();
        }

        public IEnumerable<Hospital> GetListofHospitalsFromAssetContractDetailByMasterContractId(int masterContractId)
        {
            List<Hospital> lstHospitals = new List<Hospital>();
            var lstAssets = (from master in _context.MasterContracts
                             join detail in _context.ContractDetails on master.Id equals detail.MasterContractId
                             join assetDetail in _context.AssetDetails on detail.AssetDetailId equals assetDetail.Id
                             join host in _context.Hospitals on assetDetail.HospitalId equals host.Id
                             where master.Id == masterContractId
                             select host).ToList().GroupBy(a => a.Id).ToList();
            foreach (var item in lstAssets)
            {
                Hospital hospitalObj = new Hospital();
                hospitalObj.Id = item.FirstOrDefault().Id;
                hospitalObj.Code = item.FirstOrDefault().Code;
                hospitalObj.Name = item.FirstOrDefault().Name;
                hospitalObj.NameAr = item.FirstOrDefault().NameAr;
                lstHospitals.Add(hospitalObj);
            }

            return lstHospitals;
        }

        public int Update(ContractDetail masterContractObj)
        {
            throw new NotImplementedException();
        }
    }
}
