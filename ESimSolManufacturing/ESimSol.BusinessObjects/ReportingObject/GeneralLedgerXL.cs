using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects.ReportingObject
{
    public class GeneralLedgerXL
    {
        public GeneralLedgerXL()
        {
            SLNo = "";
            VoucherDate = "";
            VoucherNo = "";
            Particulars = "";
            DebitAmount = 0;
            CreditAmount = 0;
            CurrentBalance = 0;
        }

        #region Properties
        public string SLNo { get; set; }
        public string VoucherDate { get; set; }
        public string VoucherNo { get; set; }
        public string Particulars { get; set; }
        public double DebitAmount { get; set; }
        public double CreditAmount { get; set; }
        public double CurrentBalance { get; set; }
        #endregion
    }
}
