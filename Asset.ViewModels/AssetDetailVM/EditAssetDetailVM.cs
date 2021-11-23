﻿using System;
using System.Collections.Generic;

namespace Asset.ViewModels.AssetDetailVM
{
    public class EditAssetDetailVM
    {
        public int Id { get; set; }

        public  string AssetName { get; set; }


        public string AssetNameAr { get; set; }


        public string Code { get; set; }
       public string PurchaseDate { get; set; }


        public decimal? Price { get; set; }

        public string SerialNumber { get; set; }

        public string Remarks { get; set; }

        public string Barcode { get; set; }

        public string InstallationDate { get; set; }

        public string WarrantyExpires { get; set; }
        public int? DepartmentId { get; set; }
        public int? SupplierId { get; set; }

        public int? HospitalId { get; set; }
        public int? MasterAssetId { get; set; }

        public string WarrantyStart { get; set; }
        public string WarrantyEnd { get; set; }
        public int? BuildingId { get; set; }
        public int? RoomId { get; set; }
        public int? FloorId { get; set; }

        public string OperationDate { get; set; }
        public string ReceivingDate { get; set; }
        public string PONumber { get; set; }
        public decimal? DepreciationRate { get; set; }
        public string CostCenter { get; set; }


        public List<int?> ListOwners { get; set; }
    }
}
