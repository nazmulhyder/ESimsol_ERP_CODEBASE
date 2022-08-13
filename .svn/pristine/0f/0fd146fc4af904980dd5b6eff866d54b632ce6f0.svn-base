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
	public class KommFileDA 
	{
		#region Insert Update Delete Function
		public static IDataReader InsertUpdate(TransactionContext tc, KommFile oKommFile, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_KommFile]"
                                    + "%n,%s,%s,%s,%n, %n,%n,%d, %s,%s,%n,%n,  %n,%n,%n,  %n,%n,%n,  %n,%n,%n    ,%n,  %n,%n,%n",
                                    oKommFile.KommFileID, oKommFile.FileNo, oKommFile.KommNo, oKommFile.RefNo, oKommFile.VehicleOrderID, oKommFile.BUID, oKommFile.CurrencyID, oKommFile.IssueDate, oKommFile.FeatureSetupName, oKommFile.Remarks, oKommFile.UnitPrice, oKommFile.VatInPercent, oKommFile.RegistrationFeePercent,
                                    oKommFile.VehicleModelID,
                                    oKommFile.InteriorColorID,
                                    oKommFile.ExteriorColorID,
                                    oKommFile.UpholsteryID,
                                    oKommFile.TrimID,
                                    oKommFile.WheelsID,
                                    oKommFile.ChassisID,
                                    oKommFile.EngineID,
                                    oKommFile.ETAType,
                                    oKommFile.ETAValue,
                                    nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, KommFile oKommFile, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_KommFile]"
                                     + "%n,%s,%s,%s,%n, %n,%n,%d, %s,%s,%n,%n,  %n,%n,%n,  %n,%n,%n,  %n,%n,%n    ,%n,  %n,%n,%n",
                                    oKommFile.KommFileID, oKommFile.FileNo, oKommFile.KommNo, oKommFile.RefNo, oKommFile.VehicleOrderID, oKommFile.BUID, oKommFile.CurrencyID, oKommFile.IssueDate, oKommFile.FeatureSetupName, oKommFile.Remarks, oKommFile.UnitPrice, oKommFile.VatInPercent, oKommFile.RegistrationFeePercent,
                                    oKommFile.VehicleModelID,
                                    oKommFile.InteriorColorID,
                                    oKommFile.ExteriorColorID,
                                    oKommFile.UpholsteryID,
                                    oKommFile.TrimID,
                                    oKommFile.WheelsID,
                                    oKommFile.ChassisID,
                                    oKommFile.EngineID,
                                    oKommFile.ETAType,
                                    oKommFile.ETAValue,
                                    nUserID, (int)eEnumDBOperation);
        }
		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
            return tc.ExecuteReader("SELECT * FROM View_KommFile WHERE KommFileID=%n", nID);
		}
        public static IDataReader BUWiseGets(int buid, TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_KommFile WHERE BUID = %n Order By VehicleModelID", buid);
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		}
        public static IDataReader UpdateStatus(TransactionContext tc, KommFile oKommFile)
        {
            string sSQL1 = SQLParser.MakeSQL("Update KommFile Set KommFileStatus=%n WHERE KommFileID=%n", oKommFile.KommFileStatusInInt, oKommFile.KommFileID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_KommFile WHERE KommFileID=%n", oKommFile.KommFileID);
        }
		#endregion
	}

}
