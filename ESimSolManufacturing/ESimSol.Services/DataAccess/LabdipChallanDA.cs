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
	public class LabdipChallanDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, LabdipChallan oLabdipChallan, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_LabdipChallan]"
									+"%n,%d,%s,%n,%n,%n, %s, %n,%n",
                                    oLabdipChallan.LabdipChallanID, oLabdipChallan.ChallanDate, oLabdipChallan.ChallanNo, oLabdipChallan.ContractorID, oLabdipChallan.DeliveryZoneID, oLabdipChallan.StatusInt, oLabdipChallan.Remarks, nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, LabdipChallan oLabdipChallan, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_LabdipChallan]"
                                    + "%n,%d,%s,%n,%n,%n, %s,%n,%n",
                                    oLabdipChallan.LabdipChallanID, oLabdipChallan.ChallanDate, oLabdipChallan.ChallanNo, oLabdipChallan.ContractorID, oLabdipChallan.DeliveryZoneID, oLabdipChallan.StatusInt, oLabdipChallan.Remarks, nUserID, (int)eEnumDBOperation);
		}

        public static IDataReader UpdateStatus(TransactionContext tc, LabdipChallan oLabdipChallan, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LabdipChallan_Status]"
                                    + "%n, %n, %n, %n",
                                    oLabdipChallan.LabdipChallanID, oLabdipChallan.StatusInt,  nUserID, (int)eEnumDBOperation);
        }
		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_LabdipChallan WHERE LabdipChallanID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_LabdipChallan");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		}
        public static IDataReader UpdateLabDipDetails(TransactionContext tc, LabDipDetail oLabDipDetail)
        {
            string sSQL1 = SQLParser.MakeSQL("Update LabDipDetail Set LabDipChallanID=%n WHERE LabDipDetailID=%n", oLabDipDetail.LabdipChallanID, oLabDipDetail.LabDipDetailID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_LabDipDetail WHERE LabDipDetailID=%n", oLabDipDetail.LabDipDetailID);
        }
        public static void RemoveLabDipDetail(TransactionContext tc, int LDDID)
        {
            string sSQL1 = SQLParser.MakeSQL("Update LabDipDetail Set LabDipChallanID=0 WHERE LabDipDetailID=%n", LDDID);
            tc.ExecuteNonQuery(sSQL1);
        }
        #endregion
	}

}
