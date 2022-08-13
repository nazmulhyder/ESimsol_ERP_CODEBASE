using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects.ReportingObject
{
    public class GeneralJournalXL
    {
        public GeneralJournalXL()
        {
            SLNo = "";
            VoucherDate = "";
            VoucherNo = "";
            AccountCode = "";
            AccountHeadName = "";
            DebitAmount = 0;
            CreditAmount = 0;
        }

        #region Properties
        public string SLNo { get; set; }
        public string VoucherDate { get; set; }
        public string VoucherNo { get; set; }
        public string AccountCode { get; set; }
        public string AccountHeadName { get; set; }
        public double DebitAmount { get; set; }
        public double CreditAmount { get; set; }        
        #endregion
    }
}
