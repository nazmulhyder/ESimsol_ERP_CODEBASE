using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class FabricRnDDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FabricRnD oFabricRnD, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricRnD]"
                                    + "%n,%n,%n,    %s,%n,%n,       %n,%n,%s,       %s,%s,%s        ,%n,%n,%n,	    %n,%n,%s,	     %s,%s,%s        ,%s,%s,%n,      %n,%n,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,  %n,%n",
                                    oFabricRnD.FabricRnDID, oFabricRnD.FabricID, oFabricRnD.FSCDID,
                                    oFabricRnD.FabricWidth, oFabricRnD.FinishType, oFabricRnD.FabricWeave, 
                                    oFabricRnD.ProcessType, oFabricRnD.FabricDesignID, oFabricRnD.WeftColor, 
                                    oFabricRnD.ConstructionSuggest, oFabricRnD.WarpCount, oFabricRnD.WeftCount, 
                                    oFabricRnD.ProductIDWarp, oFabricRnD.ProductIDWeft, oFabricRnD.WeightAct, 
                                    oFabricRnD.WeightCal, oFabricRnD.WeightDec, oFabricRnD.EPI, 
                                    oFabricRnD.PPI, oFabricRnD.YarnQuality, oFabricRnD.YarnFly,
                                    oFabricRnD.Note, oFabricRnD.ProductWarpRnd_Suggest, oFabricRnD.FabricWeaveSuggest, 
                                    oFabricRnD.WashType, oFabricRnD.ShadeType, oFabricRnD.CrimpWP, 
                                    oFabricRnD.CrimpWF, oFabricRnD.Growth, oFabricRnD.Recovy,
                                    oFabricRnD.Elongation, oFabricRnD.SlubLengthWP, oFabricRnD.PauseLengthWP, 
                                    oFabricRnD.SlubDiaWP, oFabricRnD.SlubLengthWF, oFabricRnD.PauseLengthWF, 
                                    oFabricRnD.SlubDiaWF, oFabricRnD.WidthGrey, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, FabricRnD oFabricRnD, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricRnD]"
                                    + "%n,%n,%n,    %s,%n,%n,       %n,%n,%s,       %s,%s,%s        ,%n,%n,%n,	    %n,%n,%s,	     %s,%s,%s        ,%s,%s,%n,      %n,%n,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,  %n,%n",
                                    oFabricRnD.FabricRnDID, oFabricRnD.FabricID, oFabricRnD.FSCDID, oFabricRnD.FabricWidth, oFabricRnD.FinishType, oFabricRnD.FabricWeave, oFabricRnD.ProcessType, oFabricRnD.FabricDesignID, oFabricRnD.WeftColor, oFabricRnD.ConstructionSuggest, oFabricRnD.WarpCount, oFabricRnD.WeftCount, oFabricRnD.ProductIDWarp, oFabricRnD.ProductIDWeft, oFabricRnD.WeightAct, oFabricRnD.WeightCal, oFabricRnD.WeightDec, oFabricRnD.EPI, oFabricRnD.PPI, oFabricRnD.YarnQuality, oFabricRnD.YarnFly, oFabricRnD.Note, oFabricRnD.ProductWarpRnd_Suggest, oFabricRnD.FabricWeaveSuggest,
                                    oFabricRnD.WashType, oFabricRnD.ShadeType, oFabricRnD.CrimpWP, oFabricRnD.CrimpWF, oFabricRnD.Growth, oFabricRnD.Recovy, oFabricRnD.Elongation, oFabricRnD.SlubLengthWP, oFabricRnD.PauseLengthWP, oFabricRnD.SlubDiaWP, oFabricRnD.SlubLengthWF, oFabricRnD.PauseLengthWF, oFabricRnD.SlubDiaWF, oFabricRnD.WidthGrey,
                                    nUserID, (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricRnD WHERE FabricRnDID=%n", nID);
        }
        public static IDataReader GetBy(TransactionContext tc, int nFSCDID, long nFabricID)
        {
            if (nFSCDID > 0) { return tc.ExecuteReader("SELECT * FROM View_FabricRnD WHERE FabricID=%n and FSCDID=%n", nFabricID, nFSCDID); }
            else { return tc.ExecuteReader("SELECT * FROM View_FabricRnD WHERE FabricID=%n", nFabricID); }
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricRnD");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);//use View_FabricRnD
        }

        public static void StatusChange(TransactionContext tc, int nFabricRnDId, long nUserId)
        {
            tc.ExecuteNonQuery("UPDATE FabricRnD SET ApprovedBy = %n, ApprovedByDate=%d WHERE FabricRnDID = %n", nUserId, DateTime.Now, nFabricRnDId);
        }

        public static bool HasHandLoomNo(TransactionContext tc, int nFabricRnDId, string sHandLoomNo)
        {
            object obj = tc.ExecuteScalar("Select Count(*) from FabricRnD Where HandLoomNo ='" + sHandLoomNo + "' And FabricRnDID!=" + nFabricRnDId + "");
            Int64 count=Convert.ToInt64(obj);

            if(count>0)
                return true;
            else 
                return false;
        }
        
        #endregion
    }  
}
