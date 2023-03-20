using Itenso.TimePeriod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.API.Helpers
{
    public class FiscalTimeCalendar:TimeCalendar
    {
        // ----------------------------------------------------------------------
        public FiscalTimeCalendar()
          : base(
            new TimeCalendarConfig
            {
                YearBaseMonth = YearMonth.July,  //  October year base month
                YearWeekType = YearWeekType.Iso8601, // ISO 8601 week numbering
                YearType = YearType.FiscalYear // treat years as fiscal years
            })
        {
        } // FiscalTimeCalendar

    }
}
