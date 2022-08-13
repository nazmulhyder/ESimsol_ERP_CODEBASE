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
    public class RouteSheetDODA
    {
        public RouteSheetDODA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, RouteSheetDO oDO, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_RouteSheetDO]" + "%n, %n, %n, %n, %n, %n",
                                    oDO.RouteSheetDOID, oDO.RouteSheetID, oDO.DyeingOrderDetailID, oDO.Qty_RS,  nUserID, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, RouteSheetDO oDO, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_RouteSheetDO]" + "%n, %n, %n, %n, %n, %n",
                                      oDO.RouteSheetDOID, oDO.RouteSheetID, oDO.DyeingOrderDetailID, oDO.Qty_RS, nUserID, (int)eEnumDBOperation);
        }
        public static void DeleteByRS(TransactionContext tc, int nRouteSheetID, string sRouteSheetDOIDs)
        {
            tc.ExecuteNonQuery("Delete from RouteSheetDO where RouteSheetID=%n and RouteSheetDOID not in (%q)", nRouteSheetID, sRouteSheetDOIDs);
        }
        public static void UpdateRouteSheet(TransactionContext tc, int nRouteSheetID)
        {
            tc.ExecuteNonQuery("UPdate RouteSheet SET Qty = (SELECT SUM(ISNULL(Qty_RS,0)) FROM RouteSheetDO WHERE RouteSheetID = %n) WHERE RouteSheetID = %n", nRouteSheetID, nRouteSheetID);
        }
        
        #endregion


        #region Get & Exist Function

        public static IDataReader Get(int nID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_RouteSheetDO WHERE RouteSheetDOID=%n", nID);
        }

        public static IDataReader GetsBy(TransactionContext tc, int nRouteSheetID)
        {
            return tc.ExecuteReader("Select * from View_RouteSheetDO where RouteSheetID=%n ", nRouteSheetID);
        }
        public static IDataReader Gets(TransactionContext tc, int nPTUID)
        {
            return tc.ExecuteReader("Select * from View_RouteSheetDO where PTUID=%n", nPTUID);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsDOYetTORS(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}
