using Asset.Models;
using System;
using System.Collections.Generic;

namespace Asset.ViewModels.ExternalAssetMovementVM
{
    public class EditExternalAssetMovementVM
    {
        public int Id { get; set; }

        public int AssetDetailId { get; set; }
        public DateTime? MovementDate { get; set; }
        public string HospitalName { get; set; }
        public string Notes { get; set; }
        public string AssetName { get; set; }
        public string AssetNameAr { get; set; }
        public string SerialNumber { get; set; }
        public string BarCode { get; set; }
        public string ModelNumber { get; set; }
        public List<ExternalAssetMovementAttachment> ListMovementAttachments { get; set; }
    }
}
