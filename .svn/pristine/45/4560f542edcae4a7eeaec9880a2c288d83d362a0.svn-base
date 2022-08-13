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
    public class DUDeliveryLotDA
    {
        public DUDeliveryLotDA() { }

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, int nOrderType, int nWorkingUnitID, int nReportType)
        {
            return tc.ExecuteReader("EXEC [sp_DUDeliveryLot]" + "%n,%n, %n", nOrderType, nWorkingUnitID, nReportType);
        }
        public static IDataReader GetsFromAdv(TransactionContext tc, string sSQL, int nReportType)
        {
            return tc.ExecuteReader("EXEC [sp_DUDeliveryLotTwo]" + "%s, %n", sSQL, nReportType);
        }
        public static IDataReader GetsDUHardWindingStock(TransactionContext tc, string sSQL, int nReportType)
        {
            return tc.ExecuteReader("EXEC [sp_DUHardWindingStock]" + "%s,%n", sSQL,nReportType);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }  
    
   
}
