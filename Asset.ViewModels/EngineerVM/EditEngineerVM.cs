using System;

namespace Asset.ViewModels.EngineerVM
{
    public class EditEngineerVM
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public string NameAr { get; set; }

        public string CardId { get; set; }
        public string Phone { get; set; }
        public string WhatsApp { get; set; }
        public string Dob { get; set; }
        public string EmpImg { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string AddressAr { get; set; }
     

        public int? GenderId { get; set; }

    }
}
