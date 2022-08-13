using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects.ReportingObject
{
    public class BalanceSheetShortXL
    {
        public BalanceSheetShortXL()
        { 
           Group = "";
           SubGroup = "";
           Ledger = "";
           LedgerBalance = 0.00;
           GroupBalance = 0.00;
        }

        #region Properties        
        public string Group { get; set; }
        public string SubGroup { get; set; }
        public string Ledger { get; set; }
        public double LedgerBalance { get; set; }
        public double GroupBalance { get; set; }        
        #endregion
    }
}
