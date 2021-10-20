using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class SP_SBPBlotter_Result
    {
        public long DealNo { get; set; }
        public string DataType { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public System.DateTime DealDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal Inflow { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal Outflow { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> OpeningBalance { get; set; }
        public int Recon { get; set; }

    }
}