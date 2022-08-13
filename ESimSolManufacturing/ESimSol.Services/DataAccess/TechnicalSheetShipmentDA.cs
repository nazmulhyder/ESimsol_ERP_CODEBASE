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
	public class TechnicalSheetShipmentDA 
	{
		#region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, TechnicalSheetShipment oTechnicalSheetShipment, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sTechnicalSheetShipmentID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_TechnicalSheetShipment]"
									+"%n,%n,%s,%n,%s,%n,%n,%s",
                                    oTechnicalSheetShipment.TechnicalSheetShipmentID, oTechnicalSheetShipment.TechnicalSheetID, oTechnicalSheetShipment.DeliveryDate, oTechnicalSheetShipment.Qty, oTechnicalSheetShipment.Remarks, nUserID, (int)eEnumDBOperation, sTechnicalSheetShipmentID);
		}

        public static void Delete(TransactionContext tc, TechnicalSheetShipment oTechnicalSheetShipment, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sTechnicalSheetShipmentID)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_TechnicalSheetShipment]"
                                    + "%n,%n,%s,%n,%s,%n,%n,%s",
                                    oTechnicalSheetShipment.TechnicalSheetShipmentID, oTechnicalSheetShipment.TechnicalSheetID, oTechnicalSheetShipment.DeliveryDate, oTechnicalSheetShipment.Qty, oTechnicalSheetShipment.Remarks, nUserID, (int)eEnumDBOperation, sTechnicalSheetShipmentID);
        }


		#endregion

		#region Get & Exist Function
		public static IDataReader Gets(int TechnicalSheetID, TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_TechnicalSheetShipment WHERE TechnicalSheetID = %n", TechnicalSheetID);
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
