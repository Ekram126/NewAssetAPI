using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
    public class AssetMovement
    {
        public int Id { get; set; }
        public int AssetDetailId { get; set; }

        public DateTime? MovementDate { get; set; }

        public int? BuildingId { get; set; }
        public int? FloorId { get; set; }

        public int? RoomId { get; set; }
        [StringLength(500)]
        public string MoveDesc { get; set; }

    }
}
