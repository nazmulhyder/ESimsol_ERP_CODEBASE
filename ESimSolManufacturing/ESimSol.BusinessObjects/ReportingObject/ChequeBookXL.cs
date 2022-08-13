using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects.ReportingObject
{
    public class ChequeBookXL 
    {
        public ChequeBookXL()
        {
            SLNo = "";
            AccountNo = "";
            BookCode = "";
            PageCount = "";
            BankName = "";
            BankBranchName = "";
            CompanyName = "";
            
        }

        #region Properties
        public string SLNo { get; set; }
        public string AccountNo { get; set; }
        public string BookCode { get; set; }
        public string PageCount { get; set; }
        public string BankName { get; set; }
        public string BankBranchName { get; set; }
        public string CompanyName { get; set; }        
        
        #endregion
    }
}
