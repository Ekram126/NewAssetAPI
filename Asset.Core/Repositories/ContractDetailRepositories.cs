using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.AssetDetailVM;
using Asset.ViewModels.ContractVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public List<IndexContractVM.GetData> GetContractAssetsByHospitalId(int hospitalId, int masterContractId)
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

            return (from master in _context.MasterContracts
                    join detail in _context.ContractDetails on master.Id equals detail.MasterContractId
                    join assetDetail in _context.AssetDetails on detail.AssetDetailId equals assetDetail.Id
                    join mainAsset in _context.MasterAssets on assetDetail.MasterAssetId equals mainAsset.Id
                    where master.Id == masterContractId
                    select new IndexContractVM.GetData
                    {
                        Id = detail.Id,
                        AssetDetailId = assetDetail.Id,
                        HospitalId = assetDetail.HospitalId,
                        SerialNumber = assetDetail.SerialNumber,
                        BarCode = assetDetail.Barcode,
                        ContractDate = detail.ContractDate,
                        MasterContractId = master.Id,
                        AssetName = mainAsset.Name,
                        AssetNameAr = mainAsset.NameAr,
                        HospitalName = _context.Hospitals.Where(a => a.Id == assetDetail.HospitalId).FirstOrDefault().Name,
                        HospitalNameAr = _context.Hospitals.Where(a => a.Id == assetDetail.HospitalId).FirstOrDefault().NameAr,
                        HasSpareParts = detail.HasSpareParts.ToString(),
                        ResponseTime = detail.ResponseTime.ToString()
                    }).ToList();
        }

        public List<Hospital> GetListofHospitalsFromAssetContractDetailByMasterContractId(int masterContractId)
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
