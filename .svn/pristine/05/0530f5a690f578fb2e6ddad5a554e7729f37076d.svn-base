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
    public class DUDeliveryChallanDA
    {
        public DUDeliveryChallanDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, DUDeliveryChallan oDO, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DUDeliveryChallan]" + "%n,%s, %D, %n, %n, %n, %n, %n,%n, %s, %s, %s, %s, %s,%s, %n,%n,%n",
                                    oDO.DUDeliveryChallanID, oDO.ChallanNo, oDO.ChallanDate, oDO.OrderType, oDO.ContractorID, oDO.ReceiveByID, oDO.StoreInchargeID, oDO.ContactPersonnelID, oDO.WorkingUnitID, oDO.GatePassNo, oDO.VehicleName, oDO.VehicleNo, oDO.ReceivedByName, oDO.Note, oDO.DeliveryBy, oDO.PackCountBy, nUserID, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, DUDeliveryChallan oDO, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DUDeliveryChallan]" + "%n,%s, %D, %n, %n, %n, %n, %n,%n, %s, %s, %s, %s, %s,%s, %n,%n,%n",
                                     oDO.DUDeliveryChallanID, oDO.ChallanNo, oDO.ChallanDate, oDO.OrderType, oDO.ContractorID, oDO.ReceiveByID, oDO.StoreInchargeID, oDO.ContactPersonnelID, oDO.WorkingUnitID, oDO.GatePassNo, oDO.VehicleName, oDO.VehicleNo, oDO.ReceivedByName, oDO.Note, oDO.DeliveryBy, oDO.PackCountBy, nUserID, (int)eEnumDBOperation);
        }
        #endregion


        #region

     
        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_DUDeliveryChallan WHERE DUDeliveryChallanID=%n", nID);
        }
        public static IDataReader GetbyDO(int nDO, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT Top(1)* FROM View_DUDeliveryChallan WHERE IsDelivered=0 and DUDeliveryChallanID in (Select DUDeliveryChallanID from DUDeliveryChallanDetail where DOID=%n)", nDO);
        }
        public static IDataReader GetsBy(TransactionContext tc, string sContractorID)
        {
            return tc.ExecuteReader("Select * from View_DUDeliveryChallan where ContractorID in (%q) ", sContractorID);
        }
        public static IDataReader GetsByPI(TransactionContext tc, int nExportPIID)
        {
            return tc.ExecuteReader("Select * from View_DUDeliveryChallan where ExportPIID=%n", nExportPIID);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader UpdateFields(TransactionContext tc, DUDeliveryChallan oDUDeliveryChallan)
        {
            string sSQL1 = SQLParser.MakeSQL("Update DUDeliveryChallan Set PackCountBy=%n, DeliveryBy=%s WHERE DUDeliveryChallanID=%n", oDUDeliveryChallan.PackCountBy, oDUDeliveryChallan.DeliveryBy, oDUDeliveryChallan.DUDeliveryChallanID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_DUDeliveryChallan WHERE DUDeliveryChallanID=%n", oDUDeliveryChallan.DUDeliveryChallanID);
        }
     
        #endregion
    }
}
