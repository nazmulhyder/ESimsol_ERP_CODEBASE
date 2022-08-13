using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class PrintDA
    {
        public PrintDA() { }

        #region Insert Function
        public static IDataReader IU(TransactionContext tc, Print oPrint, Int64 nUserId)
        {
            int nCount = 0; string sSQL=""; object obj = null;
            sSQL="Select COUNT(*) from [Print] Where ReportName='"+oPrint.ReportName+"' And ObjectID="+ oPrint.ObjectID+"";
            obj=tc.ExecuteScalar(sSQL);
            nCount=Convert.ToInt32(obj);
            if (nCount <= 0)
            {
                sSQL = "Insert into [Print] values('" + oPrint.ReportCode + "','" + oPrint.ReportName + "'," + oPrint.ObjectID + ",1," + nUserId + ",GETDATE())";
                tc.ExecuteScalar(sSQL);
            }
            else
            {
                sSQL = "Update [Print] Set NumberOfPrint=(Select IsNull((NumberOfPrint),0)+1 from [Print] Where ReportName='" + oPrint.ReportName + "' And ObjectID=" + oPrint.ObjectID + "), DBUserID=" + nUserId + ", DBServerDateTime=GETDATE() Where ReportName='" + oPrint.ReportName + "' And ObjectID=" + oPrint.ObjectID + "";
                tc.ExecuteScalar(sSQL);
            }
            sSQL = "Select * from [Print] Where ReportName='" + oPrint.ReportName + "' And ObjectID=" + oPrint.ObjectID + "";
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
