using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class SP_GetSBPBlotterRECON_Result
    {
        public long ID { get; set; }
        public long NostroBankId { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> LastStatementDate { get; set; }
        public string OurBooks { get; set; }
        public string TheirBooks { get; set; }
        public Nullable<decimal> ConversionRate { get; set; }
        public Nullable<decimal> EquivalentUSD { get; set; }
        public Nullable<decimal> LimitAvailable { get; set; }
        public int UserID { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public int BR { get; set; }
        public int BID { get; set; }
        public int CurID { get; set; }
        public string Flag { get; set; }
        public string BankName { get; set; }
    }
}