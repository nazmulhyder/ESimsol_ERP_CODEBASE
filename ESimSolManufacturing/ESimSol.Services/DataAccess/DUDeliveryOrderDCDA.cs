using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using ICS.Core.DataAccess;
using System.Data;

namespace ESimSol.Services.DataAccess
{
    public class DUDeliveryOrderDCDA
    {
        public DUDeliveryOrderDCDA() { }

        #region Insert Update Delete Function
       
        #endregion


        #region

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_DUDeliveryOrderDC WHERE DUDeliveryOrderID=%n", nID);
        }
     
        public static IDataReader GetsByNo(TransactionContext tc, string sOrderNo)
        {
            return tc.ExecuteReader("Select * from View_DUDeliveryOrderDC where OrderNo like '%" + sOrderNo + "'");
        }
        public static IDataReader GetsBy(TransactionContext tc, string sContractorID)
        {
            return tc.ExecuteReader("Select * from View_DUDeliveryOrderDC where ContractorID in (%q) and SampleInvoiceID=0", sContractorID);
        }
        public static IDataReader GetsByPI(TransactionContext tc, int nExportPIID)
        {
            return tc.ExecuteReader("Select * from View_DUDeliveryOrderDC where ExportPIID=%n", nExportPIID);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

     
        #endregion
    }
}
