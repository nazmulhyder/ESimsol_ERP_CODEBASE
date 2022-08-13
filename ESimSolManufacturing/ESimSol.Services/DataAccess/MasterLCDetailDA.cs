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
  
    
    public class MasterLCDetailDA
    {
        public MasterLCDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, MasterLCDetail oMasterLCDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sMasterLCDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_MasterLCDetail]"
                                    + "%n,%n,%n,%s,%n,%n,%d,%n,%n,%n,%s", oMasterLCDetail.MasterLCDetailID, oMasterLCDetail.MasterLCID, oMasterLCDetail.ProformaInvoiceID,oMasterLCDetail.PINo,(int)oMasterLCDetail.PIStatus,oMasterLCDetail.BuyerID,oMasterLCDetail.PIIssueDate,oMasterLCDetail.PIQty, nUserID, (int)eEnumDBOperation, "");
        }

        public static void Delete(TransactionContext tc, MasterLCDetail oMasterLCDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sMasterLCDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_MasterLCDetail]"
                                    + "%n,%n,%n,%s,%n,%n,%d,%n,%n,%n,%s", oMasterLCDetail.MasterLCDetailID, oMasterLCDetail.MasterLCID, oMasterLCDetail.ProformaInvoiceID, oMasterLCDetail.PINo, (int)oMasterLCDetail.PIStatus, oMasterLCDetail.BuyerID, oMasterLCDetail.PIIssueDate, oMasterLCDetail.PIQty,  nUserID, (int)eEnumDBOperation, sMasterLCDetailIDs);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_MasterLCDetail WHERE MasterLCDetailID=%n", nID);
        }
        public static IDataReader GetByOrderRecap(TransactionContext tc, long nOrderRecapID)
        {
            return tc.ExecuteReader("SELECT   top 1 * FROM View_MasterLCDetail WHERE ProformaInvoiceID  = (SELECT top 1 ProformaInvoiceID FROM ProformaInvoiceDetail WHERE OrderRecapID = %n)", nOrderRecapID);
        }
        public static IDataReader Gets(int nProformaInvoiceID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_MasterLCDetail where MasterLCID =%n", nProformaInvoiceID);
        }

        public static IDataReader GetsMasterLCLog(int nProformaInvoiceLogID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_MasterLCDetailLog where MasterLCLogID =%n", nProformaInvoiceLogID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static void DeleteSingleMLCDetail(TransactionContext tc, int nMasterLCDetailID)
        {
            tc.ExecuteNonQuery("DELETE FROM MasterLCDetail WHERE MasterLCDetailID =%n", nMasterLCDetailID);
        }
        #endregion
    }
    
    
}
