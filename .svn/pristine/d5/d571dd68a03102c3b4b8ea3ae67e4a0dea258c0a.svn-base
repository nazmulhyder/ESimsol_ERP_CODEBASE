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
	public class VehicleModelDA 
	{
		#region Insert Update Delete Function 

		public static IDataReader InsertUpdate(TransactionContext tc, VehicleModel oVehicleModel, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_VehicleModel]"
                                    + "%n, %s,  %n,%n,%n,  %n,%n, %s,%s, %s,  %n,  %s,%s,%s,%s, %s,%s,%s, %s, %n, %n",
                                    oVehicleModel.VehicleModelID, oVehicleModel.ModelNo, oVehicleModel.DriveType, oVehicleModel.ModelCategoryID, oVehicleModel.ModelSessionID, oVehicleModel.MinPrice, oVehicleModel.MaxPrice, oVehicleModel.SeatingCapacity, oVehicleModel.Remarks, oVehicleModel.ModelShortName,
                                    oVehicleModel.CurrencyID, oVehicleModel.EngineType, oVehicleModel.MaxPowerOutput, oVehicleModel.MaximumTorque, oVehicleModel.Transmission, oVehicleModel.DisplacementCC, oVehicleModel.TopSpeed, oVehicleModel.Acceleration, oVehicleModel.ModelCode, nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, VehicleModel oVehicleModel, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_VehicleModel]"
                                   + "%n, %s,  %n,%n,%n,  %n,%n, %s,%s, %s,  %n,  %s,%s,%s,%s, %s,%s,%s,  %s, %n, %n",
                                    oVehicleModel.VehicleModelID, oVehicleModel.ModelNo, oVehicleModel.DriveType, oVehicleModel.ModelCategoryID, oVehicleModel.ModelSessionID, oVehicleModel.MinPrice, oVehicleModel.MaxPrice, oVehicleModel.SeatingCapacity, oVehicleModel.Remarks, oVehicleModel.ModelShortName,
                                    oVehicleModel.CurrencyID, oVehicleModel.EngineType, oVehicleModel.MaxPowerOutput, oVehicleModel.MaximumTorque, oVehicleModel.Transmission, oVehicleModel.DisplacementCC, oVehicleModel.TopSpeed, oVehicleModel.Acceleration, oVehicleModel.ModelCode, nUserID, (int)eEnumDBOperation);
        }


		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
            return tc.ExecuteReader("SELECT * FROM View_VehicleModel WHERE VehicleModelID=%n", nID);
		}
        public static IDataReader GetsByModelNo(string ModelNo, TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_VehicleModel WHERE ModelNo LIKE '%"+ModelNo+"%'");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
