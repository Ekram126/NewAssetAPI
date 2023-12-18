namespace Asset.ViewModels.GovernorateVM
{
    public class EditGovernorateVM
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string NameAr { get; set; }

        public decimal? Population { get; set; }

        public decimal? Area { get; set; }

        public decimal? Latitude { get; set; }
        public decimal? Longtitude { get; set; }

        public string Logo { get; set; }
    }
}
