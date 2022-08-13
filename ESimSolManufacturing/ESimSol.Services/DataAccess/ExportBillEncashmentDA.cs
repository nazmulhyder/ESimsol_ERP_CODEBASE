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
    public class ExportBillEncashmentDA
    {
        public ExportBillEncashmentDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ExportBillEncashment oEBillE, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sExportBillEncashmentIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportBillEncashment]" + "%n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %s",
                                    oEBillE.ExportBillEncashmentID, oEBillE.ExportBillID, oEBillE.AccountHeadID, oEBillE.SubledgerID, oEBillE.LoanInstallmentID, oEBillE.CurrencyID, oEBillE.CCRate, oEBillE.Amount, oEBillE.SLNo, nUserID, (int)eEnumDBOperation, sExportBillEncashmentIDs);
        }
        public static void Delete(TransactionContext tc, ExportBillEncashment oEBillE, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sExportBillEncashmentIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ExportBillEncashment]" + "%n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %s",
                                    oEBillE.ExportBillEncashmentID, oEBillE.ExportBillID, oEBillE.AccountHeadID, oEBillE.SubledgerID, oEBillE.LoanInstallmentID, oEBillE.CurrencyID, oEBillE.CCRate, oEBillE.Amount, oEBillE.SLNo, nUserID, (int)eEnumDBOperation, sExportBillEncashmentIDs);
        }
       
       
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportBillEncashment WHERE ExportBillEncashmentID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc,int nEBillID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportBillEncashment where ExportBillID=%n", nEBillID);
        }
           
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
