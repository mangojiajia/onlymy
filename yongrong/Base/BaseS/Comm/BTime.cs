using System;
using System.Collections.Generic;
using System.Text;

namespace BaseS.Comm
{
    public static class BTime
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtime"></param>
        /// <returns></returns>
        public static DateTime ToTimeZone(DateTime dtime)
        {
            var ts = DateTime.Now - dtime;

            if (7.3 <= ts.TotalHours && 8.7 >= ts.TotalHours)
            {
                return dtime.AddHours(8.0);
            }

            return dtime;
        }
    }
}
