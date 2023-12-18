using System.Collections.Generic;

namespace Asset.ViewModels.AssetDetailVM
{
    public class DrawChart
    {
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationNameAr { get; set; }


        public List<DrawBarChart> ListBars { get; set; }
    }

    public class DrawBarChart
    {
        public int GovernorateId { get; set; }
        public string GovernorateName { get; set; }
        public string GovernorateNameAr { get; set; }

        public int AssetCount { get; set; }
    }
}
