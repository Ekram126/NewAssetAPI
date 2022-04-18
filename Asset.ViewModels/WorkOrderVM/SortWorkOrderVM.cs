using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.WorkOrderVM
{
    public class SortWorkOrderVM
    {
        public string WorkOrderNumber { get; set; }
        public string StatusName { get; set; }
        public string StatusNameAr { get; set; }
        public string Subject { get; set; }
        public string CreatedBy { get; set; }
        public string Note { get; set; }
        public string CreationDate { get; set; }
        public string ClosedDate { get; set; }
        public string ElapsedTime { get; set; }
        public string RequestSubject { get; set; }
        public string SortStatus { get; set; }
        public string AssetName { get; set; }
        public string AssetNameAr { get; set; }
        public string Barcode { get; set; }
        public string SerialNumber { get; set; }
        public string ModelNumber { get; set; }

        public string StrSerial { get; set; }
        public string StrRequestSubject { get; set; }
        public string StrSubject { get; set; }
        public string StrWorkOrderNumber { get; set; }
        public string StrBarCode { get; set; }
        public string StrModel { get; set; }

        public int? PeriorityId { get; set; }
        public int? StatusId { get; set; }

    }
}
