namespace Asset.ViewModels.MasterAssetVM
{
    public class ViewMasterAssetVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameAr { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string DescriptionAr { get; set; }
        public int ExpectedLifeTime { get; set; }    
        public string ModelNumber { get; set; }
        public string VersionNumber { get; set; }
        public double? Length { get; set; }
        public double? Height { get; set; }
        public double? Width { get; set; }
        public double? Weight { get; set; }
        public string Power { get; set; }
        public string Voltage { get; set; }
        public string Ampair { get; set; }
        public string Frequency { get; set; }
        public string ElectricRequirement { get; set; }
        public string AssetImg { get; set; }


        public int? PMTimeId { get; set; }
        public int? ECRIId { get; set; }
        public int? PeriorityId { get; set; }
        public int? OriginId { get; set; }
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public int? HospitalId { get; set; }

        public string PMTimeName { get; set; }
        public string ECRIName { get; set; }
        public string PeriorityName { get; set; }
        public string OriginName { get; set; }
        public string BrandName { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }




        public string PMTimeNameAr { get; set; }
        public string ECRINameAr { get; set; }
        public string PeriorityNameAr { get; set; }
        public string OriginNameAr { get; set; }
        public string BrandNameAr { get; set; }
        public string CategoryNameAr { get; set; }
        public string SubCategoryNameAr { get; set; }


    }
}
