using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{

    public class RouteLocationDA
    {
        public RouteLocationDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, RouteLocation oRouteLocation, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_RouteLocation]" + "%n, %s, %s, %s, %n, %n, %n, %n",
                                    oRouteLocation.RouteLocationID, oRouteLocation.LocCode, oRouteLocation.Name, oRouteLocation.Description, oRouteLocation.LocationTypeInt, oRouteLocation.BUID, nUserId, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, RouteLocation oRouteLocation, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_RouteLocation]" + "%n, %s, %s, %s, %n, %n, %n, %n",
                                    oRouteLocation.RouteLocationID, oRouteLocation.LocCode, oRouteLocation.Name, oRouteLocation.Description, oRouteLocation.LocationTypeInt, oRouteLocation.BUID, nUserId, (int)eEnumDBOperation);
        }        
        #endregion

        #region Update Function
        public static void Update(TransactionContext tc, RouteLocation oRouteLocation)
        {
            tc.ExecuteNonQuery("UPDATE RouteLocation SET  LocCode=%s, Name=%s, Description=%s, LocationType =%n, BUID = %n WHERE RouteLocationID=%n",
                oRouteLocation.LocCode, oRouteLocation.Name, oRouteLocation.Description, (int)oRouteLocation.LocationType, oRouteLocation.BUID, oRouteLocation.RouteLocationID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM RouteLocation WHERE RouteLocationID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM RouteLocation");
        }

        public static IDataReader BUWiseGets(int BUID,  TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM RouteLocation WHERE BUID = "+BUID);
        }
        
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader Gets(TransactionContext tc, int nRouteLocation)
        {
            return tc.ExecuteReader("SELECT * FROM RouteLocation WHERE LocationType = %n", nRouteLocation);
        }
        public static bool IsExist_Name(TransactionContext tc, int nRouteLocationID, int nType, string sName)
        {
            object objRouteLocationID = tc.ExecuteScalar("Select RouteLocationID from RouteLocation where LocationType=%n and [Name]=%s ", (int)nType, sName);
            int nDBnRouteLocationID = 0;
            if (DBNull.Value == objRouteLocationID || objRouteLocationID == null) return false;
            else nDBnRouteLocationID = Convert.ToInt32(objRouteLocationID);

            return (nDBnRouteLocationID != nRouteLocationID);
        }
        #region Generation Function
        public static int GetNewID(TransactionContext tc)
        {
            return tc.GenerateID("RouteLocation", "RouteLocationID");
        }
        #endregion
        #endregion
    }
}