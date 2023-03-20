using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
    public class AssetMovement
    {
        public int Id { get; set; }

        public int AssetDetailId { get; set; }
        [ForeignKey("AssetDetailId")]
        public virtual AssetDetail AssetDetail { get; set; }



        public DateTime? MovementDate { get; set; }

        public int? BuildingId { get; set; }
        [ForeignKey("BuildingId")]
        public virtual Building Building { get; set; }



        public int? RoomId { get; set; }
        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; }


        public int? FloorId { get; set; }
        [ForeignKey("FloorId")]
        public virtual Floor Floor { get; set; }


        [StringLength(500)]
        public string MoveDesc { get; set; }


        public int? HospitalId { get; set; }
        [ForeignKey("HospitalId")]
        public virtual Hospital Hospital { get; set; }

    }
}
