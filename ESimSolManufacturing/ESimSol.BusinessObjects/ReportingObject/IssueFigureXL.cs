using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects.ReportingObject
{
    public class IssueFigureXL 
    {
        public IssueFigureXL()
        {
            SLNo = "";
            ChequeIssueTo = "";
            SecondLineIssueTo = "";
        }

        #region Properties
        public string SLNo { get; set; }
        public string ChequeIssueTo { get; set; }
        public string SecondLineIssueTo { get; set; }
        #endregion
    }
}
