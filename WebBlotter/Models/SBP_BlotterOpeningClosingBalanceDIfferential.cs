using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class SBP_BlotterOpeningClosingBalanceDIfferential
    {
        public long SNo { get; set; }
        public long OpenBalID { get; set; }
        public string DataType { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<decimal> InFlow { get; set; }
        public Nullable<decimal> OutFLow { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public int BR { get; set; }
        public int CurID { get; set; }
        public string Flag { get; set; }
    }
}