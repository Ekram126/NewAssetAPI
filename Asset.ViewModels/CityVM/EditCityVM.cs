namespace Asset.ViewModels.CityVM
{
    public class EditCityVM
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string NameAr { get; set; }

        public int GovernorateId { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longtitude { get; set; }

    }
}
