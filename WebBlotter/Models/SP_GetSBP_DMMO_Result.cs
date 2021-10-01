using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class SP_GetSBP_DMMO_Result
    {
        public int SNo { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<decimal> PakistanBalance { get; set; }
        public Nullable<decimal> SBPBalanace { get; set; }
        public Nullable<decimal> BalanceDifference { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> CurID { get; set; }
        public Nullable<int> BR { get; set; }
        public Nullable<int> BID { get; set; }
        public string Flag { get; set; }
    }
}