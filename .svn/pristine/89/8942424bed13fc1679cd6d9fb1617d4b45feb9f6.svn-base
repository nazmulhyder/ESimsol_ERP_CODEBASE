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
    public class ImportLetterSetupDA
    {
        public ImportLetterSetupDA() { }

        #region Insert Function

        public static IDataReader InsertUpdate(TransactionContext tc, ImportLetterSetup oILSetup, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ImportLetterSetup]"
                                    + " %n,%n,%n,%n,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%n,%n,%n,%n,%n,%n,%n",
             oILSetup.ImportLetterSetupID
            , oILSetup.BUID 
            , (int)oILSetup.LetterTypeInt
            , oILSetup.IssueToType
            , oILSetup.LetterName
            , oILSetup.RefNo
            , oILSetup.To
            , oILSetup.ToName
            , oILSetup.Subject
            , oILSetup.SubjectTwo
            , oILSetup.DearSir
            , oILSetup.Body1
            , oILSetup.Body2
            , oILSetup.Body3
            , oILSetup.For
            , oILSetup.ForName
            , oILSetup.ThankingOne
            , oILSetup.ThankingTwo
            , oILSetup.Authorize1
            , oILSetup.Authorize2
            , oILSetup.Authorize3
            , oILSetup.SupplierName
            , oILSetup.PIBank
            , oILSetup.LCNo
            , oILSetup.LCValue
            , oILSetup.Clause
            , oILSetup.InvoiceNo
            , oILSetup.InvoiceValue
            , oILSetup.MasterLCNo
             , oILSetup.LCPayType
              , oILSetup.BLNo 
            , oILSetup.IsPrintAddress
            , oILSetup.IsPrintDateCurrentDate
            , oILSetup.IsPrintDateObject
            , oILSetup.IsAutoRefNo
            , oILSetup.Authorize1IsAuto
            , oILSetup.Authorize2IsAuto
            , oILSetup.Authorize3IsAuto
            , oILSetup.Activity
            , oILSetup.IsPrintProductName
            , oILSetup.IsPrintPINo
            , oILSetup.IsPrinTnC
            , oILSetup.IsPrintSupplierAddress
            , oILSetup.IsPrintPIBankAddress
            , oILSetup.IsPrintMaturityDate
            , oILSetup.IsCalMaturityDate 
            ,oILSetup.LCAppTypeInt
            , oILSetup.LCPaymentTypeInt
            , oILSetup.ProductType
            , oILSetup.BankBranchID
            , oILSetup.HeaderType
            , nUserId,
              (int)eEnumDBOperation);

        }

        public static void UpdateSequence(TransactionContext tc, ImportLetterSetup oImportLetterSetup)
        {
            tc.ExecuteNonQuery("UPDATE ImportLetterSetup SET  Sequence=%n WHERE ImportLetterSetupID=%n", oImportLetterSetup.Sequence, oImportLetterSetup.ImportLetterSetupID);
        }
        #endregion

        #region Delete Function
        public static void Delete(TransactionContext tc, ImportLetterSetup oILSetup, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ImportLetterSetup]"
                                    + " %n,%n,%n,%n,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%n,%n,%n,%n,%n,%n,%n",
              oILSetup.ImportLetterSetupID
            , oILSetup.BUID
            , (int)oILSetup.LetterTypeInt
            , oILSetup.IssueToType
            , oILSetup.LetterName
            , oILSetup.RefNo
            , oILSetup.To
            , oILSetup.ToName
            , oILSetup.Subject
            , oILSetup.SubjectTwo
            , oILSetup.DearSir
            , oILSetup.Body1
            , oILSetup.Body2
            , oILSetup.Body3
            , oILSetup.For
            , oILSetup.ForName
            , oILSetup.ThankingOne
            , oILSetup.ThankingTwo
            , oILSetup.Authorize1
            , oILSetup.Authorize2
            , oILSetup.Authorize3
            , oILSetup.SupplierName
            , oILSetup.PIBank
            , oILSetup.LCNo
            , oILSetup.LCValue
            , oILSetup.Clause
            , oILSetup.InvoiceNo
            , oILSetup.InvoiceValue
            , oILSetup.MasterLCNo
             , oILSetup.LCPayType
              , oILSetup.BLNo
            , oILSetup.IsPrintAddress
            , oILSetup.IsPrintDateCurrentDate
            , oILSetup.IsPrintDateObject
            , oILSetup.IsAutoRefNo
            , oILSetup.Authorize1IsAuto
            , oILSetup.Authorize2IsAuto
            , oILSetup.Authorize3IsAuto
            , oILSetup.Activity
            , oILSetup.IsPrintProductName
            , oILSetup.IsPrintPINo
            , oILSetup.IsPrinTnC
            , oILSetup.IsPrintSupplierAddress
            , oILSetup.IsPrintPIBankAddress
            , oILSetup.IsPrintMaturityDate
            , oILSetup.IsCalMaturityDate
            , oILSetup.LCAppTypeInt
            , oILSetup.LCPaymentTypeInt
            , oILSetup.ProductType
            , oILSetup.BankBranchID
            , oILSetup.HeaderType
            , nUserId,
              (int)eEnumDBOperation);

        }
        #endregion

        #region Generation Function
      
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportLetterSetup WHERE ImportLetterSetupID=%n ORDER BY Sequence ASC", nID);
        }
        public static IDataReader Get(TransactionContext tc, int nLetterType, int nIssueToType, int nImportLCID, string sSQL)
        {
            return tc.ExecuteReader("Select top(1)* from View_ImportLetterSetup where   Activity=1 and LetterType=%n and IssueToType=%n and ImportLetterSetupID in (Select LetterSetupID from LetterMapping where RefID=%n) %q", nLetterType, nIssueToType, nImportLCID, sSQL);
        }
        public static IDataReader Get(TransactionContext tc, int nLetterType, int nIssueToType, int nBUID, int nImportLCID, string sSQL)
        {
            return tc.ExecuteReader("Select top(1)* from View_ImportLetterSetup where   Activity=1 and LetterType=%n and IssueToType=%n and BUID=%n and ImportLetterSetupID in (Select ImportLetterSetupID from ImportLetterMapping where ImportLCID=%n) %q", nLetterType, nIssueToType, nBUID, nImportLCID, sSQL);
        }
        public static IDataReader GetBy(TransactionContext tc, int nLetterType, int nIssueToType, int nBUID, int nImportLCID, string sSQL)
        {
            return tc.ExecuteReader("Select top(1)* from View_ImportLetterSetup where   Activity=1 and LetterType=%n and IssueToType=%n and BUID=%n  %q", nLetterType, nIssueToType, nBUID,  sSQL);
        }
        public static IDataReader GetForIPR(TransactionContext tc, int nLetterType, int nIssueToType, int nBUID, int nIPRID, string sSQL)
        {
            return tc.ExecuteReader("Select top(1)* from View_ImportLetterSetup where   Activity=1 and LetterType=%n and IssueToType=%n and BUID=%n and ImportLetterSetupID in (Select ImportLetterSetupID from ImportLetterMapping where ImportLCID in (Select ImportInvoice.ImportLCID from ImportInvoice where ImportInvoiceID in (Select ImportInvoiceID from ImportPaymentRequestDetail where ImportPaymentRequestID=%n  ))) %q", nLetterType, nIssueToType, nBUID, nIPRID, sSQL);
        }
        public static IDataReader Gets(TransactionContext tc, bool bActivity, int nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportLetterSetup WHERE Activity=%b AND BUID = %n ORDER BY Sequence ASC", bActivity, nBUID);
        }
        public static IDataReader BUWiseGets(TransactionContext tc, int nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportLetterSetup WHERE BUID=%n ORDER BY Sequence ASC", nBUID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportLetterSetup ORDER BY Sequence ASC");
        }

        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader Activate(TransactionContext tc, ImportLetterSetup oImportLetterSetup)
        {
            string sSQL1 = SQLParser.MakeSQL("Update ImportLetterSetup Set Activity=~Activity WHERE ImportLetterSetupID=%n", oImportLetterSetup.ImportLetterSetupID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_ImportLetterSetup WHERE ImportLetterSetupID=%n", oImportLetterSetup.ImportLetterSetupID);

        }
        #endregion
    }


}
