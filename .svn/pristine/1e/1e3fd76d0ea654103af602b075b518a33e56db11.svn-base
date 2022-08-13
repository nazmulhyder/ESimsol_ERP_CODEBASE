using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ExportPartyInfoDetailDA
    {
        public ExportPartyInfoDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ExportPartyInfoDetail oExportPartyInfoDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportPartyInfoDetail]" + "%n, %n, %n,%n, %s, %s,%n, %b, %n, %n",
                                    oExportPartyInfoDetail.ExportPartyInfoDetailID, oExportPartyInfoDetail.ExportPartyInfoID, oExportPartyInfoDetail.ContractorID,oExportPartyInfoDetail.BankBranchID, oExportPartyInfoDetail.RefNo, oExportPartyInfoDetail.RefDate,oExportPartyInfoDetail.IsBank, oExportPartyInfoDetail.Activity, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, ExportPartyInfoDetail oExportPartyInfoDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ExportPartyInfoDetail]" + "%n, %n, %n,%n, %s, %s,%n, %b, %n, %n",
                                    oExportPartyInfoDetail.ExportPartyInfoDetailID, oExportPartyInfoDetail.ExportPartyInfoID, oExportPartyInfoDetail.ContractorID, oExportPartyInfoDetail.BankBranchID, oExportPartyInfoDetail.RefNo, oExportPartyInfoDetail.RefDate, oExportPartyInfoDetail.IsBank, oExportPartyInfoDetail.Activity, (int)eEnumDBOperation, nUserID);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportPartyInfoDetail WHERE ExportPartyInfoDetailID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportPartyInfoDetail");
        }
        public static IDataReader GetsByParty(TransactionContext tc, int nContractorID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportPartyInfoDetail WHERE ContractorID=%n", nContractorID);
        }
        public static IDataReader GetsBy(TransactionContext tc, int nPartyID, int nBankBranchID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportPartyInfoDetail WHERE (isnull(isBank,0)=0 and ContractorID=%n) or (isnull(isBank,0)=1 and BankBranchID=%n) Order by ExportPartyInfoID, RefNo", nPartyID, nBankBranchID);
        }
        public static IDataReader Gets(TransactionContext tc, int nPartyID, int nBankBranchID, string sIDs)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportPartyInfoDetail WHERE ((isnull(isBank,0)=0 and ContractorID=%n) or (isnull(isBank,0)=1 and BankBranchID=%n)) and ExportPartyInfoDetailID not in (%q) Order by ExportPartyInfoID, RefNo", nPartyID, nBankBranchID, sIDs);
        }
        public static IDataReader GetsByPartyAndBill(TransactionContext tc, int nContractorID, int nExportLCID, int nRefType)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportPartyInfoDetail WHERE ContractorID =%n AND ExportPartyInfoID IN (SELECT HH.ExportPartyInfoID FROM ExportPartyInfoBill AS HH WHERE HH.ReferenceID=%n AND HH.RefType=%n) ORDER BY ExportPartyInfoDetailID ASC", nContractorID, nExportLCID,nRefType);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
