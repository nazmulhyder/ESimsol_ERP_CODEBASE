using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects.ReportingObject
{
    public class CostCenterDetailXL
    {
        public CostCenterDetailXL()
        {
            SLNo = "";
            VoucherDate = "";
            VoucherNo = "";
            Particulars = "";
            DebitAmount = 0;
            CreditAmount = 0;
            ClosingValueDebitCredit = "";
            ClosingValue = 0;
        }

        #region Properties
        public string SLNo { get; set; }
        public string VoucherDate { get; set; }
        public string VoucherNo { get; set; }
        public string Particulars { get; set; }
        public double DebitAmount { get; set; }
        public double CreditAmount { get; set; }
        public string ClosingValueDebitCredit { get; set; }
        public double ClosingValue { get; set; }
        #endregion
    }
}
