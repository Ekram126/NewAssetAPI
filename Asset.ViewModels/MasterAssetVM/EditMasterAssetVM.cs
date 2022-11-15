namespace Asset.ViewModels.MasterAssetVM
{
    public class EditMasterAssetVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameAr { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string DescriptionAr { get; set; }
        public int? ExpectedLifeTime { get; set; }

        public int? ECRIId { get; set; }
        public string Model{ get; set; }
        public string ModelNumber { get; set; }
        public string VersionNumber { get; set; }
        public int? PeriorityId { get; set; }
        public int? OriginId { get; set; }
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public double? Length { get; set; }
        public double? Height { get; set; }
        public double? Width { get; set; }
        public double? Weight { get; set; }

        public string Power { get; set; }
        public string Voltage { get; set; }
        public string Ampair { get; set; }
        public string Frequency { get; set; }
        public string ElectricRequirement { get; set; }

        public int? PMTimeId { get; set; }

        public string AssetImg { get; set; }

        public int? HospitalId { get; set; }


        public string BrandName { get; set; }
        public string BrandNameAr { get; set; }
    }
}
