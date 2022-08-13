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
    public class DeliveryChallanDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, DeliveryChallan oDeliveryChallan, short nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DeliveryChallan]"
                                   + "%n,%n,%n, %s,%d,%n,%n,%n,%n,%s,%s,%s,%s,%s,%n,%d,%n,%n,%s,%n, %n,%n",
                                   oDeliveryChallan.DeliveryChallanID, oDeliveryChallan.BUID, oDeliveryChallan.ChallanType, oDeliveryChallan.ChallanNo, oDeliveryChallan.ChallanDate, oDeliveryChallan.ChallanStatus, oDeliveryChallan.DeliveryOrderID, oDeliveryChallan.ContractorID, oDeliveryChallan.ContactPersonnelID, oDeliveryChallan.GatePassNo, oDeliveryChallan.VehicleName, oDeliveryChallan.VehicleNo, oDeliveryChallan.ReceivedByName, oDeliveryChallan.Note, oDeliveryChallan.ApproveBy, oDeliveryChallan.ApproveDate, oDeliveryChallan.WorkingUnitID, oDeliveryChallan.StoreInchargeID, oDeliveryChallan.DeliveryToAddress, oDeliveryChallan.BuyerID,  nUserID, nDBOperation);
        }

        public static void Delete(TransactionContext tc, DeliveryChallan oDeliveryChallan, short nDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DeliveryChallan]"
                                   + "%n,%n,%n, %s,%d,%n,%n,%n,%n,%s,%s,%s,%s,%s,%n,%d,%n,%n,%s,%n, %n,%n",
                                   oDeliveryChallan.DeliveryChallanID, oDeliveryChallan.BUID, oDeliveryChallan.ChallanType, oDeliveryChallan.ChallanNo, oDeliveryChallan.ChallanDate, oDeliveryChallan.ChallanStatus, oDeliveryChallan.DeliveryOrderID, oDeliveryChallan.ContractorID, oDeliveryChallan.ContactPersonnelID, oDeliveryChallan.GatePassNo, oDeliveryChallan.VehicleName, oDeliveryChallan.VehicleNo, oDeliveryChallan.ReceivedByName, oDeliveryChallan.Note, oDeliveryChallan.ApproveBy, oDeliveryChallan.ApproveDate, oDeliveryChallan.WorkingUnitID, oDeliveryChallan.StoreInchargeID, oDeliveryChallan.DeliveryToAddress, oDeliveryChallan.BuyerID, nUserID, nDBOperation);
        }


        public static IDataReader Approve(TransactionContext tc, int nDeliveryChallanID, Int64 nUserID)
        {
            //
            return tc.ExecuteReader("EXEC [SP_CommitDeliveryChallan]"
                                 + "%n,%n",nDeliveryChallanID, nUserID);

        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nDeliveryChallanID)
        {
            return tc.ExecuteReader("SELECT * FROM View_DeliveryChallan WHERE DeliveryChallanID=%n", nDeliveryChallanID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }

}
