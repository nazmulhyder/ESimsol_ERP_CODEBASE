using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects.ReportingObject
{
    public class ReceivedChequeHistoryXL 
    {
        public ReceivedChequeHistoryXL()
        {
            SLNo = "";
            PreviousStatus = "";
            CurrentStatus= "";
            OperationBy = "";
            Note = "";
            ChangeLog = "";
            OperationDateTime = "";
            
        }

        #region Properties
        public string SLNo { get; set; }
        public string PreviousStatus { get; set; }
        public string CurrentStatus { get; set; }
        public string OperationBy { get; set; }
        public string Note { get; set; }
        public string ChangeLog { get; set; }
        public string OperationDateTime { get; set; }        
        
        #endregion
    }
}
