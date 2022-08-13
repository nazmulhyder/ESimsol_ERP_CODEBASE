using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects.ReportingObject
{
    public class VoucherXL
    {
        public VoucherXL()
        {
            SLNo = "";
            VoucherNo = "";
            VoucherName = "";
            Date = "";
            AuthorizedBy = "";
            Amount = 0;
            
        }

        #region Properties
        public string SLNo { get; set; }
        public string VoucherNo { get; set; }
        public string VoucherName { get; set; }
        public string Date { get; set; }
        public string AuthorizedBy { get; set; }
        public double Amount { get; set; }        
            
        #endregion
    }
}
