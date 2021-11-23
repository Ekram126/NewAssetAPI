using Asset.Domain.Repositories;
using Asset.Models;
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

        public IEnumerable<IndexContractVM.GetData> GetContractsByMasterContractId(int masterContractId)
        {

            return (from master in _context.MasterContracts
                    join detail in _context.ContractDetails on master.Id equals detail.MasterContractId
                    join assetDetail in _context.AssetDetails on detail.AssetDetailId equals assetDetail.Id
                    join mainAsset in _context.MasterAssets on assetDetail.MasterAssetId equals mainAsset.Id
                    where master.Id == masterContractId
                    select new IndexContractVM.GetData
                    {
                        Id= detail.Id,
                        AssetDetailId = assetDetail.Id,
                        ContractDate = detail.ContractDate,
                        MasterContractId = master.Id,
                        AssetName = mainAsset.Name,
                        AssetNameAr = mainAsset.NameAr,
                        HasSpareParts = detail.HasSpareParts.ToString(),
                        ResponseTime =detail.ResponseTime.ToString()
                    }).ToList();
        }

        public int Update(ContractDetail masterContractObj)
        {
            throw new NotImplementedException();
        }
    }
}
