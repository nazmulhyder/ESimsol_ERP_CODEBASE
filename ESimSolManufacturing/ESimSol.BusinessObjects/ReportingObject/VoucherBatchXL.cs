using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects.ReportingObject
{
    public class VoucherBatchXL
    {
        public VoucherBatchXL()
        {
            SLNo = "";
            BatchNO = "";
            BatchStatusInString = "";
            CreateByName = "";
            RequestToName = "";
        }

        #region Properties
        public string SLNo { get; set; }
        public string BatchNO { get; set; }
        public string BatchStatusInString { get; set; }
        public string CreateByName { get; set; }
        public string RequestToName { get; set; }
       
        #endregion
    }
}
