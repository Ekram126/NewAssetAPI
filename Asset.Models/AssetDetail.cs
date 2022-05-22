using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
    public class AssetDetail
    {
        public int Id { get; set; }
        [StringLength(5)]
        public string Code { get; set; }
        public DateTime? PurchaseDate { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Price { get; set; }


        [StringLength(20)]
        public string SerialNumber { get; set; }

        public string Remarks { get; set; }


        [StringLength(20)]
        public string Barcode { get; set; }

        public DateTime? InstallationDate { get; set; }
        [StringLength(50)]

        public string WarrantyExpires { get; set; }


        public int? BuildingId { get; set; }
        [ForeignKey("BuildingId")]
        public virtual Building Building { get; set; }



        public int? RoomId { get; set; }
        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; }






        public int? FloorId { get; set; }
        [ForeignKey("FloorId")]
        public virtual Floor Floor { get; set; }





        public int? DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }



        public int? SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }

        public int? HospitalId { get; set; }

        [ForeignKey("HospitalId")]
        public virtual Hospital Hospital { get; set; }

        public int? MasterAssetId { get; set; }

        [ForeignKey("MasterAssetId")]
        public virtual MasterAsset MasterAsset { get; set; }



        [DataType(DataType.Date)]
        public DateTime? WarrantyStart { get; set; }

        [DataType(DataType.Date)]
        public DateTime? WarrantyEnd { get; set; }


        [DataType(DataType.Date)]
        public DateTime? OperationDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ReceivingDate { get; set; }

        public string PONumber { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? DepreciationRate { get; set; }

        [StringLength(50)]

        public string CostCenter { get; set; }
        public string QrFilePath { get; set; }

    }
}
