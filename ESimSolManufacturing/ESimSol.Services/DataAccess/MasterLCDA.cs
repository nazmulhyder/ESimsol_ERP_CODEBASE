using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{

    public class MasterLCDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, MasterLC oMasterLC, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_MasterLC]" + "%n,%s,%n,%n,%n,%s,%n,%d,%d,%d,%d,%n,%n,%n,%n,%n,%s,%s,%n,%n,%n,%n,%n,%n,%n,%n,%b,%s,%s,%n,%n",
                                    oMasterLC.MasterLCID, oMasterLC.FileNo, (int)oMasterLC.LCStatus,oMasterLC.BUID, (int)oMasterLC.MasterLCType,  oMasterLC.MasterLCNo, oMasterLC.Applicant, NullHandler.GetNullValue(oMasterLC.MasterLCDate), oMasterLC.ReceiveDate, oMasterLC.LastDateofShipment, oMasterLC.ExpireDate, oMasterLC.Beneficiary, oMasterLC.IssueBankID, oMasterLC.AdviceBankID, oMasterLC.CurrencyID, oMasterLC.LCValue, oMasterLC.Remark, oMasterLC.ShipmentPort, (int)oMasterLC.PartialShipmentAllow, (int)oMasterLC.Transferable, (int)oMasterLC.LCType, (int)oMasterLC.DeferredFrom, oMasterLC.DefferedDaysCount, oMasterLC.DiscrepancyCharge, oMasterLC.Consignee, oMasterLC.NotifyParty, oMasterLC.MLCWithOrder, oMasterLC.Country, oMasterLC.ProductDesc,  nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, MasterLC oMasterLC, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_MasterLC]" + "%n,%s,%n,%n,%n,%s,%n,%d,%d,%d,%d,%n,%n,%n,%n,%n,%s,%s,%n,%n,%n,%n,%n,%n,%n,%n,%b,%s,%s,%n,%n",
                                    oMasterLC.MasterLCID, oMasterLC.FileNo, (int)oMasterLC.LCStatus, oMasterLC.BUID, (int)oMasterLC.MasterLCType, oMasterLC.MasterLCNo, oMasterLC.Applicant,NullHandler.GetNullValue(oMasterLC.MasterLCDate), oMasterLC.ReceiveDate, oMasterLC.LastDateofShipment, oMasterLC.ExpireDate, oMasterLC.Beneficiary, oMasterLC.IssueBankID, oMasterLC.AdviceBankID, oMasterLC.CurrencyID, oMasterLC.LCValue, oMasterLC.Remark, oMasterLC.ShipmentPort, (int)oMasterLC.PartialShipmentAllow, (int)oMasterLC.Transferable, (int)oMasterLC.LCType, (int)oMasterLC.DeferredFrom, oMasterLC.DefferedDaysCount, oMasterLC.DiscrepancyCharge, oMasterLC.Consignee, oMasterLC.NotifyParty,  oMasterLC.MLCWithOrder, oMasterLC.Country, oMasterLC.ProductDesc, nUserID, (int)eEnumDBOperation);
        }

        public static IDataReader ChangeStatus(TransactionContext tc, MasterLC oMasterLC, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC[SP_MasterLCOperation]" + "%n,%n,%n,%s,%n,%n,%n",
                                   oMasterLC.MasterLCHistoryID, oMasterLC.MasterLCID, (int)oMasterLC.LCStatus, oMasterLC.Remark, (int)oMasterLC.MasterLCActionType, nUserID, (int)eEnumDBOperation);
        }

        #region Accept PI Revise
        public static IDataReader AcceptMasterLCRevise(TransactionContext tc, MasterLC oMasterLC, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC[SP_AcceptMasterLCAmendment]" + "%n, %s, %n,%n,%s, %s, %n, %d, %d, %d, %d, %n, %n, %n, %n, %n, %s, %n, %s, %n, %n, %n, %n, %n, %n, %n, %n,%s,%s, %n",
                                    oMasterLC.MasterLCID, oMasterLC.FileNo, (int)oMasterLC.LCStatus, oMasterLC.BUID, (int)oMasterLC.MasterLCType, oMasterLC.MasterLCNo, oMasterLC.Applicant, oMasterLC.MasterLCDate, oMasterLC.ReceiveDate, oMasterLC.LastDateofShipment, oMasterLC.ExpireDate, oMasterLC.Beneficiary, oMasterLC.IssueBankID, oMasterLC.AdviceBankID, oMasterLC.CurrencyID, oMasterLC.LCValue, oMasterLC.Remark, oMasterLC.ApprovedBy, oMasterLC.ShipmentPort, (int)oMasterLC.PartialShipmentAllow, (int)oMasterLC.Transferable, (int)oMasterLC.LCType, (int)oMasterLC.DeferredFrom, oMasterLC.DefferedDaysCount, oMasterLC.DiscrepancyCharge, oMasterLC.Consignee, oMasterLC.NotifyParty, oMasterLC.Country, oMasterLC.ProductDesc, nUserID);
        }


        #endregion

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_MasterLC WHERE MasterLCID=%n", nID);
        }
        public static IDataReader GetsByLCID(TransactionContext tc, int nExportLCID)
        {
            return tc.ExecuteReader("SELECT * FROM MasterLC WHERE MasterLCID IN(SELECT MasterLCID FROM MasterLCMapping where ExportLCID=%n) Order By [MasterLCNo]", nExportLCID);
        }
        public static IDataReader GetBySaleOrder(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_MasterLC WHERE MasterLCID = (SELECT top 1 MasterLCID FROM MasterLCDetail WHERE ProformaInvoiceID = (SELECT top 1 ProformaInvoiceID FROM ProformaInvoiceDetail WHERE OrderRecapID = %n))", nID);
        }

        public static IDataReader GetLog(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_MasterLCLog WHERE MasterLCLogID=%n", nID);
        }

        public static IDataReader Gets(int buid, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_MasterLC WHERE BUID = %n",buid);
        }

        public static IDataReader GetsMasterLCLog(int id, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_MasterLCLog WHERE MasterLCID=%n", id);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsByContractorID(TransactionContext tc, int nContractorID)
        {
            return tc.ExecuteReader("SELECT * FROM View_MasterLC WHERE MasterLCID NOT IN (SELECT MasterLCID FROM MasterLCMapping) OR MasterLCID IN (SELECT MasterLCID FROM MasterLCMapping WHERE ContractorID=%n)", nContractorID);
        }
        #endregion
    }

}
