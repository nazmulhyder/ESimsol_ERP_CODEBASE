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


    public class PurchaseOrderDA
    {
        public PurchaseOrderDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, PurchaseOrder oPO, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_PurchaseOrder]" + "%n, %n, %s, %d, %n, %n, %n, %n, %n, %s, %n, %n, %d, %n, %n,%s,%s,%n,%n,%n,%n,%n, %n,%n,%n",
                                    oPO.POID,
                                    oPO.BUID, 
                                    oPO.PONo, 
                                    oPO.PODate, 
                                    oPO.RefTypeInt, 
                                    oPO.RefID, 
                                    oPO.StatusInt, 
                                    oPO.ContractorID, 
                                    oPO.ContactPersonnelID, 
                                    oPO.Note,
                                    oPO.ConcernPersonID, 
                                    oPO.ApproveBy, 
                                    oPO.ApproveDate, 
                                    oPO.CurrencyID,     
                                    oPO.PaymentTermID,  
                                    oPO.ShipBy, 
                                    oPO.TradeTerm, 
                                    oPO.DeliveryTo, 
                                    oPO.DeliveryToContactPerson,
                                    oPO.BillTo, 
                                    oPO.BIllToContactPerson, 
                                    oPO.CRate,
                                    oPO.PaymentModeInt,
                                    nUserID, 
                                    (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, PurchaseOrder oPO, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_PurchaseOrder]" + "%n, %n, %s, %d, %n, %n, %n, %n, %n, %s, %n, %n, %d, %n, %n,%s,%s,%n,%n,%n,%n,%n, %n,%n,%n",
                                    oPO.POID, oPO.BUID, oPO.PONo, oPO.PODate, oPO.RefTypeInt, oPO.RefID, oPO.StatusInt, oPO.ContractorID, oPO.ContactPersonnelID, oPO.Note, oPO.ConcernPersonID, oPO.ApproveBy, oPO.ApproveDate, oPO.CurrencyID, oPO.PaymentTermID, oPO.ShipBy, oPO.TradeTerm, oPO.DeliveryTo, oPO.DeliveryToContactPerson, oPO.BillTo, oPO.BIllToContactPerson, oPO.CRate,
                                    oPO.PaymentModeInt, nUserID, (int)eEnumDBOperation);
        }
        public static void Approved(TransactionContext tc, PurchaseOrder oPurchaseOrder, Int64 nUserID)
        {
            tc.ExecuteNonQuery("UPDATE PurchaseOrder SET ApproveBy =%n, ApproveDate=%d, Status=%n WHERE POID =%n", nUserID, DateTime.Today, (int)EnumPOStatus.Approved, oPurchaseOrder.POID);
        }

        public static void UpdateReportSubject(TransactionContext tc, PurchaseOrder oPurchaseOrder, Int64 nUserID)
        {
            tc.ExecuteNonQuery("UPDATE PurchaseOrder SET SubjectName =%s WHERE POID =%n", oPurchaseOrder.SubjectName, oPurchaseOrder.POID);
        }
       
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_PurchaseOrder WHERE POID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_PurchaseOrder");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }  
    
   
}
