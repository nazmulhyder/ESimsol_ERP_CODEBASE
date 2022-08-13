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
    public class TransferPromotionIncrementDA
    {
        public TransferPromotionIncrementDA() { }

        #region Insert Update Delete Function
        
        public static IDataReader IUD(TransactionContext tc, TransferPromotionIncrement oTransferPromotionIncrement, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_TransferPromotionIncrement] %n,%n,%n,%n,%n,%n,%n,%b,%b,%b,%n,%n,%n,%n,%n,%n,%b,%s,%d,%n,%n,%n,%n,%n,%b,%b,%n",
                   oTransferPromotionIncrement.TPIID, oTransferPromotionIncrement.EmployeeID,
                   oTransferPromotionIncrement.DesignationID, oTransferPromotionIncrement.DRPID,
                   oTransferPromotionIncrement.ASID, oTransferPromotionIncrement.SalarySchemeID,
                   oTransferPromotionIncrement.GrossSalary, oTransferPromotionIncrement.IsTransfer,
                   oTransferPromotionIncrement.IsPromotion, oTransferPromotionIncrement.IsIncrement,
                   oTransferPromotionIncrement.TPIDesignationID, oTransferPromotionIncrement.TPIDRPID,
                   oTransferPromotionIncrement.TPIASID, oTransferPromotionIncrement.TPISalarySchemeID, oTransferPromotionIncrement.TPIShiftID,
                   oTransferPromotionIncrement.TPIGrossSalary, oTransferPromotionIncrement.TPIIsFixedAmount,
                   oTransferPromotionIncrement.Note, oTransferPromotionIncrement.EffectedDate,
                   nUserID, nDBOperation,oTransferPromotionIncrement.CompTPIGrossSalary,oTransferPromotionIncrement.EmployeeTypeID, oTransferPromotionIncrement.TPIEmployeeTypeID, oTransferPromotionIncrement.IsNoHistory, oTransferPromotionIncrement.IsCashFixed, oTransferPromotionIncrement.CashAmount);


        }

        public static IDataReader AttScheme(TransactionContext tc, TransferPromotionIncrement oTransferPromotionIncrement, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_Update_AttScheme] %n,%n,%n,%n",
                   oTransferPromotionIncrement.EmployeeID,
                   oTransferPromotionIncrement.TPIASID,
                   oTransferPromotionIncrement.TPIShiftID, 
                   nUserID);


        }
        
        public static IDataReader IUDQuick(TransactionContext tc, TransferPromotionIncrement oTPI, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_TransferPromotionQuick] %n,%s,%b,%b,%n,%n,%n,%n,%s,%n,%n",
                   oTPI.TPIID, oTPI.EmployeeIDs,
                   oTPI.IsTransfer, oTPI.IsPromotion,
                   oTPI.TPIDesignationID, oTPI.TPIDRPID,
                   oTPI.TPIASID, oTPI.TPIShiftID,
                   oTPI.Note, nUserID, nDBOperation);

        }
        public static IDataReader GetsIncrementByPercent(TransactionContext tc, string sEmployeeIDs, int nSalaryHeadID, int nPercent, string sMonthIDs, string sYearIDs, Int64 nUserID, string sBUIDs, string sLocationIDs, int nIndex)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_IncrementByPercent] %s,%n,%n,%s,%s,%n, %n, %s, %s",
                   sEmployeeIDs,
                   nSalaryHeadID,
                   nPercent,
                   sMonthIDs,
                   sYearIDs,
                   nUserID,
                   nIndex,
                   sBUIDs,
                   sLocationIDs
                   );

        }
        public static IDataReader Recommend(int nTPIID, Int64 nUserID, TransactionContext tc)
        {
            return tc.ExecuteReader("UPDATE TransferPromotionIncrement SET RecommendedBy=%n,RecommendedByDate=%d WHERE TPIID=%n;SELECT * FROM View_TransferPromotionIncrement WHERE TPIID=%n", nUserID, DateTime.Now, nTPIID, nTPIID);
        }
        public static IDataReader Approve(int nTPIID, Int64 nUserID, TransactionContext tc)
        {
            return tc.ExecuteReader("UPDATE TransferPromotionIncrement SET ApproveBy=%n,ApproveByDate=%d WHERE TPIID=%n;SELECT * FROM View_TransferPromotionIncrement WHERE TPIID=%n", nUserID, DateTime.Now, nTPIID, nTPIID);
        }
        public static IDataReader MultipleApprove(TransferPromotionIncrement oTransferPromotionIncrement, Int64 nUserID, TransactionContext tc)
        {
            return tc.ExecuteReader("UPDATE TransferPromotionIncrement SET ApproveBy=%n,ApproveByDate=%d WHERE TPIID IN("+oTransferPromotionIncrement.IDs+");SELECT * FROM View_TransferPromotionIncrement WHERE TPIID IN("+oTransferPromotionIncrement.IDs+")", nUserID, DateTime.Now);
        }
        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nTPIID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_TransferPromotionIncrement WHERE TPIID=%n", nTPIID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_TransferPromotionIncrement");
        }
        public static IDataReader GetsAutoTPI(TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Process_TPIAutoEffect]");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion

        #region Effect
        public static IDataReader Effect(TransactionContext tc, TransferPromotionIncrement oTransferPromotionIncrement, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Process_TransferPromotionIncrement] %n,%d,%n",
                   oTransferPromotionIncrement.TPIID, oTransferPromotionIncrement.ActualEffectedDate,
                   nUserID);

        }
        public static IDataReader UploadXLAsPerScheme(TransactionContext tc, TransferPromotionIncrement oTransferPromotionIncrement, Int64 nUserID)
         {
            return tc.ExecuteReader("EXEC [SP_Upload_IncrementAsPerScheme] %s,%s,%d,%n,%n,%b,%n, %n",
                   oTransferPromotionIncrement.EmployeeCode
                   , oTransferPromotionIncrement.SalarySchemeName
                   , oTransferPromotionIncrement.ActualEffectedDate
                   , oTransferPromotionIncrement.TPIGrossSalary
                   , oTransferPromotionIncrement.CompTPIGrossSalary
                   , oTransferPromotionIncrement.IsCashFixed
                   , oTransferPromotionIncrement.CashAmount,
                   nUserID);

        }
        
        public static int GetEmployeeSalaryStructureID(string sSQL, TransactionContext tc)
        {
            object obj = tc.ExecuteScalar(sSQL);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                int n = Convert.ToInt32(obj);
                return n;
            }
        }

        #endregion

        #region Upload
        public static IDataReader UploadXL(TransactionContext tc, TransferPromotionIncrement oTPI, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Upload_Increment] %s,%d,%n,%s,%s,%s,%n, %n,%b, %b,%n",
                   oTPI.EmployeeCode,
                   oTPI.EffectedDate,
                   oTPI.GrossSalary,
                   oTPI.Note,
                   oTPI.SalarySchemeName,
                   oTPI.SalaryHeadNames,
                   nUserID,
                   oTPI.CompGrossSalary,
                   oTPI.IsNoHistory,
                   oTPI.IsCashFixed,
                   oTPI.CashAmount
                   );
        }
        public static IDataReader UploadXLTP(TransactionContext tc, TransferPromotionIncrement oTPI, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Upload_TransferPromotion] %s,%s,%s,%s,%s,%s,%s, %s, %n, %b,%n",
                   oTPI.EmployeeCode,
                   oTPI.BUCode,
                   oTPI.LocCode,
                   oTPI.DeptCode,
                   oTPI.AttSchemeName,
                   oTPI.ShiftCode,
                   oTPI.DesgCode,
                   oTPI.EmpTypeName,
                   nUserID,
                   oTPI.IsCashFixed,
                   oTPI.CashAmount);
        }
        #endregion

    }
}
