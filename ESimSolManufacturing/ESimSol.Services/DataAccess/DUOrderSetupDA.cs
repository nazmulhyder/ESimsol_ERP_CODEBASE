using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{

    public class DUOrderSetupDA
    {
        public DUOrderSetupDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, DUOrderSetup oDUOrderSetup, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_DUOrderSetup]"
                                    + "%n,%n, %s,%s,%s, %s, %s,%s, %n,%b,%b,%b,%n,%n, %n,%b,%b,%b,%b,%b,  %n, %n,%b, %b, %n, %n,%n,%n, %n",
                                    oDUOrderSetup.DUOrderSetupID, oDUOrderSetup.OrderType, oDUOrderSetup.ShortName, oDUOrderSetup.NoCode, oDUOrderSetup.DONoCode, oDUOrderSetup.OrderName, oDUOrderSetup.PrintName, oDUOrderSetup.NoteFixed, oDUOrderSetup.PrintNo, oDUOrderSetup.IsPIMendatory, oDUOrderSetup.IsApplyOutside, oDUOrderSetup.Activity, oDUOrderSetup.MUnitID, oDUOrderSetup.MUnitID_Alt, oDUOrderSetup.BUID, oDUOrderSetup.IsRateMendatory, oDUOrderSetup.IsInvoiceMendatory, oDUOrderSetup.IsApplyDyeingStep, oDUOrderSetup.IsSaveLabDip, oDUOrderSetup.IsApplyFabric, oDUOrderSetup.CurrencyID, oDUOrderSetup.ComboNo, oDUOrderSetup.IsInHouse, oDUOrderSetup.IsOpenRawLot, oDUOrderSetup.ComboNoDC,oDUOrderSetup.DeliveryGrace,oDUOrderSetup.DeliveryValidation, nUserId, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, DUOrderSetup oDUOrderSetup, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_DUOrderSetup]"
                                    + "%n,%n, %s,%s,%s, %s, %s,%s, %n,%b,%b,%b,%n,%n, %n,%b,%b,%b,%b,%b,  %n, %n,%b, %b, %n, %n,%n,%n, %n",
                                    oDUOrderSetup.DUOrderSetupID, oDUOrderSetup.OrderType, oDUOrderSetup.ShortName, oDUOrderSetup.NoCode, oDUOrderSetup.DONoCode, oDUOrderSetup.OrderName, oDUOrderSetup.PrintName, oDUOrderSetup.NoteFixed, oDUOrderSetup.PrintNo, oDUOrderSetup.IsPIMendatory, oDUOrderSetup.IsApplyOutside, oDUOrderSetup.Activity, oDUOrderSetup.MUnitID, oDUOrderSetup.MUnitID_Alt, oDUOrderSetup.BUID, oDUOrderSetup.IsRateMendatory, oDUOrderSetup.IsInvoiceMendatory, oDUOrderSetup.IsApplyDyeingStep, oDUOrderSetup.IsSaveLabDip, oDUOrderSetup.IsApplyFabric, oDUOrderSetup.CurrencyID, oDUOrderSetup.ComboNo, oDUOrderSetup.IsInHouse, oDUOrderSetup.IsOpenRawLot, oDUOrderSetup.ComboNoDC, oDUOrderSetup.DeliveryGrace, oDUOrderSetup.DeliveryValidation, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_DUOrderSetup WHERE DUOrderSetupID=%n", nID);
        }

        public static IDataReader GetByType(TransactionContext tc, int nOrderType)
        {
            return tc.ExecuteReader("SELECT * FROM View_DUOrderSetup WHERE OrderType=%n and Activity=1", nOrderType);
        }
        public static IDataReader GetByOrderTypes(TransactionContext tc,int nBUID, bool bIsInHouse, string sOrderType)
        {
            return tc.ExecuteReader("SELECT * FROM View_DUOrderSetup WHERE Activity=1 and BUID=%n and IsInHouse=%b and OrderType in (%q) order by ComboNo",nBUID, bIsInHouse, sOrderType);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_DUOrderSetup order by ComboNo");
        }
        public static IDataReader GetsActive(TransactionContext tc, int nBUID)
        {
            string sSQL = "SELECT * FROM View_DUOrderSetup WHERE Activity=1";
            if (nBUID > 0) { sSQL = sSQL + " and BUID="+nBUID;}
            sSQL=sSQL+" order by ComboNo";
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Activate(TransactionContext tc, DUOrderSetup oDUOrderSetup)
        {
            string sSQL1 = SQLParser.MakeSQL("Update DUOrderSetup Set Activity=~Activity WHERE DUOrderSetupID=%n", oDUOrderSetup.DUOrderSetupID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_DUOrderSetup WHERE DUOrderSetupID=%n", oDUOrderSetup.DUOrderSetupID);

        }
    
     
    
        #endregion
    }
}