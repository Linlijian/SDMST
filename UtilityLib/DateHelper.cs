using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityLib
{
    public static class DateHelper
    {

        public const string DateFormat = "{0:dd/MM/yyyy}";
        public static bool CompareDate(this DateTime? start, DateTime? end)
        {
            if (start == null || end == null) return true;
            var startDate = Convert.ToDateTime(start);
            var endDate = Convert.ToDateTime(end);
            return DateTime.Compare(endDate, startDate) > 0;
        }

    }
}
