using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ContractorTypeDA
    {
        public ContractorTypeDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ContractorType oCP, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ContractorType]"
                                    + "%n,%n, %n,%n",
                                  oCP.ContractorID, oCP.ContractorTypeID, nUserId, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, ContractorType oCP, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ContractorType]"
                                    + "%n,%n,%n,%n",
                                    oCP.ContractorID, oCP.ContractorTypeID, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function

        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM ContractorType WHERE ContractorTypeID=%n", nID);
        }
     
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ContractorType");
        }

        public static IDataReader GetsByContractor(TransactionContext tc, int nContractorID)
        {
            return tc.ExecuteReader("SELECT * FROM ContractorType WHERE ContractorID=%n", nContractorID);
        }
    
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
       

       
        #endregion
    }
}
