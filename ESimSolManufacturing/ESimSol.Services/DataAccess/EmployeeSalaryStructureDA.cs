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
    public class EmployeeSalaryStructureDA
    {
        public EmployeeSalaryStructureDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, EmployeeSalaryStructure oEmployeeSalaryStructure, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeSalaryStructure] %n, %n, %n, %n, %s, %n, %b, %n, %n, %b, %b, %b, %b, %n, %n, %n, %n, %n, %b",
                   oEmployeeSalaryStructure.ESSID, oEmployeeSalaryStructure.EmployeeID,oEmployeeSalaryStructure.SSGradeID,
                   oEmployeeSalaryStructure.SalarySchemeID, oEmployeeSalaryStructure.Description,
                   oEmployeeSalaryStructure.GrossAmount, oEmployeeSalaryStructure.IsIncludeFixedItem,
                   oEmployeeSalaryStructure.ActualGrossAmount, oEmployeeSalaryStructure.CurrencyID,
                   oEmployeeSalaryStructure.IsActive, oEmployeeSalaryStructure.IsAllowBankAccount,
                   oEmployeeSalaryStructure.IsAllowOverTime, oEmployeeSalaryStructure.IsAttendanceDependent,
                   oEmployeeSalaryStructure.StartDay, nUserID, nDBOperation, oEmployeeSalaryStructure.CashAmount, oEmployeeSalaryStructure.CompGrossAmount, oEmployeeSalaryStructure.IsCashFixed);

        }

        public static IDataReader EditBankCash(TransactionContext tc, EmployeeSalaryStructure oESS, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_Update_BankCash]"
                                    + "%n, %n, %b, %b, %n, %n",
                                    oESS.EmployeeID, oESS.CashAmount, oESS.IsCashFixed, oESS.IsFullBonus, oESS.BonusCashAmount, nUserId);

        }
        public static IDataReader Activity(int nEmpID, int nESSID, bool IsActive, TransactionContext tc)
        {
            string sSql = "";
            if (IsActive == false)
            {
                sSql = "BEGIN TRAN IF EXISTS(SELECT EmployeeID FROM EmployeeSalaryStructure WHERE IsActive=1 AND EmployeeID =" + nEmpID + " AND ESSID !=" + nESSID + ")BEGIN ROLLBACK RAISERROR (N'One Active Item is Exist!',16,1)RETURN END;UPDATE EmployeeSalaryStructure SET IsActive=1 WHERE ESSID=" + nESSID + ";SELECT * FROM View_EmployeeSalaryStructure WHERE ESSID=" + nESSID + " ";
            }
            else
            {
                sSql = "UPDATE EmployeeSalaryStructure SET IsActive=0 WHERE ESSID=" + nESSID + ";SELECT * FROM View_EmployeeSalaryStructure WHERE ESSID=" + nESSID + "";
            }
            return tc.ExecuteReader(sSql);

        }

        public static IDataReader CopyEmployeeSalaryStructure(TransactionContext tc, int nEmployeeID, int nCopyFromESSID, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC SP_CopyEmployeeSalary %n,%n,%n", nEmployeeID, nCopyFromESSID, nUserId);
        }


        public static IDataReader MultipleIncrement(string sParam, Int64 nUserID, TransactionContext tc)
        {
            string sESSIDs = sParam.Split('~')[0];
            string sEmpIDs = sParam.Split('~')[1];
            int nSSID = Convert.ToInt32(sParam.Split('~')[2]);
            double nGrossAmount = Convert.ToDouble(sParam.Split('~')[3]);
            bool IsPercent = Convert.ToBoolean(sParam.Split('~')[4]);

            return tc.ExecuteReader("EXEC [SP_Process_Increment] %s,%s,%n,%n,%b,%n", sESSIDs, sEmpIDs, nSSID, nGrossAmount,IsPercent, nUserID);

        }
        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nESSID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeSalaryStructure WHERE ESSID=%n", nESSID);
        }
        public static IDataReader GetByEmp(int nEmployeeID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeSalaryStructure AS HH WHERE HH.EmployeeID = %n", nEmployeeID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeSalaryStructure");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
