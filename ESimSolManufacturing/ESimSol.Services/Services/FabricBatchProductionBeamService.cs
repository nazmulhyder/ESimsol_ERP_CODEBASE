using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class FabricBatchProductionBeamService : MarshalByRefObject, IFabricBatchProductionBeamService
    {
        #region Private functions and declaration
        private FabricBatchProductionBeam MapObject(NullHandler oReader)
        {
            FabricBatchProductionBeam oFabricBatchProductionBeam = new FabricBatchProductionBeam();
            oFabricBatchProductionBeam.FBPBeamID = oReader.GetInt32("FBPBeamID");
            oFabricBatchProductionBeam.FBPID = oReader.GetInt32("FBPID");
            oFabricBatchProductionBeam.FBPDetailID = oReader.GetInt32("FBPDetailID");
            oFabricBatchProductionBeam.BeamID = oReader.GetInt32("BeamID");
            oFabricBatchProductionBeam.Qty = oReader.GetDouble("Qty");
            oFabricBatchProductionBeam.QtyM = oReader.GetDouble("QtyM");
            oFabricBatchProductionBeam.IsFinish = oReader.GetBoolean("IsFinish");
            oFabricBatchProductionBeam.FBID = oReader.GetInt32("FBID");
            oFabricBatchProductionBeam.WeavingProcessType = (EnumWeavingProcess)oReader.GetInt32("WeavingProcessType");
            oFabricBatchProductionBeam.BeamName = oReader.GetString("BeamName");
            oFabricBatchProductionBeam.BeamNo = oReader.GetString("BeamNo");
            oFabricBatchProductionBeam.BuyerID = oReader.GetInt32("BuyerID");
            oFabricBatchProductionBeam.FabricSalesContractDetailID = oReader.GetInt32("FabricSalesContractDetailID");
            oFabricBatchProductionBeam.FEONo = oReader.GetString("FEONo");
            oFabricBatchProductionBeam.IsInHouse = oReader.GetBoolean("IsInHouse");
            oFabricBatchProductionBeam.OrderType = (EnumOrderType)oReader.GetInt32("OrderType");
            oFabricBatchProductionBeam.Construction = oReader.GetString("Construction");
            oFabricBatchProductionBeam.BuyerName = oReader.GetString("BuyerName");
            oFabricBatchProductionBeam.TotalEnds = oReader.GetInt32("TotalEnds");
            oFabricBatchProductionBeam.Weave = oReader.GetString("Weave");
            oFabricBatchProductionBeam.ReedCount = oReader.GetInt32("ReedCount");
            oFabricBatchProductionBeam.MachineStatus = (EnumMachineStatus)oReader.GetInt32("MachineStatus");
            oFabricBatchProductionBeam.FBStatus = (EnumFabricBatchState)oReader.GetInt32("FBStatus");
            oFabricBatchProductionBeam.StartTime = oReader.GetDateTime("StartTime"); 
            oFabricBatchProductionBeam.EndTime = oReader.GetDateTime("EndTime"); 
            oFabricBatchProductionBeam.FbpIdForWeaving = oReader.GetInt32("FbpIdForWeaving");
            oFabricBatchProductionBeam.BatchNo = oReader.GetString("BatchNo");
            oFabricBatchProductionBeam.MachineWiseQty = oReader.GetDouble("MachineWiseQty");
            oFabricBatchProductionBeam.FEOQty = oReader.GetDouble("FEOQty");
            oFabricBatchProductionBeam.FBPQty = oReader.GetDouble("FBPQty");
            oFabricBatchProductionBeam.BatchQty = oReader.GetDouble("QtyBatch");
            oFabricBatchProductionBeam.RPM = oReader.GetInt32("RPM");
            oFabricBatchProductionBeam.RCount = oReader.GetDouble("RCount");
            oFabricBatchProductionBeam.Dent = oReader.GetString("Dent");
            oFabricBatchProductionBeam.NoOfColor = oReader.GetInt32("NoOfColor");
            oFabricBatchProductionBeam.IsYarnDyed = oReader.GetBoolean("IsYarnDyed");
            oFabricBatchProductionBeam.WarpColor = oReader.GetString("WarpColor");
            oFabricBatchProductionBeam.WeftColor = oReader.GetString("WeftColor");
            oFabricBatchProductionBeam.LoomDent = oReader.GetString("LoomDent");
            oFabricBatchProductionBeam.LoomReedCount = oReader.GetDouble("LoomReedCount");
            oFabricBatchProductionBeam.ChildMachineTypeID = oReader.GetInt32("ChildMachineTypeID");
            oFabricBatchProductionBeam.ChildMachineTypeName = oReader.GetString("ChildMachineTypeName");
            oFabricBatchProductionBeam.FBPMachineID = oReader.GetInt32("FBPMachineID");
            oFabricBatchProductionBeam.FBPMachineName = oReader.GetString("FBPMachineName");
            oFabricBatchProductionBeam.IsDirect = oReader.GetBoolean("IsDirect");
            oFabricBatchProductionBeam.ParentMachineTypeName = oReader.GetString("ParentMachineTypeName");
            oFabricBatchProductionBeam.IsDrawing = oReader.GetInt32("IsDrawing");
            oFabricBatchProductionBeam.FEOID = oReader.GetInt32("FEOID");
            oFabricBatchProductionBeam.FEOSID = oReader.GetInt32("FEOSID");
            oFabricBatchProductionBeam.FSpcType = (EnumFabricSpeType)oReader.GetInt32("FSpcType");
            oFabricBatchProductionBeam.PlanType = (EnumPlanType)oReader.GetInt32("PlanType");

            return oFabricBatchProductionBeam;
        }
        private FabricBatchProductionBeam CreateObject(NullHandler oReader)
        {
            FabricBatchProductionBeam oFabricBatchProductionBeam = new FabricBatchProductionBeam();
            oFabricBatchProductionBeam = MapObject(oReader);
            return oFabricBatchProductionBeam;
        }
        public List<FabricBatchProductionBeam> CreateObjects(IDataReader oReader)
        {
            List<FabricBatchProductionBeam> oFabricBatchProductionBeam = new List<FabricBatchProductionBeam>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricBatchProductionBeam oItem = CreateObject(oHandler);
                oFabricBatchProductionBeam.Add(oItem);
            }
            return oFabricBatchProductionBeam;
        }

        #endregion

        #region Interface implementation
        public FabricBatchProductionBeam Save(FabricBatchProductionBeam oFabricBatchProductionBeam, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFabricBatchProductionBeam.FBPBeamID <= 0)
                {
                    reader = FabricBatchProductionBeamDA.InsertUpdate(tc, oFabricBatchProductionBeam, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = FabricBatchProductionBeamDA.InsertUpdate(tc, oFabricBatchProductionBeam, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricBatchProductionBeam = new FabricBatchProductionBeam();
                    oFabricBatchProductionBeam = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricBatchProductionBeam.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricBatchProductionBeam;
        }
        public FabricBatchProductionBeam Finish(FabricBatchProductionBeam oFabricBatchProductionBeam, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricBatchProductionBeamDA.Finish(tc, oFabricBatchProductionBeam.FBPBeamID, oFabricBatchProductionBeam.IsFinish);

                IDataReader reader;
                reader = FabricBatchProductionBeamDA.Get(tc, oFabricBatchProductionBeam.FBPBeamID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricBatchProductionBeam = new FabricBatchProductionBeam();
                    oFabricBatchProductionBeam = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricBatchProductionBeam.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricBatchProductionBeam;
        }
        public FabricBatchProductionBeam TransferFinishBeam(FabricBatchProductionBeam oFabricBatchProductionBeam, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
               

                IDataReader reader;
                reader = FabricBatchProductionBeamDA.TransferFinishBeam(tc, oFabricBatchProductionBeam.FBPBeamID, oFabricBatchProductionBeam.BeamID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricBatchProductionBeam = new FabricBatchProductionBeam();
                    oFabricBatchProductionBeam = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricBatchProductionBeam.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricBatchProductionBeam;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricBatchProductionBeam oFabricBatchProductionBeam = new FabricBatchProductionBeam();
                oFabricBatchProductionBeam.FBPBeamID = id;
                DBTableReferenceDA.HasReference(tc, "FabricBatchProductionBeam", id);
                FabricBatchProductionBeamDA.Delete(tc, oFabricBatchProductionBeam, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                #endregion
            }
            return Global.DeleteMessage;
        } 
        public List<FabricBatchProductionBeam> GetsByFabricBatchProduction(int nFBPID, Int64 nUserID)
        {
            List<FabricBatchProductionBeam> oFabricBatchProductionBeams = new List<FabricBatchProductionBeam>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricBatchProductionBeamDA.GetsByFabricBatchProduction(tc, nFBPID);
                oFabricBatchProductionBeams = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricBatchProductionBeams = new List<FabricBatchProductionBeam>();
                FabricBatchProductionBeam oFabricBatchProductionBeam = new FabricBatchProductionBeam();
                oFabricBatchProductionBeam.ErrorMessage = e.Message.Split('~')[0];
                oFabricBatchProductionBeams.Add(oFabricBatchProductionBeam);
                #endregion
            }
            return oFabricBatchProductionBeams;
        }
        public List<FabricBatchProductionBeam> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricBatchProductionBeam> oFabricBatchProductionBeams = new List<FabricBatchProductionBeam>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricBatchProductionBeamDA.Gets(tc, sSQL);
                oFabricBatchProductionBeams = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricBatchProductionBeams = new List<FabricBatchProductionBeam>();
                FabricBatchProductionBeam oFabricBatchProductionBeam = new FabricBatchProductionBeam();
                oFabricBatchProductionBeam.ErrorMessage = e.Message.Split('~')[0];
                oFabricBatchProductionBeams.Add(oFabricBatchProductionBeam);
                #endregion
            }
            return oFabricBatchProductionBeams;
        }
        public List<FabricBatchProductionBeam> Gets(Int64 nUserID)
        {
            List<FabricBatchProductionBeam> oFabricBatchProductionBeams = new List<FabricBatchProductionBeam>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricBatchProductionBeamDA.Gets(tc);
                oFabricBatchProductionBeams = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricBatchProductionBeams = new List<FabricBatchProductionBeam>();
                FabricBatchProductionBeam oFabricBatchProductionBeam = new FabricBatchProductionBeam();
                oFabricBatchProductionBeam.ErrorMessage = e.Message.Split('~')[0];
                oFabricBatchProductionBeams.Add(oFabricBatchProductionBeam);
                #endregion
            }
            return oFabricBatchProductionBeams;
        }
        public List<FabricBatchProductionBeam> GetsFinishedBeams(Int64 nUserID)
        {
            List<FabricBatchProductionBeam> oFabricBatchProductionBeams = new List<FabricBatchProductionBeam>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricBatchProductionBeamDA.GetsFinishedBeams(tc);
                oFabricBatchProductionBeams = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricBatchProductionBeams = new List<FabricBatchProductionBeam>();
                FabricBatchProductionBeam oFabricBatchProductionBeam = new FabricBatchProductionBeam();
                oFabricBatchProductionBeam.ErrorMessage = e.Message.Split('~')[0];
                oFabricBatchProductionBeams.Add(oFabricBatchProductionBeam);
                #endregion
            }
            return oFabricBatchProductionBeams;
        }
        public FabricBatchProductionBeam Get(int id, Int64 nUserId)
        {
            FabricBatchProductionBeam oFabricBatchProductionBeam = new FabricBatchProductionBeam();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricBatchProductionBeamDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricBatchProductionBeam = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricBatchProductionBeam.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricBatchProductionBeam;
        }

        public List<FabricBatchProductionBeam> DrawingLeasingOperation(List<FabricBatchProductionBeam> oFabricBatchProductionBeams, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<FabricBatchProductionBeam> oFBPBs = new List<FabricBatchProductionBeam>();
            FabricBatchProductionBeam oFabricBatchProductionBeam = new FabricBatchProductionBeam();
            try
            {
                tc = TransactionContext.Begin(true);
                foreach (FabricBatchProductionBeam oItem in oFabricBatchProductionBeams)
                {
                    IDataReader reader;
                    reader = FabricBatchProductionBeamDA.InsertUpdate(tc, oItem, EnumDBOperation.Active, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFabricBatchProductionBeam = new FabricBatchProductionBeam();
                        oFabricBatchProductionBeam = CreateObject(oReader);
                        oFBPBs.Add(oFabricBatchProductionBeam);
                    }
                    reader.Close();
                }
                
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricBatchProductionBeam.ErrorMessage = e.Message.Split('~')[0];
                oFBPBs = new List<FabricBatchProductionBeam>();
                oFBPBs.Add(oFabricBatchProductionBeam);
                #endregion
            }
            return oFBPBs;
        }

        #endregion
    }
}
