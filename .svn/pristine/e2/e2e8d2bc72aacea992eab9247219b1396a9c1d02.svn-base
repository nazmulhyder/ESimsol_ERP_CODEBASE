using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
    public class MenuDA
    {
        public MenuDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, Menu oMenu, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_Menu]"
                                    + "%n, %n, %s, %s, %s,%s, %s, %n, %n,%n,%b, %b,%n,%n,%n",
                                    oMenu.MenuID, oMenu.ParentID, oMenu.MenuName, oMenu.ActionName, oMenu.ControllerName, oMenu.CallingPerameterValue, oMenu.CallingPerameterType, (int)oMenu.ApplicationType, oMenu.MenuSequence, oMenu.BUID, oMenu.IsWithBU, oMenu.IsActive, (int)oMenu.ModuleName, nUserId, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, Menu oMenu, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_Menu]"
                                    + "%n, %n, %s, %s, %s,%s, %s, %n, %n,%n,%b, %b,%n,%n,%n",
                                    oMenu.MenuID, oMenu.ParentID, oMenu.MenuName, oMenu.ActionName, oMenu.ControllerName, oMenu.CallingPerameterValue, oMenu.CallingPerameterType, (int)oMenu.ApplicationType, oMenu.MenuSequence, oMenu.BUID, oMenu.IsWithBU, oMenu.IsActive, (int)oMenu.ModuleName, nUserId, (int)eEnumDBOperation);
        }

        public static void UpdateSequence(TransactionContext tc, Menu oMenu)
        {
            tc.ExecuteNonQuery("Update Menu SET MenuSequence = %n WHERE MenuID = %n", oMenu.MenuSequence, oMenu.MenuID);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_Menu WHERE MenuID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc, int nEnumApplicationType)
        {
            return tc.ExecuteReader("SELECT * FROM View_Menu WHERE ApplicationType=%n Order By MenuSequence", nEnumApplicationType);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_Menu Order By MenuSequence");
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader Get(TransactionContext tc, string ActionName, string ControllerName)
        {
            return tc.ExecuteReader("Select top(1)* from View_Menu Where ActionName=%s And ControllerName=%s", ActionName, ControllerName);
        }
        
        #endregion
    }
}
