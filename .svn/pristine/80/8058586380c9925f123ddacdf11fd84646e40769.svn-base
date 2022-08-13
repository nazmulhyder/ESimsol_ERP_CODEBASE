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
    public class DyeingOrderDA
    {
        public DyeingOrderDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, DyeingOrder oDO, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DyeingOrder]" + "%n,%s,  %D, %n, %n, %n, %n, %s, %s, %n,%n,%n,%s,%n,%n,%n,%n,%n,%s,%s,%s,%b,%n,%n,%s,%n,%n",
                                    oDO.DyeingOrderID, oDO.OrderNo, oDO.OrderDate, oDO.ContractorID, oDO.ContactPersonnelID, oDO.DeliveryToID, oDO.ContactPersonnelID_DelTo, oDO.StyleNo, oDO.RefNo, oDO.PriorityInt, oDO.LightSourchID, oDO.LightSourchIDTwo, oDO.Note, oDO.DyeingOrderType, oDO.PaymentType, oDO.ExportPIID, oDO.SampleInvoiceID, oDO.MKTEmpID, oDO.StripeOrder, oDO.KnittingStyle, oDO.Gauge, oDO.IsInHouse, oDO.DyeingStepTypeInt,oDO.FSCDetailID, oDO.ReviseNote, nUserID, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, DyeingOrder oDO, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DyeingOrder]" + "%n,%s,  %D, %n, %n, %n, %n, %s, %s, %n,%n,%n,%s,%n,%n,%n,%n,%n,%s,%s,%s,%b,%n,%n,%s,%n,%n",
                                      oDO.DyeingOrderID, oDO.OrderNo, oDO.OrderDate, oDO.ContractorID, oDO.ContactPersonnelID, oDO.DeliveryToID, oDO.ContactPersonnelID_DelTo, oDO.StyleNo, oDO.RefNo, oDO.PriorityInt, oDO.LightSourchID, oDO.LightSourchIDTwo, oDO.Note, oDO.DyeingOrderType, oDO.PaymentType, oDO.ExportPIID, oDO.SampleInvoiceID, oDO.MKTEmpID, oDO.StripeOrder, oDO.KnittingStyle, oDO.Gauge, oDO.IsInHouse, oDO.DyeingStepTypeInt,oDO.FSCDetailID,oDO.ReviseNote, nUserID, (int)eEnumDBOperation);
        }
        public static IDataReader InsertUpdate_Log(TransactionContext tc, DyeingOrder oDO, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DyeingOrderLog]" + "%n,%s,  %D, %n, %n, %n, %n, %s, %s, %n,%n,%n,%s,%n,%n,%n,%n,%n,%s,%s,%s,%n,%b,%n,%n,%D,%s,%b,%n,%n",
                                    oDO.DyeingOrderID, oDO.OrderNo, oDO.OrderDate, oDO.ContractorID, oDO.ContactPersonnelID, oDO.DeliveryToID, oDO.ContactPersonnelID_DelTo, oDO.StyleNo, oDO.RefNo, oDO.PriorityInt, oDO.LightSourchID, oDO.LightSourchIDTwo, oDO.Note, oDO.DyeingOrderType, oDO.PaymentType, oDO.ExportPIID, oDO.SampleInvoiceID, oDO.MKTEmpID, oDO.StripeOrder, oDO.KnittingStyle, oDO.Gauge, oDO.Amount, oDO.IsInHouse, oDO.DyeingStepTypeInt,oDO.FSCDetailID,NullHandler.GetNullValue(oDO.ReviseDate),oDO.ReviseNote, oDO.IsCreateReviseNo, nUserID, (int)eEnumDBOperation);
        }
        public static IDataReader InsertUpdate_DeliveryOrder(TransactionContext tc, DyeingOrder oDO, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DUDeliveryOrder_Auto]" + "%n,%s, %D, %n, %n, %n, %n, %n, %n, %n,%n,%n,%n,%n",
                                    oDO.DyeingOrderID, oDO.OrderNo, oDO.OrderDate, oDO.ContractorID, oDO.ContactPersonnelID, oDO.DeliveryToID, oDO.ContactPersonnelID_DelTo, oDO.ExportPIID, oDO.SampleInvoiceID, oDO.MKTEmpID, oDO.DyeingOrderType, oDO.DyeingOrderID, nUserID, (int)eEnumDBOperation);
        }
        public static IDataReader DOCancel(TransactionContext tc, DyeingOrder oDO, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DyeingOrder_Cancel]" + "%n,%n",oDO.DyeingOrderID,  nUserID);
        }
        public static void CreateLabdipByDO(TransactionContext tc, int nDyeingOrderID, Int64 nUserID)
        {
             tc.ExecuteNonQuery("EXEC [SP_IUD_LabDip_DTMAVL]" + "%n, %n", nDyeingOrderID, nUserID);
        }
        public static IDataReader CreateServisePI(TransactionContext tc, DyeingOrder oDO, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ImportPI_Service]" + "%n,%n,%n,%n,%n", oDO.DyeingOrderID, oDO.BUID, (int)EnumProductNature.Yarn, nUserID, eEnumDBOperation);
        }
        //public static void UpdateDeliveryZone(TransactionContext tc, int id, int DeliveryZoneID)
        //{
        //    tc.ExecuteNonQuery("Update DyeingOrder SET DeliveryZoneID =%n  WHERE DyeingOrderID = %n", DeliveryZoneID, id);
        //}
        public static void UpdateDeliveryNote(TransactionContext tc,  string sDeliveryNote,int id)
        {
            tc.ExecuteNonQuery("Update DyeingOrder SET DeliveryNote =%s  WHERE DyeingOrderID = %n", sDeliveryNote, id);
        }

        public static IDataReader UpdateMasterBuyer(TransactionContext tc, DyeingOrder oDyeingOrder)
        {
            string sSQL1 = SQLParser.MakeSQL("Update DyeingOrder SET MBuyerID =%n  WHERE DyeingOrderID = %n", oDyeingOrder.MBuyerID, oDyeingOrder.DyeingOrderID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_DyeingOrder WHERE DyeingOrderID=%n", oDyeingOrder.DyeingOrderID);

        }
        #endregion

        #region

        public static IDataReader DyeingOrderAdjustmentForExportPI(TransactionContext tc, string sDyeingOrderIDs, int nExportPIID, int nDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DyeingOrderAdjustmentForExportPI]" + "%s, %n, %n, %n", sDyeingOrderIDs, nExportPIID, nDBOperation, nUserId);
        }
        public static IDataReader DyeingOrderSendToProduction(TransactionContext tc, DyeingOrder oDO,  Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_PTU_DyeingOrder]" + " %n, %n", oDO.DyeingOrderID, nUserID);
        }
        public static IDataReader DyeingOrderHistory(TransactionContext tc, DyeingOrder oDO, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DyeingOrderHistory]" + " %n,%n,%s, %n,%n", oDO.DyeingOrderID, (int)oDO.Status, oDO.Note, nUserID,(int)EnumDBOperation.Insert );
        }
        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nDEOID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_DyeingOrder WHERE DyeingOrderID=%n", nDEOID);
        }
        public static IDataReader GetFSCD(int nFSEDetailID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_DyeingOrder WHERE isnull(FSCDetailID,0)>0 and FSCDetailID=%n", nFSEDetailID);
        }
        public static IDataReader GetsByPaymentType(TransactionContext tc, string sPaymentTypes)
        {
            return tc.ExecuteReader("SELECT * FROM View_DyeingOrder where SampleInvoiceID=0 and [PaymentType] in (%q)", sPaymentTypes);
        }
        public static IDataReader GetsByNo(TransactionContext tc, string sOrderNo)
        {
            return tc.ExecuteReader("Select * from View_DyeingOrder where OrderNo like '%" + sOrderNo + "'");
        }
        public static IDataReader GetsBy(TransactionContext tc, string sContractorID)
        {
            return tc.ExecuteReader("Select * from View_DyeingOrder where ContractorID in (%q) and SampleInvoiceID=0 and PaymentType  in (" + (int)EnumOrderPaymentType.AdjWithNextLC + "," + (int)EnumOrderPaymentType.AdjWithPI + "," + (int)EnumOrderPaymentType.CashOrCheque + ")", sContractorID);
        }
        public static IDataReader GetsByPI(TransactionContext tc, int nExportPIID)
        {
            return tc.ExecuteReader("Select * from View_DyeingOrder where ExportPIID=%n", nExportPIID);
        }
        public static IDataReader GetsByInvoice(TransactionContext tc, int nSampleInvoiceID)
        {
            return tc.ExecuteReader("Select * from View_DyeingOrder where SampleInvoiceID=%n and PaymentType  in (" + (int)EnumOrderPaymentType.AdjWithNextLC + "," + (int)EnumOrderPaymentType.AdjWithPI + "," + (int)EnumOrderPaymentType.CashOrCheque + ")", nSampleInvoiceID);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader OrderClose(TransactionContext tc, DyeingOrder oDO)
        {
            string sSQL1 = SQLParser.MakeSQL("Update DyeingOrder Set IsClose=~isnull(IsClose,0) WHERE DyeingOrderID=%n", oDO.DyeingOrderID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_DyeingOrder WHERE DyeingOrderID=%n", oDO.DyeingOrderID);

        }
      
        public static bool GetIsLabdipApply(TransactionContext tc, int nOrderType)
        {
            object obj = tc.ExecuteScalar("Select IsSaveLabDip from DUOrderSetup where Activity=1 and Ordertype=%n", nOrderType);
            if (obj == null) return false;
            else { return Convert.ToBoolean(obj); }
           
        }
        #endregion
    }
}
