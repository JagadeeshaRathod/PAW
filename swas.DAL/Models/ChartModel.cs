using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace swas.DAL.Models
{

    ///Developer Name :- Sub Maj M Sanal Kumar

    ///Revised on :- 19,20 & 26, 27 Aug 2023  ---    pie chart incorrect data shown fixed by using sp json
    /// Revised on :- 07& 08 Oct  2023  ---    removed sp json and corrected BAL and view



    public class ChartModel
    {

        [Key]
        public int serno { get; set; }
        public string? AppType { get; set; }
        public string? MonthStart { get; set; }

        public int? AppTypeCount { get; set; }

        public string? AppDesc { get; set; }
        public string? MonthName { get; set; }

    }

    public class ChartModelS
    {
        [Key]
        public int serno { get; set; }
        public int? unitid { get; set; }
        public string? MonthNames { get; set; }
        public string? MonthNameYr { get; set; }
        public int? TotalIn { get; set; }
        public int? TotalOut { get; set; }

        public string? unitname { get; set; }



    }
}
