using Asset.ViewModels.PMAssetTaskVM;
using System;
using System.Collections.Generic;

namespace Asset.ViewModels.WNPMAssetTimes
{
    public class CalendarWNPMAssetTimeVM
    {
        public int Id { get; set; }
        public int MasterAssetId { get; set; }
        public string AssetName { get; set; }
        public string AssetNameAr { get; set; }
        public string SerialNumber { get; set; }
        public string ModelNumber { get; set; }
        public string BarCode { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentNameAr { get; set; }

        public int? HospitalId { get; set; }
        public int? DepartmentId { get; set; }
        public DateTime? PMDate { get; set; }
        public int? AssetDetailId { get; set; } 
        public DateTime? DoneDate { get; set; }
        public bool? IsDone { get; set; }
        public DateTime? DueDate { get; set; }
        public string Comment { get; set; }

        public string start { get; set; }
        public string end { get; set; }
        public bool allDay { get; set; }
        public string title { get; set; }
        public string titleAr { get; set; }
        public string color { get; set; }
        public string textColor { get; set; }


        public List<IndexPMAssetTaskVM.GetData> ListMasterAssetTasks { get; set; }
       
    }    
}
