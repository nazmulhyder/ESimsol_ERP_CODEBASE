using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;



namespace ESimSol.Services.DataAccess
{
    public class ImportLCDA
    {
        public ImportLCDA() { }

        #region Insert, Update, Delete
        public static IDataReader InsertUpdate(TransactionContext tc, ImportLC oImportLC, EnumDBOperation eEnumDBImportLC, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ImportLC]"
                                    + " %n,%s,%n,%n,%n,%n,%n,%n,%n,%n,%n,%D,%D,%s,%D,%D,%s,%n,%n,%n,%b,%b, %n,%n,%n,%n,%n,%n,%b, %n, %n,%n",
                                    oImportLC.ImportLCID,
                                    oImportLC.FileNo,
                                    oImportLC.AmendmentNo,
                                    oImportLC.ContractorID,
                                    oImportLC.BankBranchID_Nego,
                                    oImportLC.InsuranceCompanyID,
                                    oImportLC.Amount,
                                    oImportLC.CCRate,
                                    oImportLC.CurrencyID,
                                    oImportLC.LCPaymentType,
                                    oImportLC.LCCurrentStatus,
                                    oImportLC.ExpireDate,
                                    oImportLC.ShipmentDate,
                                    oImportLC.LCCoverNoteNo,
                                    oImportLC.CoverNoteDate,
                                    oImportLC.LCRequestDate,
                                    oImportLC.LCANo,
                                    oImportLC.LCTermID,
                                    oImportLC.LCTermID_Bene,
                                    (int)oImportLC.ShipmentBy,
                                    oImportLC.IsPartShipmentAllow,
                                    oImportLC.IsTransShipmentAllow,
                                    oImportLC.PaymentInstructionInt,
                                    oImportLC.BUID,
                                    oImportLC.LCAppTypeInt ,
		                            oImportLC.LCMargin ,
		                            oImportLC.Tolerance,
		                            oImportLC.BankAccountID ,
                                    oImportLC.IsConfirmation,
                                    oImportLC.LCChargeTypeInt,
                                    nUserId,
                                    (int)eEnumDBImportLC);
        }
        public static void Delete(TransactionContext tc, ImportLC oImportLC, EnumDBOperation eEnumDBImportLC, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ImportLC]"
                                     + " %n,%s,%n,%n,%n,%n,%n,%n,%n,%n,%n,%D,%D,%s,%D,%D,%s,%n,%n,%n,%b,%b, %n,%n,%n,%n,%n,%n,%b, %n, %n,%n",
                                    oImportLC.ImportLCID,
                                    oImportLC.FileNo,
                                    oImportLC.AmendmentNo,
                                    oImportLC.ContractorID,
                                    oImportLC.BankBranchID_Nego,
                                    oImportLC.InsuranceCompanyID,
                                    oImportLC.Amount,
                                    oImportLC.CCRate,
                                    oImportLC.CurrencyID,
                                    oImportLC.LCPaymentType,
                                    oImportLC.LCCurrentStatus,
                                    oImportLC.ExpireDate,
                                    oImportLC.ShipmentDate,
                                    oImportLC.LCCoverNoteNo,
                                    oImportLC.CoverNoteDate,
                                    oImportLC.LCRequestDate,
                                    oImportLC.LCANo,
                                    oImportLC.LCTermID,
                                    oImportLC.LCTermID_Bene,
                                    (int)oImportLC.ShipmentBy,
                                    oImportLC.IsPartShipmentAllow,
                                    oImportLC.IsTransShipmentAllow,
                                    oImportLC.PaymentInstructionInt,
                                    oImportLC.BUID,
                                    oImportLC.LCAppTypeInt,
                                    oImportLC.LCMargin,
                                    oImportLC.Tolerance,
                                    oImportLC.BankAccountID,
                                    oImportLC.IsConfirmation,
                                    oImportLC.LCChargeTypeInt,
                                    nUserId,
                                    (int)eEnumDBImportLC);
        }
        public static IDataReader UpdateImportLC(TransactionContext tc, ImportLC oImportLC, EnumDBOperation eEnumDBImportLC, Int64 nUserId) // edited by akram (shipment date, expire date added) [SP: L-131]
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ImportLCOpen]"
                                    + " %n,%s,%s,%D,%D,%D,%D,%n,%D,%D,%n,%n",
                                        oImportLC.ImportLCID,
                                        oImportLC.ImportLCNo,
                                        oImportLC.BBankRefNo,
                                        oImportLC.ImportLCDate,
                                        oImportLC.ReceiveDate,
                                        oImportLC.ForwardDate,
                                        oImportLC.AckmentRecDate,
                                        oImportLC.ReceivedBy,
                                        oImportLC.ShipmentDate,
                                        oImportLC.ExpireDate,
                                        (int)oImportLC.LCCurrentStatus,
                                        nUserId,
                                        (int)eEnumDBImportLC);

        }
        public static IDataReader UpdateImportLC_FileNo(TransactionContext tc, ImportLC oImportLC, EnumDBOperation eEnumDBImportLC, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ImportLC_FileNo]"
                                    + " %n,%s,%D,%s,%n",
                                        oImportLC.ImportLCID,
                                        oImportLC.ImportLCNo,
                                        oImportLC.ImportLCDate,
                                        oImportLC.FileNo,
                                        nUserId
                                        );

        }

        #endregion
        #region Insert, Update, Delete Log
        public static IDataReader InsertUpdateLog(TransactionContext tc, ImportLC oImportLC, EnumDBOperation eEnumDBImportLC, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ImportLCLog]"
                                   + "%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%D,%D,%s,%D,%D,%s,%n,%n,%n,%b,%b, %n,%n,%n,%n,%n,%n,%b, %n, %n,%n",
                                    oImportLC.ImportLCID,
                                    oImportLC.ImportLCLogID,
                                    oImportLC.AmendmentNo,
                                    oImportLC.ContractorID,
                                    oImportLC.BankBranchID_Nego,
                                    oImportLC.InsuranceCompanyID,
                                    oImportLC.Amount,
                                    oImportLC.CCRate,
                                    oImportLC.CurrencyID,
                                    oImportLC.LCPaymentType,
                                    oImportLC.LCCurrentStatusInt,
                                    oImportLC.ExpireDate,
                                    oImportLC.ShipmentDate,
                                    oImportLC.LCCoverNoteNo,
                                    oImportLC.CoverNoteDate,
                                    oImportLC.LCRequestDate,
                                    oImportLC.LCANo,
                                    oImportLC.LCTermID,
                                     oImportLC.LCTermID_Bene,
                                    (int)oImportLC.ShipmentBy,
                                    oImportLC.IsPartShipmentAllow,
                                    oImportLC.IsTransShipmentAllow,
                                    oImportLC.PaymentInstructionInt,
                                    oImportLC.BUID,
                                    oImportLC.LCAppTypeInt,
                                    oImportLC.LCMargin,
                                    oImportLC.Tolerance,
                                    oImportLC.BankAccountID,
                                    oImportLC.IsConfirmation,
                                    oImportLC.LCChargeTypeInt,
                                    nUserId,
                                    (int)eEnumDBImportLC);
        }
        public static IDataReader RequestConfirm(TransactionContext tc, ImportLC oImportLC, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ImportLCAmendment]"
                                   + "%n,%n,%D,%D",
                                    oImportLC.ImportLCID,
                                    (int)oImportLC.LCCurrentStatus,
                                    NullHandler.GetNullValue(oImportLC.ImportLCDate),
                                    NullHandler.GetNullValue(oImportLC.ReceiveDate)
                                    );
        }
        public static IDataReader InsertUpdateLCStatus(TransactionContext tc, ImportLC oImportLC, EnumDBOperation eENumDBPurchaseInvoice, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ImportILCHistory_Update]"
                                    + "%n,%n,%n,%n",
                                    oImportLC.ImportLCID,
                                    (int)oImportLC.LCCurrentStatus,
                                    nUserId,
                                    (int)eENumDBPurchaseInvoice);
        }
        #endregion

        #region Get & Exist Function

        public static IDataReader Get(TransactionContext tc, int nID)
        {

            return tc.ExecuteReader("SELECT * from [View_ImportLC]  WHERE ImportLCID=%n Order By [ImportLCID]", nID);
        }
        public static IDataReader GetLog(TransactionContext tc, int nImportLCID)
        {

            return tc.ExecuteReader("SELECT * from [View_ImportLCLog]  WHERE ImportLCID=%n and IsRequested=1", nImportLCID);
        }
      
        public static IDataReader GetsByStatus(TransactionContext tc, string sLCCurrentStatus, int nBUID)
        {
            if (nBUID > 0)
            {
                return tc.ExecuteReader("SELECT * FROM [View_ImportLC] where BUID=%n AND LCCurrentStatus in (%q) Order By ImportLCDate, ImportLCNo ASC", nBUID, sLCCurrentStatus);
            }
            else
            {
                return tc.ExecuteReader("SELECT * FROM [View_ImportLC] where LCCurrentStatus in (%q) Order By ImportLCDate, ImportLCNo ASC", sLCCurrentStatus);
            }
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion



    }


}
