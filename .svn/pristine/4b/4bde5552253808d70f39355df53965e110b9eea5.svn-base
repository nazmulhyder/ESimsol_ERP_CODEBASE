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
    public class SalarySchemeDetailCalculationDA
    {
        public SalarySchemeDetailCalculationDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, SalarySchemeDetailCalculation oSalarySchemeDetailCalculation, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_SalarySchemeDetailCalculation] %n,%n,%n,%n,%n,%n,%n,%n,%n,%n",
                   oSalarySchemeDetailCalculation.SSDCID, oSalarySchemeDetailCalculation.SalarySchemeDetailID,
                   oSalarySchemeDetailCalculation.ValueOperatorInt, oSalarySchemeDetailCalculation.CalculationOnInt,
                   oSalarySchemeDetailCalculation.FixedValue, oSalarySchemeDetailCalculation.OperatorInt,
                   oSalarySchemeDetailCalculation.SalaryHeadID, oSalarySchemeDetailCalculation.PercentVelue,
                   nUserID, nDBOperation);

        }

        public static IDataReader IUDGross(TransactionContext tc, SalarySchemeDetailCalculation oSalarySchemeDetailCalculation, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_GrossSalaryCalculation] %n,%n,%n,%n,%n,%n,%n,%n,%n,%n",
                   oSalarySchemeDetailCalculation.GSCID, oSalarySchemeDetailCalculation.SalarySchemeID,
                   oSalarySchemeDetailCalculation.ValueOperatorInt, oSalarySchemeDetailCalculation.CalculationOnInt,
                   oSalarySchemeDetailCalculation.FixedValue, oSalarySchemeDetailCalculation.OperatorInt,
                   oSalarySchemeDetailCalculation.SalaryHeadID, oSalarySchemeDetailCalculation.PercentVelue,
                   nUserID, nDBOperation);

        }

        public static void Delete(TransactionContext tc, int nSalarySchemeDetailID)
        {
            tc.ExecuteNonQuery("DELETE FROM SalarySchemeDetailCalculation WHERE SalarySchemeDetailID=%n", nSalarySchemeDetailID);
        }
        public static void DeleteGross(TransactionContext tc, int nSalarySchemeid)
        {
            tc.ExecuteNonQuery("DELETE FROM GrossSalaryCalculation WHERE SalarySchemeID=%n", nSalarySchemeid);
        }
        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nSSDCID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_SalarySchemeDetailCalculation WHERE SSDCID=%n", nSSDCID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_SalarySchemeDetailCalculation");
        }
        public static IDataReader GetsBySalarySchemeDetail(TransactionContext tc, int nSchemeDetailID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SalarySchemeDetailCalculation WHERE SalarySchemeDetailID=%n", nSchemeDetailID);
        }
        public static IDataReader GetsBySalarySchemeGross(TransactionContext tc, int nSchemeID)
        {
            return tc.ExecuteReader("SELECT * FROM View_GrossSalaryCalculation WHERE SalarySchemeID=%n", nSchemeID);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
