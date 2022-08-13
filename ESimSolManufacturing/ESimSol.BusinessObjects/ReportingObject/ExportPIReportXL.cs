using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects.ReportingObject
{
    public class ExportPIReportXL
    {
        public ExportPIReportXL()
        {
            SL = "";
            Date = "";
            PINo = "";
            FactoryName = "";
            BuyingHouse = "";
            MKTPName = "";
            ProductName = "";
            Quantity = "";
            UnitPrice = "";
            Amount = "";
            Status = "";
            LCNo = "";
            BankName = "";
          
        }

        #region Properties
        public string SL { get; set; }
        public string Date { get; set; }
        public string PINo { get; set; }
        public string BankName { get; set; }
        public string FactoryName { get; set; }
        public string BuyingHouse { get; set; }
        public string MKTPName { get; set; }
        public string ProductName { get; set; }
        public string Quantity { get; set; }
        public string UnitPrice { get; set; }
        public string Amount { get; set; }
        public string Status { get; set; }
        public string LCNo { get; set; }

        #endregion
    }
}
