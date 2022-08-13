using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
    public class DeliveryOrderDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, DeliveryOrder oDeliveryOrder, short nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DeliveryOrder]"
                                   + "%n,%n,%s,%d,%n,%n,%n,%n,%d,%s,%s,%n,%n,%n,%n",
                                   oDeliveryOrder.DeliveryOrderID, oDeliveryOrder.BUID, oDeliveryOrder.DONo, oDeliveryOrder.DODate, oDeliveryOrder.DOStatus, oDeliveryOrder.RefType, oDeliveryOrder.RefID, oDeliveryOrder.ContractorID, oDeliveryOrder.DeliveryDate, oDeliveryOrder.DeliveryPoint, oDeliveryOrder.Note, oDeliveryOrder.ApproveBy, oDeliveryOrder.ReviseNo, nUserID, nDBOperation);
        }

        public static void Delete(TransactionContext tc, DeliveryOrder oDeliveryOrder, short nDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DeliveryOrder]"
                                    + "%n,%n,%s,%d,%n,%n,%n,%n,%d,%s,%s,%n,%n,%n,%n",
                                    oDeliveryOrder.DeliveryOrderID, oDeliveryOrder.BUID, oDeliveryOrder.DONo, oDeliveryOrder.DODate, oDeliveryOrder.DOStatus, oDeliveryOrder.RefType, oDeliveryOrder.RefID, oDeliveryOrder.ContractorID, oDeliveryOrder.DeliveryDate, oDeliveryOrder.DeliveryPoint, oDeliveryOrder.Note, oDeliveryOrder.ApproveBy, oDeliveryOrder.ReviseNo, nUserID, nDBOperation);
        }

        public static void Approve(TransactionContext tc, int nDeliveryOrderID, Int64 nUserID)
        {
            tc.ExecuteNonQuery("UPDATE DeliveryOrder SET DOStatus=%n, ApproveBy =%n,  ApprovedDate=%D WHERE DeliveryOrderID =%n", (int)EnumDOStatus.Approved, nUserID,  DateTime.Now, nDeliveryOrderID);
        }

        public static IDataReader ChangeStatus(TransactionContext tc, DeliveryOrder oDeliveryOrder, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_DeliveryOrderOperation]"
                                    + "%n,%n,%n,%s,%n,%n,%n",
                                    0, oDeliveryOrder.DeliveryOrderID, (int)oDeliveryOrder.DOStatus, oDeliveryOrder.Note, (int)oDeliveryOrder.DOActionType, nUserID, (int)eEnumDBOperation);
        }

        public static IDataReader AcceptRevise(TransactionContext tc, DeliveryOrder oDeliveryOrder, short nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_AcceptDeliveryOrderRevise]"
                                   + "%n,%n,%s,%d,%n,%n,%n,%n,%d,%s,%s,%n, %b",
                                   oDeliveryOrder.DeliveryOrderID, oDeliveryOrder.BUID, oDeliveryOrder.DONo, oDeliveryOrder.DODate, oDeliveryOrder.DOStatus, oDeliveryOrder.RefType, oDeliveryOrder.RefID, oDeliveryOrder.ContractorID, oDeliveryOrder.DeliveryDate, oDeliveryOrder.DeliveryPoint, oDeliveryOrder.Note, nUserID, oDeliveryOrder.IsNewVersion);
        }


        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nDeliveryOrderID)
        {
            return tc.ExecuteReader("SELECT * FROM View_DeliveryOrder WHERE DeliveryOrderID=%n", nDeliveryOrderID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }

}
