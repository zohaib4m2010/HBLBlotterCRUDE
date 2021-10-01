﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class SBP_BlotterCRRFINCON
    {
        //public long SNo { get; set; }

        //[Required]
        //[DataType(DataType.Date, ErrorMessage = "Date only")]
        //[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        //public Nullable<System.DateTime> StartDate { get; set; }

        //[Required]
        //[DataType(DataType.Date, ErrorMessage = "Date only")]
        //[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        //public Nullable<System.DateTime> EndDate { get; set; }

        //[Required]
        //[DisplayFormat(DataFormatString = "{0:N2}")]
        //public Nullable<decimal> DemandTimeLiablities { get; set; }
        //[Required]
        //[DisplayFormat(DataFormatString = "{0:N2}")]
        //public Nullable<decimal> TimeLiablitiesOverOneYear { get; set; }
        //[DisplayFormat(DataFormatString = "{0:N2}")]
        //public Nullable<decimal> DemandTimeLiablitiesTotal { get; set; }
        //[Required]
        //[DisplayFormat(DataFormatString = "{0:N2}")]
        //public Nullable<decimal> DepositEligibleFor { get; set; }
        //[Required]
        //[DisplayFormat(DataFormatString = "{0:N2}")]
        //public Nullable<decimal> OtherAmounts { get; set; }
        //[DisplayFormat(DataFormatString = "{0:N2}")]
        //public Nullable<decimal> TotalEligibleForCRR { get; set; }
        //public int UserID { get; set; }
        //public Nullable<System.DateTime> CreateDate { get; set; }
        //public Nullable<System.DateTime> UpdateDate { get; set; }
        //public int BR { get; set; }
        //public int BID { get; set; }
        //public int CurID { get; set; }
        //public string Flag { get; set; }


        public long SNo { get; set; }
        [Required]
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> StartDate { get; set; }
        [Required]
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> EndDate { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Nullable<decimal> DemandTimeLiablities { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Nullable<decimal> TimeLiablitiesOverOneYear { get; set; }
  
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Nullable<decimal> DemandTimeLiablitiesTotal { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Nullable<decimal> PreMatureDeposit { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Nullable<decimal> DemandTimeLiablitiesTotalForCRR { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Nullable<decimal> Penalty { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Nullable<decimal> ExtraBenefits { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Nullable<decimal> CRR1Requirement { get; set; }
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Nullable<decimal> CRR2Requirement { get; set; }
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Nullable<decimal> RequirementPenalty { get; set; }
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Nullable<decimal> RequirementExtBenefit { get; set; }
        [DisplayFormat(DataFormatString = "{0:N4}")]
        public Nullable<decimal> BalMaintAgainstPenalty { get; set; }
        [DisplayFormat(DataFormatString = "{0:N4}")]
        public Nullable<decimal> BalMaintAgainstExtBenft { get; set; }
        public int UserID { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public int BR { get; set; }
        public int BID { get; set; }
        public int CurID { get; set; }
        public string Flag { get; set; }
    }
}