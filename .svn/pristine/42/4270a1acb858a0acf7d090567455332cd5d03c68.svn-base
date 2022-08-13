using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ExportSCDODA
    {
        public ExportSCDODA() { }

        #region Insert Update Delete Function
  
      
      
      
        #endregion

      

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportSC_DO WHERE ExportSCID=%n", nID);
        }
     
        public static IDataReader Get(TransactionContext tc, string sPINo, int nTexTileUnit)
        {
            int nCount = 0;
            if (sPINo.Length > 15) { 
                nCount = 15;
                return tc.ExecuteReader("SELECT * FROM View_ExportSC_DO WHERE LEFT(PINo," + nCount + ") LIKE '%" + sPINo.Split('/')[0] + "/" + sPINo.Split('/')[1] + "/" + sPINo.Split('/')[2] + "/" + sPINo.Split('/')[3] + "%' AND RIGHT(PINo,3)='/" + sPINo.Split('/')[4] + "' AND TextileUnit=" + nTexTileUnit);

            }
            else { 
                nCount = 13;
                return tc.ExecuteReader("SELECT * FROM View_ExportSC_DO WHERE LEFT(PINo," + nCount + ") LIKE '%" + sPINo.Split('/')[0] + "/" + sPINo.Split('/')[1] + "/" + sPINo.Split('/')[2] + "%' AND RIGHT(PINo,3)='/" + sPINo.Split('/')[3] + "' AND TextileUnit=" + nTexTileUnit);
            }
        }
        public static IDataReader GetByPI(TransactionContext tc,  int nExportPIID)
        {
            return tc.ExecuteReader("Select * from View_ExportSC_DO  where ExportPIID=%n", nExportPIID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("Select * from View_ExportSC_DO where TotalQty>DOQty and PIStatus in (0,1,2,3,4,5)");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}
