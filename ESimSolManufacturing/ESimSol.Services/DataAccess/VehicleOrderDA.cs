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
	public class VehicleOrderDA 
	{
		#region Insert Update Delete Function
		public static IDataReader InsertUpdate(TransactionContext tc, VehicleOrder oVehicleOrder, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_VehicleOrder]"
                                    + "%n, %s, %n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n, %d,%s,%s,%n,%n,%n",
                                    oVehicleOrder.VehicleOrderID, oVehicleOrder.RefNo, oVehicleOrder.VehicleModelID, oVehicleOrder.BUID, oVehicleOrder.ChassisID, oVehicleOrder.EngineID, oVehicleOrder.ExteriorColorID, oVehicleOrder.InteriorColorID, oVehicleOrder.UpholsteryID, oVehicleOrder.TrimID, oVehicleOrder.WheelsID, oVehicleOrder.CurrencyID, oVehicleOrder.ETAValue, oVehicleOrder.ETATypeInInt, oVehicleOrder.IssueDate, oVehicleOrder.FeatureSetupName, oVehicleOrder.Remarks, oVehicleOrder.OrderStatusInInt, nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, VehicleOrder oVehicleOrder, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_VehicleOrder]"
                                    + "%n, %s, %n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n, %d,%s,%s,%n,%n,%n",
                                    oVehicleOrder.VehicleOrderID, oVehicleOrder.RefNo, oVehicleOrder.VehicleModelID, oVehicleOrder.BUID, oVehicleOrder.ChassisID, oVehicleOrder.EngineID, oVehicleOrder.ExteriorColorID, oVehicleOrder.InteriorColorID, oVehicleOrder.UpholsteryID, oVehicleOrder.TrimID, oVehicleOrder.WheelsID, oVehicleOrder.CurrencyID, oVehicleOrder.ETAValue, oVehicleOrder.ETATypeInInt, oVehicleOrder.IssueDate, oVehicleOrder.FeatureSetupName, oVehicleOrder.Remarks, oVehicleOrder.OrderStatusInInt, nUserID, (int)eEnumDBOperation);
        }



		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
            return tc.ExecuteReader("SELECT * FROM View_VehicleOrder WHERE VehicleOrderID=%n", nID);
		}
        public static IDataReader BUWiseGets(int buid, TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_VehicleOrder WHERE BUID = %n",buid);
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}
}
