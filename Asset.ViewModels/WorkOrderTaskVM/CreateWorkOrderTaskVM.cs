using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.WorkOrderTaskVM
{
    public class CreateWorkOrderTaskVM
    {
        public int WorkOrderId { get; set; }
        public List<CreateTasks> LstCreateTasks { get; set; }

    }
}
