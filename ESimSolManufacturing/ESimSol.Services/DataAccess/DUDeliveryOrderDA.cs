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
    public class DUDeliveryOrderDA
    {
        public DUDeliveryOrderDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, DUDeliveryOrder oDO, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DUDeliveryOrder]" + "%n,%s, %D, %n, %n, %n, %n,%n, %b,%n,%d, %s, %s,%n,%n",
                                    oDO.DUDeliveryOrderID, oDO.DONo, oDO.DODate, oDO.DOStatus, oDO.OrderType,oDO.OrderID,oDO.ExportPIID, oDO.ContractorID, oDO.IsRaw, oDO.ContactPersonnelID, oDO.DeliveryDate, oDO.DeliveryPoint, oDO.Note, nUserID, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, DUDeliveryOrder oDO, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DUDeliveryOrder]" + "%n,%s, %D, %n, %n, %n, %n,%n, %b,%n,%d, %s, %s,%n,%n",
                                       oDO.DUDeliveryOrderID, oDO.DONo, oDO.DODate, oDO.DOStatus, oDO.OrderType, oDO.OrderID, oDO.ExportPIID, oDO.ContractorID, oDO.IsRaw, oDO.ContactPersonnelID, oDO.DeliveryDate, oDO.DeliveryPoint, oDO.Note, nUserID, (int)eEnumDBOperation);
        }
        public static IDataReader InsertUpdate_Log(TransactionContext tc, DUDeliveryOrder oDO, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DUDeliveryOrderLog]" + "%n,%s, %D, %n, %n, %n, %n,%n, %b,%n,%d, %s, %s,%b,%n,%n",
                                    oDO.DUDeliveryOrderID, oDO.DONo, oDO.DODate, oDO.DOStatus, oDO.OrderType, oDO.OrderID, oDO.ExportPIID, oDO.ContractorID, oDO.IsRaw, oDO.ContactPersonnelID, oDO.DeliveryDate, oDO.DeliveryPoint, oDO.Note, oDO.IsRevise, nUserID, (int)eEnumDBOperation);
        }
        public static IDataReader DOCancel(TransactionContext tc, DUDeliveryOrder oDO, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DUDeliveryOrder]" + "%n,%s, %D, %n, %n, %n, %n,%n, %b,%n,%d, %s, %s,%n,%n",
                                   oDO.DUDeliveryOrderID, oDO.DONo, oDO.DODate, oDO.DOStatus, oDO.OrderType, oDO.OrderID, oDO.ExportPIID, oDO.ContractorID, oDO.IsRaw, oDO.ContactPersonnelID, oDO.DeliveryDate, oDO.DeliveryPoint, oDO.Note, nUserID, (int)eEnumDBOperation);
        }
        public static IDataReader UpdateDONo(TransactionContext tc, DUDeliveryOrder oDO, Int64 nUserID, EnumDBOperation eEnumDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DUDeliveryOrderNo]" + "%n, %s,%n", oDO.DUDeliveryOrderID, oDO.DONo, nUserID);
        }
        #endregion


        #region

        public static IDataReader DUDeliveryOrderAdjustmentForExportPI(TransactionContext tc, string sDUDeliveryOrderIDs, int nExportPIID, int nDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DUDeliveryOrderAdjustmentForExportPI]" + "%s, %n, %n, %n", sDUDeliveryOrderIDs, nExportPIID, nDBOperation, nUserId);
        }
        public static IDataReader DUDeliveryOrderSendToProduction(TransactionContext tc, DUDeliveryOrder oDO,  Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_PTU_DUDeliveryOrder]" + " %n, %n", oDO.DUDeliveryOrderID, nUserID);
        }
        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nDEOID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_DUDeliveryOrder WHERE DUDeliveryOrderID=%n", nDEOID);
        }
        public static IDataReader GetsByPaymentType(TransactionContext tc, string sPaymentTypes)
        {
            return tc.ExecuteReader("SELECT * FROM View_DUDeliveryOrder where SampleInvoiceID=0 and [PaymentType] in (%q)", sPaymentTypes);
        }
        public static IDataReader GetsByNo(TransactionContext tc, string sOrderNo)
        {
            return tc.ExecuteReader("Select * from View_DUDeliveryOrder where OrderNo like '%" + sOrderNo + "'");
        }
        public static IDataReader GetsBy(TransactionContext tc, string sContractorID)
        {
            return tc.ExecuteReader("Select * from View_DUDeliveryOrder where ContractorID in (%q) and SampleInvoiceID=0", sContractorID);
        }
        public static IDataReader GetsByPI(TransactionContext tc, int nExportPIID)
        {
            return tc.ExecuteReader("Select * from View_DUDeliveryOrder where ExportPIID=%n", nExportPIID);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

     
        #endregion
    }
}
