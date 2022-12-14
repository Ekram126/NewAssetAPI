using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Asset.Domain.Repositories;
using Asset.Models;
using Asset.ViewModels.ScrapVM;
using Microsoft.EntityFrameworkCore;

namespace Asset.Core.Repositories
{
    public class ScrapRepository : IScrapRepository
    {
        private ApplicationDbContext _context;
        public ScrapRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public int Add(CreateScrapVM createScrapVM)
        {
            Scrap scrapObj = new Scrap();
            scrapObj.AssetDetailId = createScrapVM.AssetDetailId;
            scrapObj.ScrapDate = createScrapVM.ScrapDate;
            scrapObj.ScrapDate = DateTime.Parse(createScrapVM.StrScrapDate);
            scrapObj.SysDate = createScrapVM.SysDate;
            scrapObj.Comment = createScrapVM.Comment;
            scrapObj.ScrapNo = createScrapVM.ScrapNo;
            // scrapObj.ListAttachments = createScrapVM.ListAttachments;
            _context.Scraps.Add(scrapObj);
            _context.SaveChanges();
            int id = scrapObj.Id;
            if (createScrapVM.ReasonIds.Count() > 0)
            {
                foreach (var reasonId in createScrapVM.ReasonIds)
                {
                    AssetScrap assetScrapObj = new AssetScrap();
                    assetScrapObj.ScrapId = id;
                    assetScrapObj.ScrapReasonId = reasonId;

                    _context.AssetScraps.Add(assetScrapObj);
                    _context.SaveChanges();
                }
            }
            return scrapObj.Id;
        }
        public GeneratedScrapNumberVM GenerateScrapNumber()
        {
            GeneratedScrapNumberVM numberObj = new GeneratedScrapNumberVM();
            string WO = "Scr";

            var lstIds = _context.Scraps.ToList();
            if (lstIds.Count > 0)
            {
                var code = lstIds.LastOrDefault().Id;
                numberObj.ScrapNo = WO + (code + 1);
            }
            else
            {
                numberObj.ScrapNo = WO + 1;
            }

            return numberObj;
        }
        public int CreateScrapAttachments(ScrapAttachment attachObj)
        {
            ScrapAttachment documentObj = new ScrapAttachment();
            documentObj.Title = attachObj.Title;
            documentObj.FileName = attachObj.FileName;
            documentObj.ScrapId = attachObj.ScrapId;
            _context.ScrapAttachments.Add(documentObj);
            _context.SaveChanges();
            return attachObj.Id;
        }
        public int Delete(int id)
        {
            var scrapObj = _context.Scraps.Find(id);
            try
            {

                _context.Scraps.Remove(scrapObj);
                var isDeleted = _context.SaveChanges();
                if (isDeleted == 1)
                {
                    var lstAssetTrans = _context.AssetStatusTransactions.Include(a => a.AssetDetail).Where(a => a.AssetDetailId == scrapObj.AssetDetailId).ToList();
                    if (lstAssetTrans.Count > 0)
                    {
                        AssetStatusTransaction AssetStatusTransactionsTransactionObj = new AssetStatusTransaction();
                        AssetStatusTransactionsTransactionObj.AssetDetailId = (int)scrapObj.AssetDetailId;
                        AssetStatusTransactionsTransactionObj.AssetStatusId = 3;
                        var lstAssets = _context.AssetDetails.Where(a => a.Id == scrapObj.AssetDetailId).ToList();
                        if (lstAssets.Count > 0)
                        {
                            AssetStatusTransactionsTransactionObj.HospitalId = lstAssets[0].HospitalId;
                        }
                        AssetStatusTransactionsTransactionObj.StatusDate = DateTime.Now;
                        _context.AssetStatusTransactions.Add(AssetStatusTransactionsTransactionObj);
                        _context.SaveChanges();
                    }
                }
                return 1;



            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }
        public List<IndexScrapVM.GetData> GetAll()
        {
            List<IndexScrapVM.GetData> list = new List<IndexScrapVM.GetData>();
            var lstScraps = _context.Scraps
                .Include(a => a.AssetDetail)
                .Include(a => a.AssetDetail.MasterAsset)
                .Include(a => a.AssetDetail.Department)
                .Include(a => a.AssetDetail.MasterAsset.brand)
               .ToList();
            foreach (var item in lstScraps)
            {
                IndexScrapVM.GetData scrapObj = new IndexScrapVM.GetData();
                scrapObj.Id = item.Id;
                scrapObj.AssetName = item.AssetDetail.MasterAsset != null ? item.AssetDetail.MasterAsset.Name : "";
                scrapObj.AssetNameAr = item.AssetDetail.MasterAsset != null ? item.AssetDetail.MasterAsset.NameAr : "";
                scrapObj.Barcode = item.AssetDetail.Barcode;
                scrapObj.SerialNumber = item.AssetDetail.SerialNumber;
                scrapObj.ScrapDate = item.ScrapDate.ToString();
                scrapObj.ScrapNo = item.ScrapNo;
                scrapObj.DepartmentName = item.AssetDetail.Department.Name;
                scrapObj.DepartmentNameAr = item.AssetDetail.Department.NameAr;
                if (item.AssetDetail.MasterAsset.brand != null)
                {
                    scrapObj.BrandId = item.AssetDetail.MasterAsset.BrandId;
                    scrapObj.BrandName = item.AssetDetail.MasterAsset.brand.Name;
                    scrapObj.BrandNameAr = item.AssetDetail.MasterAsset.brand.NameAr;
                }
                scrapObj.Model = item.AssetDetail.MasterAsset.ModelNumber;


                list.Add(scrapObj);
            }
            return list;
        }
        public ViewScrapVM ViewScrapById(int id)
        {
            var lstScraps = _context.Scraps.Include(a => a.AssetDetail).Include(a => a.AssetDetail.MasterAsset)
                //.Include(a => a.ScrapReason)
                .Include(a => a.AssetDetail.MasterAsset.brand).Where(a => a.Id == id).ToList();
            if (lstScraps.Count > 0)
            {
                Scrap scrapObj = lstScraps[0];
                ViewScrapVM editscrapObj = new ViewScrapVM();
                scrapObj.Id = scrapObj.Id;
                editscrapObj.AssetName = scrapObj.AssetDetail.MasterAsset.Name;
                editscrapObj.AssetNameAr = scrapObj.AssetDetail.MasterAsset.NameAr;
                editscrapObj.Barcode = scrapObj.AssetDetail.Barcode;
                editscrapObj.Model = scrapObj.AssetDetail.MasterAsset.ModelNumber;
                editscrapObj.ScrapDate = scrapObj.ScrapDate.ToString();
                editscrapObj.ScrapNo = scrapObj.ScrapNo;
                editscrapObj.Comment = scrapObj.Comment;
                editscrapObj.SerialNumber = scrapObj.AssetDetail.SerialNumber;

                if (scrapObj.AssetDetail.MasterAsset.brand != null)
                {
                    editscrapObj.BrandId = scrapObj.AssetDetail.MasterAsset.BrandId;
                    editscrapObj.BrandName = scrapObj.AssetDetail.MasterAsset.brand.Name;
                    editscrapObj.BrandNameAr = scrapObj.AssetDetail.MasterAsset.brand.NameAr;
                }
                // editscrapObj.ScrapReasonName = scrapObj.ScrapReason.Name;
                // editscrapObj.ScrapReasonNameAr = scrapObj.ScrapReason.NameAr;
                return editscrapObj;
            }
            return null;
        }
        public Scrap GetById(int id)
        {
            return _context.Scraps.Find(id);
        }
        public IEnumerable<ScrapAttachment> GetScrapAttachmentByScrapId(int scrapId)
        {
            var lstAttachments = _context.ScrapAttachments.Where(a => a.ScrapId == scrapId).ToList();
            return lstAttachments;
        }
        public List<ViewScrapVM> GetScrapReasonByScrapId(int scrapId)
        {
            var lstReasons = _context.AssetScraps.Include(a => a.ScrapReason).Where(a => a.ScrapId == scrapId).ToList()
                .Select(item => new ViewScrapVM
                {
                    ScrapReasonName = item.ScrapReason.Name,
                    ScrapReasonNameAr = item.ScrapReason.NameAr
                }).ToList();
            return lstReasons;
        }
        public IEnumerable<IndexScrapVM.GetData> SearchInScraps(SearchScrapVM searchObj)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<IndexScrapVM.GetData> SortScraps(SortScrapVM sortObj, int statusId)
        {
            throw new NotImplementedException();
        }



    }
}
