using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ContractorAddressDA
    {
        public ContractorAddressDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ContractorAddress oCP, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ContractorAddress]"
                                    + "%n,%n,%n,%s,%s, %n,%n",
                                    oCP.ContractorAddressID, oCP.ContractorID, oCP.AddressTypeInt, oCP.Address, oCP.Note, nUserId, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, ContractorAddress oCP, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ContractorAddress]"
                                    + "%n,%n,%n,%s,%s, %n,%n",
                                   oCP.ContractorAddressID, oCP.ContractorID, oCP.AddressTypeInt, oCP.Address, oCP.Note, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function

        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ContractorAddress WHERE ContractorAddressID=%n", nID);
        }
        public static IDataReader GetsForPIDeliveredTo(TransactionContext tc, int nPTUID)
        {
            return tc.ExecuteReader("select * from View_ContractorAddress where ContractorAddressID in (select DeliverToBCP from [PI] where PIID in (select PIID from Job where JobID in (select OrderID from ProductionTracingUnit where ordertype=3 and ProductionTracingUnitID=%n)))", nPTUID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ContractorAddress");
        }

        public static IDataReader GetsByContractor(TransactionContext tc, int nContractorID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ContractorAddress WHERE ContractorID=%n", nContractorID);
        }
        public static IDataReader GetsBy(TransactionContext tc, int nContractorID, string sAddtessType)
        {
            return tc.ExecuteReader("SELECT * FROM View_ContractorAddress WHERE ContractorID=%n and AddressType in (%q) ", nContractorID, sAddtessType);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsOnlyCommission(TransactionContext tc, string sContractorIDs)
        {
            return tc.ExecuteReader("SELECT * FROM View_ContractorAddress WHERE ContractorAddressID in(select ContractPersonalID from CommissionPercent) and  ContractorID IN (%q) Order By ContractorID", sContractorIDs);
        }

        public static IDataReader Gets(TransactionContext tc, int nContractorID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ContractorAddress WHERE ContractorID=%n", nContractorID);
        }
        #endregion
    }
}
