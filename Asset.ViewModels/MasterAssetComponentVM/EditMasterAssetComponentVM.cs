namespace Asset.ViewModels.MasterAssetComponentVM
{
    public class EditMasterAssetComponentVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameAr { get; set; }
        public string Code { get; set; }
        public string PartNo { get; set; }
        public string Description { get; set; }
        public string DescriptionAr { get; set; }
        public int? MasterAssetId { get; set; }
        public decimal? Price { get; set; }
    }
}
