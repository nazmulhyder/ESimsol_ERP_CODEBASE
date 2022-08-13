using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class FabricDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, Fabric oFabric, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
             if(!string.IsNullOrEmpty(oFabric.Construction)){ oFabric.Construction = oFabric.Construction.Replace(" ", string.Empty);}
             if (!string.IsNullOrEmpty(oFabric.ConstructionPI)) { oFabric.ConstructionPI = oFabric.ConstructionPI.Replace(" ", string.Empty); }
           
            return tc.ExecuteReader("EXEC [SP_IUD_Fabric]"
                                    + "%n, %s, %s, %d, %n, %n, %n, %s, %s, %s, %b, %b, %b, %b, %n, %s, %n, %n, %d, %d, %d, %n, %n, %s, %n, %s, %n, %n, %n, %s, %n, %n, %s, %s, %n,%n, %s, %s",
                                    oFabric.FabricID, oFabric.FabricNo, oFabric.BuyerReference, oFabric.IssueDate, oFabric.BuyerID, oFabric.ProductID, oFabric.MKTPersonID, oFabric.Construction, oFabric.ColorInfo, oFabric.FabricWidth, oFabric.IsWash, oFabric.IsFinish, oFabric.IsDyeing, oFabric.IsPrint, oFabric.FinishType, oFabric.Remarks,
                                    oFabric.ApprovedBy, (int)oFabric.PriorityLevel, NullHandler.GetNullValue(oFabric.SeekingSubmissionDate), NullHandler.GetNullValue(oFabric.SubmissionDate), NullHandler.GetNullValue(oFabric.ReceiveDate), oFabric.ProcessType, oFabric.FabricWeave, oFabric.StyleNo, oFabric.FabricDesignID, oFabric.ConstructionPI, (int)eEnumDBOperation,
                                    nUserID, (int)oFabric.FabricOrderTypeInt, oFabric.HandLoomNo, oFabric.PrimaryLightSourceID, oFabric.SecondaryLightSourceID, oFabric.EndUse, oFabric.OptionNo, oFabric.NoOfFrame,oFabric.WeightDec, oFabric.WeftColor, oFabric.ActualConstruction);
        }

        public static void Delete(TransactionContext tc, Fabric oFabric, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_Fabric]"
                                    + "%n, %s, %s, %d, %n, %n, %n, %s, %s, %s, %b, %b, %b, %b, %n, %s, %n, %n, %d, %d, %d, %n, %n, %s, %n,%s, %n, %n, %n, %s, %n, %n, %s, %s, %n,%n, %s, %s",
                                    oFabric.FabricID, oFabric.FabricNo, oFabric.BuyerReference, oFabric.IssueDate, oFabric.BuyerID, oFabric.ProductID, oFabric.MKTPersonID, oFabric.Construction, oFabric.ColorInfo, oFabric.FabricWidth, oFabric.IsWash, oFabric.IsFinish, oFabric.IsDyeing, oFabric.IsPrint, oFabric.FinishType, oFabric.Remarks, 
                                    oFabric.ApprovedBy, (int)oFabric.PriorityLevel, NullHandler.GetNullValue(oFabric.SeekingSubmissionDate), NullHandler.GetNullValue(oFabric.SubmissionDate), NullHandler.GetNullValue(oFabric.ReceiveDate), oFabric.ProcessType, oFabric.FabricWeave, oFabric.StyleNo, oFabric.FabricDesignID, oFabric.ConstructionPI, (int)eEnumDBOperation,
                                    nUserID, oFabric.FabricOrderTypeInt, oFabric.HandLoomNo, oFabric.PrimaryLightSourceID, oFabric.SecondaryLightSourceID, oFabric.EndUse, oFabric.OptionNo, oFabric.NoOfFrame,oFabric.WeightDec, oFabric.WeftColor, oFabric.ActualConstruction);
        }

        public static IDataReader ReFabricSubmission(TransactionContext tc, Fabric oFabric , Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Fabric_ReFabricSubmission]"
                                    + "%n, %s, %s, %d, %n, %n, %n, %s, %s, %s, %b, %b, %b, %b, %n, %s, %n, %n, %d, %d, %d, %n, %n, %s, %n, %s, %s, %n, %n, %s, %n",
                                    oFabric.FabricID, oFabric.FabricNo, oFabric.BuyerReference, oFabric.IssueDate, oFabric.BuyerID, oFabric.ProductID, oFabric.MKTPersonID, oFabric.Construction, oFabric.ColorInfo, oFabric.FabricWidth, oFabric.IsWash, oFabric.IsFinish, oFabric.IsDyeing, oFabric.IsPrint, (int)oFabric.FinishType, oFabric.Remarks, oFabric.ApprovedBy, (int)oFabric.PriorityLevel, oFabric.SeekingSubmissionDate, oFabric.SubmissionDate, oFabric.ReceiveDate, oFabric.ProcessType, oFabric.FabricWeave, oFabric.StyleNo, oFabric.FabricDesignID, oFabric.ConstructionPI, oFabric.HandLoomNo, oFabric.PrimaryLightSourceID, oFabric.SecondaryLightSourceID, oFabric.EndUse, nUserID);
        }

        public static void SaveReceiveDate(TransactionContext tc, Fabric oFabric,Int64 nUserID)
        {
            tc.ExecuteNonQuery("UPDATE Fabric SET ReceiveDate = '" + oFabric.ReceiveDate.ToString("dd MMM yyyy hh:mm:ss") + "', HandLoomNo='"+ oFabric.HandLoomNo.Trim() +"' WHERE FabricID = " + oFabric.FabricID + "");
        }
        //public static void SaveSubmission(TransactionContext tc, Fabric oFabric, Int64 nUserID)
        //{
        //    tc.ExecuteNonQuery("UPDATE [dbo].[Fabric]   SET ProductID=" + oFabric.ProductID + ", [ColorInfo] = '" + oFabric.ColorInfo + "' ,[FabricWidth] ='" + oFabric.FabricWidth + "',[FinishType] =" + oFabric.FinishType + " ,[WarpCount] ='" + oFabric.WarpCount + "',[WeftCount] ='" + oFabric.WeftCount + "',[EPI] ='" + oFabric.EPI + "',[PPI] ='" + oFabric.PPI + "',[Construction] ='" + oFabric.Construction 
        //        + "',[ProcessType] =" + oFabric.ProcessType + " ,[FabricWeave] =" + oFabric.FabricWeave + " ,[FabricDesignID] = " + oFabric.FabricDesignID + ",[PrimaryLightSourceID] =" + oFabric.PrimaryLightSourceID + " ,[SecondaryLightSourceID] =" + oFabric.SecondaryLightSourceID + "  ,[EndUse] ='" + oFabric.EndUse + "',[OptionNo] ='" + oFabric.OptionNo + "' ,[NoOfFrame] = '" + oFabric.NoOfFrame + "'  ,[WeftColor] ='" + oFabric.WeftColor + "' ,[LastUpdateBy] = " + nUserID + "    ,[LastUpdateDateTime] = GetDate() ,[ActualConstruction] = '" + oFabric.ActualConstruction + "',FabricOrderType=" + (int)oFabric.FabricOrderTypeInt + ",ProductIDWeft=" + oFabric.ProductIDWeft + ",WeightAct=" + oFabric.WeightAct + ",WeightCal=" + oFabric.WeightCal + ",WeightDec=" + oFabric.WeightDec + ",FinishTypeSugg=" + oFabric.FinishTypeSugg + ",NoteRnD='" + oFabric.NoteRnD + "' WHERE FabricID=" + oFabric.FabricID);
        //}
        public static void SaveSubmission(TransactionContext tc, Fabric oFabric, Int64 nUserID)
        {
            tc.ExecuteNonQuery("UPDATE [dbo].[Fabric] SET ProductID=%n, [ColorInfo]=%s, [FabricWidth]=%s, FinishType=%n, WarpCount=%s, WeftCount=%s, [EPI]=%s, [PPI]=%s,Construction=%s     , [ProcessType]=%n,[FabricWeave] =%n,[FabricDesignID] =%n, [PrimaryLightSourceID] =%n, [SecondaryLightSourceID] =%n  ,[EndUse] =%s,[OptionNo] =%s ,[NoOfFrame] = %n ,[WeftColor] =%s,[LastUpdateBy] = %n    ,[LastUpdateDateTime] = %q,[ActualConstruction] = %s,FabricOrderType=%n,ProductIDWeft=%n,WeightAct=%n,WeightCal=%n,WeightDec=%n,FinishTypeSugg=%n,NoteRnD=%s   WHERE FabricID=%n",
               oFabric.ProductID,  oFabric.ColorInfo, oFabric.FabricWidth, oFabric.FinishType, oFabric.WarpCount, oFabric.WeftCount, oFabric.EPI, oFabric.PPI, oFabric.Construction, oFabric.ProcessType, oFabric.FabricWeave, oFabric.FabricDesignID, oFabric.PrimaryLightSourceID, oFabric.SecondaryLightSourceID, oFabric.EndUse, oFabric.OptionNo, oFabric.NoOfFrame, oFabric.WeftColor, nUserID, Global.DBDateTime, oFabric.ActualConstruction, (int)oFabric.FabricOrderTypeInt, oFabric.ProductIDWeft, oFabric.WeightAct, oFabric.WeightCal , oFabric.WeightDec, oFabric.FinishTypeSugg, oFabric.NoteRnD, oFabric.FabricID);

        }
        public static IDataReader Received(TransactionContext tc, Fabric oFabric, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Fabric_HandLoomNo]" + "%n, %s, %n", oFabric.FabricID, oFabric.HandLoomNo, nUserID);
        }
        public static void SaveSubmissionDate(TransactionContext tc, Fabric oFabric, Int64 nUserID)
        {
            tc.ExecuteNonQuery("UPDATE Fabric SET SubmissionDate = '" + oFabric.SubmissionDate.ToString("dd MMM yyyy hh:mm:ss") + "' WHERE FabricID = " + oFabric.FabricID + "");
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_Fabric WHERE FabricID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_Fabric");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);//use View_Fabric
        }

        public static void StatusChange(TransactionContext tc, int nFabricId, long nUserId)
        {
            tc.ExecuteNonQuery("UPDATE Fabric SET ApprovedBy = %n, ApprovedByDate=%d WHERE FabricID = %n", nUserId, DateTime.Now, nFabricId);
        }

        public static bool HasHandLoomNo(TransactionContext tc, int nFabricId, string sHandLoomNo)
        {
            object obj = tc.ExecuteScalar("Select Count(*) from Fabric Where HandLoomNo ='" + sHandLoomNo + "' And FabricID!=" + nFabricId + "");
            Int64 count=Convert.ToInt64(obj);

            if(count>0)
                return true;
            else 
                return false;
        }
        
        #endregion
    }  
}
