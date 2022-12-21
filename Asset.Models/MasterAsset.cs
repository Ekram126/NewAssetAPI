using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
    public class MasterAsset
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(100)]
        public string NameAr { get; set; }
        [StringLength(5)]
        public string Code { get; set; }
        public string Description { get; set; }
        public string DescriptionAr { get; set; }
        public int? ExpectedLifeTime { get; set; }


        public int? ECRIId { get; set; }
        [ForeignKey("ECRIId")]
        public virtual ECRI ECRIS { get; set; }


        [StringLength(20)]
        public string ModelNumber { get; set; }

        [StringLength(20)]
        public string VersionNumber { get; set; }


        public int? PeriorityId { get; set; }
        [ForeignKey("PeriorityId")]
        public virtual AssetPeriority AssetPeriority { get; set; }



        public int? OriginId { get; set; }
        [ForeignKey("OriginId")]
        public virtual Origin Origin { get; set; }




        public int? BrandId { get; set; }
        [ForeignKey("BrandId")]
        public virtual Brand brand { get; set; }
        public int? PMTimeId { get; set; }

        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }



        public int? SubCategoryId { get; set; }

        [ForeignKey("SubCategoryId")]
        public virtual SubCategory SubCategory { get; set; }



        public double? Length { get; set; }
        public double? Height { get; set; }
        public double? Width { get; set; }
        public double? Weight { get; set; }

        [StringLength(10)]
        public string Power { get; set; }
        [StringLength(10)]
        public string Voltage { get; set; }

        [StringLength(10)]
        public string Ampair { get; set; }

        [StringLength(10)]
        public string Frequency { get; set; }

        [StringLength(10)]
        public string ElectricRequirement { get; set; }


        [StringLength(10)]
        public string PMColor { get; set; }


        [StringLength(10)]
        public string PMBGColor { get; set; }

        [StringLength(50)]
        public string AssetImg { get; set; }

        //public int? HospitalId { get; set; }
        //[ForeignKey("HospitalId")]
        //public virtual Hospital Hospital { get; set; }

    }
}
