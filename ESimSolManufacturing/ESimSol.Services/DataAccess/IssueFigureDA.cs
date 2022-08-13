using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class IssueFigureDA
    {
        public IssueFigureDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, IssueFigure oIssueFigure, EnumDBOperation eEnumDBOperation,int nCurrentUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_IssueFigure]" + "%n, %n, %s, %s, %s, %b, %n, %n",
                                    oIssueFigure.IssueFigureID, oIssueFigure.ContractorID, oIssueFigure.ChequeIssueTo, oIssueFigure.SecondLineIssueTo, oIssueFigure.DetailNote, oIssueFigure.IsActive, (int)eEnumDBOperation, nCurrentUserID);
        }

        public static void Delete(TransactionContext tc, IssueFigure oIssueFigure, EnumDBOperation eEnumDBOperation, int nCurrentUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_IssueFigure]" + "%n, %n, %s, %s, %s, %b, %n, %n",
                                    oIssueFigure.IssueFigureID, oIssueFigure.ContractorID, oIssueFigure.ChequeIssueTo, oIssueFigure.SecondLineIssueTo, oIssueFigure.DetailNote, oIssueFigure.IsActive, (int)eEnumDBOperation, nCurrentUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM IssueFigure WHERE IssueFigureID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM IssueFigure ORDER BY ChequeIssueTo ASC");
        }
        public static IDataReader Gets(TransactionContext tc, int nContractorID, bool bIsActive)
        {
            return tc.ExecuteReader("SELECT * FROM IssueFigure WHERE ContractorID=%n AND IsActive=%b ORDER BY ChequeIssueTo ASC", nContractorID, bIsActive);
        }

        public static IDataReader Gets(TransactionContext tc, int nContractorID)
        {
            return tc.ExecuteReader("SELECT * FROM IssueFigure WHERE ContractorID =%n ORDER BY ChequeIssueTo ASC", nContractorID);
        }
        public static IDataReader GetsByName(TransactionContext tc, int nContractorID, string sName)
        {
            return tc.ExecuteReader("SELECT * FROM IssueFigure WHERE ContractorID =" + nContractorID + " AND ChequeIssueTo LIKE '%" + sName + "%' ORDER BY ChequeIssueTo ASC");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }  
}
