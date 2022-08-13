using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.DataAccess
{
    public class ExportCommercialDocDA
    {
        public ExportCommercialDocDA() { }

        #region Insert Function
      
        #endregion

        #region Delete Function
     

        #endregion
               

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nExportBillID)
        {
            return tc.ExecuteReader("SELECT * FROM [View_ExportCommercialDoc] as ExportCommercialDoc  WHERE ExportBillID=%n", nExportBillID);
        }

        public static IDataReader GetForBuying(TransactionContext tc, int nCommercialInvoiceID)
        {
            return tc.ExecuteReader("SELECT * FROM [View_ExportCommercialDoc_ForBuying] as ExportCommercialDoc  WHERE CommercialInvoiceID=%n", nCommercialInvoiceID);
        }


        #endregion

    }
}
