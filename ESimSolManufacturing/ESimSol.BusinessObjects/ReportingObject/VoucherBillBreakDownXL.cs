using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects.ReportingObject
{
    public class VoucherBillBreakDownXL
    {
        public VoucherBillBreakDownXL()
        {
            SLNo = "";
            BillNo = "";
            OpeningValueDebitCredit = "";
            OpeningValue = 0;
            DebitAmount = 0;
            CreditAmount = 0;
            ClosingValueDebitCredit = "";
            ClosingValue = 0;
        }

        #region Properties
        public string SLNo { get; set; }
        public string BillNo { get; set; }
        public string OpeningValueDebitCredit { get; set; }
        public double OpeningValue { get; set; }
        public double DebitAmount { get; set; }
        public double CreditAmount { get; set; }
        public string ClosingValueDebitCredit { get; set; }
        public double ClosingValue { get; set; }
        #endregion
    }
}
