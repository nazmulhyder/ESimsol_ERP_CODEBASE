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

    public class TAPDA
    {
        public TAPDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, TAP oTAP, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_TAP]"
                                    + "%n, %s, %n,%n,%d,%n,%n,%n,%n,%n,%n,%n, %n, %d, %n, %s, %s, %d,%n, %n, %n",
                                    oTAP.TAPID, oTAP.PlanNo, (int)oTAP.TAPStatus, oTAP.TechnicalSheetID, oTAP.PODate, oTAP.OrderQty, oTAP.LeadTime, oTAP.ApprovalDuration, oTAP.ProductionLeadTime, oTAP.FabricLeadTime, oTAP.ProductionTime, oTAP.BufferingDays, oTAP.OrderRecapID, oTAP.PlanDate, oTAP.PlanBy, oTAP.TampleteName, oTAP.Remarks, oTAP.ShipmentDate, oTAP.BUID, (int)eEnumDBOperation, nUserID);
        }
        public static void Delete(TransactionContext tc, TAP oTAP, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_TAP]"
                                    + "%n, %s, %n,%n,%d,%n,%n,%n,%n,%n,%n,%n, %n, %d, %n, %s, %s, %d,%n, %n, %n",
                                    oTAP.TAPID, oTAP.PlanNo, (int)oTAP.TAPStatus, oTAP.TechnicalSheetID, oTAP.PODate, oTAP.OrderQty, oTAP.LeadTime, oTAP.ApprovalDuration, oTAP.ProductionLeadTime, oTAP.FabricLeadTime, oTAP.ProductionTime, oTAP.BufferingDays, oTAP.OrderRecapID, oTAP.PlanDate, oTAP.PlanBy, oTAP.TampleteName, oTAP.Remarks, oTAP.ShipmentDate, oTAP.BUID, (int)eEnumDBOperation, nUserID);
        }

        public static IDataReader AcceptRevise(TransactionContext tc, TAP oTAP, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_AcceptTAPRevise]" + "%n, %d, %s, %n", oTAP.TAPID, oTAP.ShipmentDate, oTAP.Remarks, nUserID);
        }


        #region Factory TAP Insert Update  
        public static IDataReader InsertUpdateSaveFactoryTAP(TransactionContext tc, TAP oTAP, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FactoryTAP]"
                                    + "%n, %s,%n, %n,%d,%n,%s,%n,%n,%n,%n",
                                    oTAP.TAPID, oTAP.PlanNo, (int)oTAP.TAPStatus,  oTAP.OrderRecapID, oTAP.PlanDate, oTAP.PlanBy, oTAP.Remarks, oTAP.ApprovedBy, oTAP.BUID,  (int)eEnumDBOperation, nUserID);
        }

        #endregion


        public static IDataReader ChangeStatus(TransactionContext tc, TAP oTAP, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC[SP_TAPOperation]" + "%n, %n, %s, %n, %n, %n",
                                  oTAP.TAPID, (int)oTAP.TAPStatus, oTAP.Remarks, (int)oTAP.TAPActionType, nUserID, (int)eEnumDBOperation);
        }

        public static IDataReader RearangeTAPSequence(TransactionContext tc, int  nTAPID)
        {
            return tc.ExecuteReader("EXEC[SP_RearrangeTAPSequence]" + "%n", nTAPID);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_TAP WHERE TAPID=%n", nID);
        }

        public static IDataReader GetByRecap(TransactionContext tc, long nORID)
        {
            return tc.ExecuteReader("SELECT top 1 * FROM View_TAP WHERE OrderRecapID=%n", nORID);
        }
        
        public static IDataReader GetByHIA(TransactionContext tc, long nHIAID)
        {
            return tc.ExecuteReader("SELECT top 1 * FROM View_TAP WHERE OrderRecapID = ( SELECT OperationObjectID AS OrderRecapID FROM View_HumanInteractionAgent WHERE HIAID = %n)", nHIAID);
        }
        
        public static IDataReader GetFactoryTAP(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FactoryTAP WHERE TAPID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_TAP");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }  
    
  
}
