using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
    public class PETransactionDA
    {
        public PETransactionDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, PETransaction oPETransaction, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_PETransaction]"
                                    + "%n, %n, %n, %n, %n, %n, %n, %n, %s, %n, %n, %n, %n, %D, %n, %n",
                                    oPETransaction.PETransactionID, oPETransaction.ProductionExecutionID, oPETransaction.MeasurementUnitID, oPETransaction.Quantity, oPETransaction.CycleTime, oPETransaction.Cavity, oPETransaction.ShortCounter, oPETransaction.ProductionHour, oPETransaction.Remarks, oPETransaction.OperationEmpID, oPETransaction.ShiftID, oPETransaction.MachineID, oPETransaction.OperatorPerMachine, oPETransaction.TransactionDate,  nUserId, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, PETransaction oPETransaction, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_PETransaction]"
                                    + "%n, %n, %n, %n, %n, %n, %n, %n, %s, %n, %n, %n, %n, %D, %n, %n",
                                    oPETransaction.PETransactionID, oPETransaction.ProductionExecutionID, oPETransaction.MeasurementUnitID, oPETransaction.Quantity, oPETransaction.CycleTime, oPETransaction.Cavity, oPETransaction.ShortCounter, oPETransaction.ProductionHour, oPETransaction.Remarks, oPETransaction.OperationEmpID, oPETransaction.ShiftID, oPETransaction.MachineID, oPETransaction.OperatorPerMachine, oPETransaction.TransactionDate, nUserId, (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_PETransaction WHERE PETransactionID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_PETransaction");
        }

        public static IDataReader Gets(TransactionContext tc, int nRecipeID)
        {
            return tc.ExecuteReader("SELECT * FROM View_PETransaction WHERE RecipeID=%n", nRecipeID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
   
}
