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
    public class Export_LDBPDA
    {
        public Export_LDBPDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, Export_LDBP oExport_LDBP, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_Export_LDBP]" + "%n, %s, %n, %d, %n, %s,%n, %n, %n",
                                    oExport_LDBP.Export_LDBPID, oExport_LDBP.RefNo, oExport_LDBP.BankAccountID, oExport_LDBP.LetterIssueDate,  oExport_LDBP.CurrencyType, oExport_LDBP.Note, oExport_LDBP.BUID, nUserID, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, Export_LDBP oExport_LDBP, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_Export_LDBP]" + "%n, %s, %n, %d, %n, %s,%n, %n, %n",
                                    oExport_LDBP.Export_LDBPID, oExport_LDBP.RefNo, oExport_LDBP.BankAccountID, oExport_LDBP.LetterIssueDate, oExport_LDBP.CurrencyType, oExport_LDBP.Note, oExport_LDBP.BUID, nUserID, (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_Export_LDBP WHERE Export_LDBPID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_Export_LDBP ORDER BY RefNo");
        }
        public static IDataReader WaitForApproval(TransactionContext tc,int nBUID)
        {
            string sSQL = "";
            if (nBUID>0)
            { sSQL = "SELECT * FROM View_Export_LDBP WHERE ISNULL(RequestBy,0)!=0 AND ISNULL(ApprovedBy,0)=0  ORDER BY RefNo"; }
            else { sSQL = "SELECT * FROM View_Export_LDBP WHERE ISNULL(RequestBy,0)!=0 AND ISNULL(ApprovedBy,0)=0 and BUID=" + nBUID + "  ORDER BY RefNo"; }
            return tc.ExecuteReader(sSQL);
        }        
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
