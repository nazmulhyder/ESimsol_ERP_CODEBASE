using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class FabricExecutionOrderSpecificationDA
    {
        public FabricExecutionOrderSpecificationDA() { }

        #region IUD
        public static IDataReader IUD(TransactionContext tc, FabricExecutionOrderSpecification oFEOS, int nDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricExecutionOrderSpecification] %n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%s,%n,%n,%n,%n, %s,%s,%s,%n,%n,%n,%n,%s,%n,%n,%n,%n,%n,%n,%b,%n,%s,%s,%D,%n,%b	,%n	,%n	,%n	,%n	,%n,%s,%b,%n,%n",
            oFEOS.FEOSID, oFEOS.FSCDID, oFEOS.ReferenceFSCDID, oFEOS.Reed, oFEOS.ReedWidth, oFEOS.FinishPick, oFEOS.GreigeFabricWidth, oFEOS.Ends, oFEOS.Picks,
            oFEOS.GreigeDemand, oFEOS.RequiredWarpLength, oFEOS.GroundEnds, oFEOS.WarpSet, oFEOS.SetLength, oFEOS.EndsRepeat, oFEOS.RepeatSection, oFEOS.SectionEnds, oFEOS.NoOfSection,
            oFEOS.PerConeLength, oFEOS.PerConeDia, oFEOS.LengthSpecification, oFEOS.GrayTargetInPercent, oFEOS.GrayWarpInPercent, oFEOS.Qty, oFEOS.Crimp, oFEOS.RefNote, oFEOS.WeftColor, oFEOS.WarpColor,
            oFEOS.SelvedgeEnds, oFEOS.ReqLoomProduction, oFEOS.FinishWidth, oFEOS.FinishEnd, oFEOS.NoOfFrame, oFEOS.SelvedgeEndTwo, oFEOS.Dent, oFEOS.TotalEnds, oFEOS.LoomPPAdd, oFEOS.WarpLenAdd, oFEOS.TotalEndsAdd, oFEOS.IsTEndsAdd, oFEOS.QtyExtraMet, oFEOS.WarpCount, oFEOS.WeftCount, oFEOS.IssueDate, oFEOS.ProdtionTypeInt, oFEOS.IsSepBeam, (int)oFEOS.SEBeamType, (int)oFEOS.FSpcType, oFEOS.TotalEndsUB, oFEOS.TotalEndsLB, oFEOS.ReqWarpLenLB, oFEOS.RefNo, oFEOS.IsOutSide, nUserId, nDBOperation);
        }
        public static IDataReader IUD_Log(TransactionContext tc, FabricExecutionOrderSpecification oFEOS, int nDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricExecutionOrderSpecificationLog] %n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%s,%n,%n,%n,%n, %s,%s,%s,%n,%n,%n,%n,%s,%n,%n,%n,%n,%n,%n,%b,%n,%s,%s,%D,%n,%b	,%n	,%n	,%n	,%n	,%n,%s,%d,%b,%n,%n",
          oFEOS.FEOSID, oFEOS.FSCDID, oFEOS.ReferenceFSCDID, oFEOS.Reed, oFEOS.ReedWidth, oFEOS.FinishPick, oFEOS.GreigeFabricWidth, oFEOS.Ends, oFEOS.Picks,
          oFEOS.GreigeDemand, oFEOS.RequiredWarpLength, oFEOS.GroundEnds, oFEOS.WarpSet, oFEOS.SetLength, oFEOS.EndsRepeat, oFEOS.RepeatSection, oFEOS.SectionEnds, oFEOS.NoOfSection,
          oFEOS.PerConeLength, oFEOS.PerConeDia, oFEOS.LengthSpecification, oFEOS.GrayTargetInPercent, oFEOS.GrayWarpInPercent, oFEOS.Qty, oFEOS.Crimp, oFEOS.RefNote, oFEOS.WeftColor, oFEOS.WarpColor,
          oFEOS.SelvedgeEnds, oFEOS.ReqLoomProduction, oFEOS.FinishWidth, oFEOS.FinishEnd, oFEOS.NoOfFrame, oFEOS.SelvedgeEndTwo, oFEOS.Dent, oFEOS.TotalEnds, oFEOS.LoomPPAdd, oFEOS.WarpLenAdd, oFEOS.TotalEndsAdd, oFEOS.IsTEndsAdd, oFEOS.QtyExtraMet, oFEOS.WarpCount, oFEOS.WeftCount, oFEOS.IssueDate, oFEOS.ProdtionTypeInt, oFEOS.IsSepBeam, (int)oFEOS.SEBeamType, (int)oFEOS.FSpcType, oFEOS.TotalEndsUB, oFEOS.TotalEndsLB, oFEOS.ReqWarpLenLB, oFEOS.RefNo, oFEOS.ReviseDate, oFEOS.IsRevise, nUserId, nDBOperation);

            //return tc.ExecuteReader("EXEC [SP_IUD_FabricExecutionOrderSpecificationLog]  %n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%d,%s,%n,%n,%n,%n, %s,%s,%s,%n,%n,%n,%n,%s,%n,%n,%n,%n,%n,%n,%b,%n,%s,%s,%D,%b,%b	,%n	,%n	,%n	,%n	,%n,%s,%n,%n",
            //oFEOS.FEOSID, oFEOS.FSCDID, oFEOS.ReferenceFSCDID, oFEOS.Reed, oFEOS.ReedWidth, oFEOS.FinishPick, oFEOS.GreigeFabricWidth, oFEOS.Ends, oFEOS.Picks,
            //oFEOS.GreigeDemand, oFEOS.RequiredWarpLength, oFEOS.GroundEnds, oFEOS.WarpSet, oFEOS.SetLength, oFEOS.EndsRepeat, oFEOS.RepeatSection, oFEOS.SectionEnds, oFEOS.NoOfSection,
            //oFEOS.PerConeLength, oFEOS.PerConeDia, oFEOS.ApproveDate, oFEOS.LengthSpecification, oFEOS.GrayTargetInPercent, oFEOS.GrayWarpInPercent, oFEOS.Qty, oFEOS.Crimp, oFEOS.RefNote, oFEOS.WeftColor, oFEOS.WarpColor,
            //oFEOS.SelvedgeEnds, oFEOS.ReqLoomProduction, oFEOS.FinishWidth, oFEOS.FinishEnd, oFEOS.NoOfFrame, oFEOS.SelvedgeEndTwo, oFEOS.Dent, oFEOS.TotalEnds, oFEOS.LoomPPAdd, oFEOS.WarpLenAdd, oFEOS.TotalEndsAdd, oFEOS.IsTEndsAdd, oFEOS.QtyExtraMet, oFEOS.WarpCount, oFEOS.WeftCount, oFEOS.ReviseDate, oFEOS.IsRevise, oFEOS.IsSepBeam, oFEOS.SEBeamType, oFEOS.FSpcType, oFEOS.TotalEndsUB, oFEOS.TotalEndsLB, oFEOS.ReqWarpLenLB,oFEOS.RefNo, nUserId, nDBOperation);
        }
        public static IDataReader UpdateOutSide(TransactionContext tc, FabricExecutionOrderSpecification oFEOS)
        {
            string sSQL1 = SQLParser.MakeSQL("Update FabricExecutionOrderSpecification Set IsOutSide =%b, ContractorID=%n, WarpWeftType=%n where FabricExecutionOrderSpecification.FEOSID=%n", oFEOS.IsOutSide, oFEOS.ContractorID,(int)oFEOS.WarpWeftType, oFEOS.FEOSID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("select * from View_FabricExecutionOrderSpecification where FEOSID=%n", oFEOS.FEOSID);
        }
        public static IDataReader IUD_DO(TransactionContext tc, FabricExecutionOrderSpecification oFEOS,  Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DyeingOrderFabric] %n,%n,%s,%n", oFEOS.FEOSID, oFEOS.FSCDID, oFEOS.ExeNo, nUserId);
        }
      
        #endregion
        
        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nFEOSID, Int64 nUserId)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricExecutionOrderSpecification WHERE FEOSID=%n", nFEOSID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL, Int64 nUserId)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion

    }
}
